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
                clickedSlot.Clear();               // 클릭한 슬롯을 비움
            }
            else // 현재 슬롯에 아이템이 있을 경우
            {
                isDraging = false;
                if (this.itemSlot.item == clickedSlot.item)  // 같은 아이템이면
                {
                    if (this.itemSlot.item.stackable)  // 스택 가능한 아이템이면
                    {
                        this.itemSlot.amount += clickedSlot.amount;  // 수량 합치기
                        clickedSlot.Copy(itemSlot); // 클릭한 슬롯에 현재 슬롯의 아이템과 개수 설정
                        itemSlot.Clear(); // 마우스 아이템 슬롯 비우기
                    }
                }
                else  // 아이템이 다른 경우
                {
                    SetItem(clickedSlot);
                }

            }
            UpdateIcon();                          // 아이콘 업데이트 메서드 호출
        }

        private void SetItem(ItemSlot clickedSlot)
        {
            ItemSO tempItem = clickedSlot.item;  // 클릭한 슬롯의 아이템을 임시 변수에 저장
            int tempCount = clickedSlot.amount; // 클릭한 슬롯의 아이템 개수를 임시 변수에 저장
            clickedSlot.Copy(itemSlot);        // 현재 슬롯의 아이템 정보를 클릭한 슬롯으로 복사
            itemSlot.Set(tempItem, tempCount); // 임시 변수의 아이템 정보를 현재 슬롯에 설정
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
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);        // 마우스 위치를 3D 월드 좌표로 변환
            worldPosition.z = 0f;                                                               // z 값 0으로 설정 (2D 게임)

            if (itemSlot.item.name.Contains("Tower"))
            {
                Debug.Log("타워 아이템 드롭");
                TowerSpawnManager.instance.SpawnTower(worldPosition, itemSlot.item.prefab); // 타워 아이템 드롭
                itemSlot.amount--;                                                       // 슬롯의 아이템 개수 감소
                if (itemSlot.amount <= 0) itemSlot.Clear();                             // 슬롯의 아이템 개수가 0 이하이면 슬롯 비우기
            }
            else
            {
                ItemSpawnManager.instance.SpawnItem(worldPosition, itemSlot.item, itemSlot.amount);  // 아이템을 월드에 드롭
                itemSlot.Clear();                                                                   // 현재 슬롯 비우기
            }
            UpdateIcon();                                                           // 아이콘 업데이트 메서드 호출
        }
    }
}