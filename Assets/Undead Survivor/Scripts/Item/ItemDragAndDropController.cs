using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyVampireSurvivors
{
    public class ItemDragAndDropController : MonoBehaviour
    {
        #region Variables
        [SerializeField] private GameObject itemIcon;   // 아이템 아이콘을 나타내는 GameObject
        public ItemSlot itemSlot;     // 드래그 및 드롭을 위한 현재 아이템 슬롯
        private RectTransform iconTransform;            // 아이콘의 RectTransform (UI 위치 및 크기)
        private Image itemIconImage;                    // 아이콘의 이미지 컴포넌트
        public bool isDraging = false;                  // 드래그 중인지 여부를 나타내는 플래그

        private ItemSlot originSlot; // 드래그 시작 슬롯
        #endregion

        private void Start()
        {
            itemSlot = new ItemSlot(); // 아이템 슬롯 초기화
            //참조 변수
            iconTransform = itemIcon.GetComponent<RectTransform>();  // 아이콘의 RectTransform 참조
            itemIconImage = itemIcon.GetComponent<Image>();          // 아이콘 이미지 컴포넌트 참조
        }

        private void Update()
        {
            // 아이콘이 활성화되어 있으면
            if (itemIcon.activeInHierarchy)
            {
                // 아이콘의 위치를 마우스 위치로 업데이트
                iconTransform.position = Input.mousePosition;
            }
        }

        // 슬롯 클릭 시 호출되는 메서드
        public void ItemMove(ItemSlot clickedSlot)
        {
            // 현재 슬롯에 아이템이 없으면
            if (itemSlot.item == null)
            {
                itemSlot.Copy(clickedSlot);        // 클릭한 슬롯의 아이템 정보를 복사

                //originSlot = clickedSlot;       // 시작 슬롯 저장

                clickedSlot.Clear();               // 클릭한 슬롯을 비움
            }
            else // 현재 슬롯에 아이템이 있을 경우
            {
                isDraging = false;

                if (this.itemSlot.item == clickedSlot.item)  // 같은 아이템이면
                {
                    if (this.itemSlot.item.stackable)        // 스택 가능한 아이템이면
                    {
                        int max = this.itemSlot.item.maxStack;

                        int sum = this.itemSlot.amount + clickedSlot.amount;

                        if (sum <= max)
                        {
                            // 합쳐도 max 이하인 경우 그대로 합치기
                            clickedSlot.amount = sum;
                            this.itemSlot.Clear();
                        }
                        else
                        {
                            // 초과하는 경우 → 클릭된 슬롯은 max까지, 나머지는 슬롯에 남김
                            int remain = sum - max;
                            this.itemSlot.amount = remain;
                            clickedSlot.amount = max;
                        }
                    }
                }
                else  // 아이템이 다른 경우 → 두 슬롯의 아이템을 서로 교환
                {
                    // 💡 추가 조건: 제자리에 놓았을 경우 → 그냥 마우스 비우고 끝
                    if (originSlot == clickedSlot)
                    {
                        originSlot.Set(itemSlot.item, itemSlot.amount); // 원래 슬롯에 아이템 복사
                        itemSlot.Clear();
                    }
                    else
                    {
                        SwapItemSlots(clickedSlot); // 서로 다른 슬롯이면 자리 교환
                    }
                }
            }

            UpdateIcon();  // 아이콘 업데이트
        }

        // 두 슬롯의 아이템과 수량을 교환하는 함수
        private void SwapItemSlots(ItemSlot targetSlot)
        {
            // targetSlot은 드래그해서 놓은 슬롯 (예: 돌)
            // originSlot은 나무가 있던 원래 슬롯
            // itemSlot은 마우스로 들고 있는 아이템 (나무)

            ItemSO targetItem = targetSlot.item;
            int targetAmount = targetSlot.amount;

            // 1. targetSlot ← 나무
            targetSlot.Set(itemSlot.item, itemSlot.amount);

            // 2. originSlot ← 돌
            originSlot.Set(targetItem, targetAmount);

            // 3. 마우스 슬롯 비우기
            itemSlot.Clear();
        }

        // 아이콘 업데이트 메서드
        private void UpdateIcon()
        {
            itemIcon.SetActive(itemSlot.item != null);                            // 현재 슬롯에 아이템이 없으면 아이콘 비활성화, 있으면 활성화
            if (itemSlot.item != null) itemIconImage.sprite = itemSlot.item.icon; // 아이콘 이미지 설정
        }

        public void OnDragStart(ItemSlot draggedSlot)
        {
            isDraging = true;     // 드래그 중 플래그 설정

            originSlot = draggedSlot; // 드래그 시작 슬롯 저장

            ItemMove(draggedSlot); // 드래그하는 슬롯 클릭 처리
        }

        // 인벤토리 UI에 드롭 시 호출되는 메서드
        public void DropInInventoryUI(ItemSlot targetSlot)
        {
            isDraging = false;   // 드래그 중 플래그 해제
            ItemMove(targetSlot); // 타겟 슬롯 클릭 처리
        }

        // 드래그 종료 시 호출되는 메서드
        public void OnEndDrag()
        {
            isDraging = false; // 드래그 중 플래그 해제
            DropItem();        // 아이템 드롭 메서드 호출
        }

        // 아이템 드롭 메서드
        private void DropItem()
        {
            // 마우스 위치를 월드 좌표로 변환 (z는 2D 게임이므로 0으로 고정)
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0f;

            // 타워 아이템일 경우
            if (itemSlot.item.name.Contains("Tower"))
            {
                // 설치할 타워의 제작 레시피 가져오기
                BuildRecipeSO recipe = GameManager.instance.recipeDatabase.GetRecipe(itemSlot.item);

                // 인벤토리에 필요한 재료가 모두 있는 경우
                if (GameManager.instance.inventory.HasMaterials(recipe))
                {
                    // 1. 타워를 해당 위치에 설치 시도 (겹침 검사 포함됨)
                    bool isSuccess = TowerSpawnManager.instance.SpawnTower(worldPosition, itemSlot.item.prefab);

                    if (isSuccess)
                    {
                        // 2. 설치 성공 시 → 재료 소비
                        GameManager.instance.inventory.ConsumeMaterials(recipe);

                        // 3. 드래그 중인 슬롯에서 아이템 수량 감소
                        itemSlot.amount--;

                        // 수량이 0 이하이면 슬롯 비우기
                        if (itemSlot.amount <= 0)
                            itemSlot.Clear();
                    }
                    else
                    {
                        // 4. 설치 실패 시 → 다시 인벤토리에 아이템 되돌려 넣기
                        GameManager.instance.inventory.Add(itemSlot.item, itemSlot.amount);
                        itemSlot.Clear();
                    }
                }
                else
                {
                    // 재료 부족 알림
                    GameManager.instance.achiveManager.NotEnoughMaterial("나무와 돌이 5개씩 필요합니다!");

                    // ✅ 설치 실패 시 → 다시 인벤토리에 아이템 되돌려 넣기
                    GameManager.instance.inventory.Add(itemSlot.item, itemSlot.amount);

                    // 드래그 슬롯 비우기
                    itemSlot.Clear();
                }
            }
            else
            {
                // 일반 아이템일 경우 → 플레이어 위치에 아이템을 드롭
                ItemSpawnManager.instance.SpawnItem(GameManager.instance.player.transform.position, itemSlot.item, itemSlot.amount);

                // 슬롯 비우기
                itemSlot.Clear();
            }

            // 아이템 아이콘 갱신 (UI 업데이트)
            UpdateIcon();
        }


    }
}