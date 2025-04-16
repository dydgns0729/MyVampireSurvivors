using System.Collections.Generic; // List 사용을 위한 네임스페이스
using UnityEngine;

namespace MyVampireSurvivors
{
    // 인벤토리 UI를 관리하는 클래스
    public class InventoryPanel : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        ItemContainer inventory; // 실제 아이템 데이터를 저장하는 컨테이너 (ScriptableObject)

        [SerializeField]
        List<InventoryButton> inventoryButtons; // UI에서 각각의 인벤토리 슬롯에 해당하는 버튼 리스트
        #endregion

        private void Awake()
        {
            // 인벤토리 버튼들의 인덱스를 설정 (inventory.itemSlots와 매핑)
            SetIndex();
        }

        private void OnEnable()
        {
            // 인벤토리 패널이 켜질 때 현재 인벤토리 상태를 UI에 반영
            Show();

            // 인벤토리 데이터가 변경되면 Show()를 자동으로 호출해서 UI를 갱신하게 설정
            inventory.inventoryChanged += Show;
        }

        private void OnDisable()
        {
            // 인벤토리 패널이 꺼질 때 이벤트 구독 해제 (메모리 누수 방지)
            inventory.inventoryChanged -= Show;
        }

        // 인벤토리 슬롯의 인덱스를 InventoryButton에 설정하는 메서드
        private void SetIndex()
        {
            // 자식 오브젝트 중 InventoryButton 컴포넌트를 모두 찾아서 리스트에 추가
            inventoryButtons.AddRange(this.transform.GetComponentsInChildren<InventoryButton>());

            // 실제 아이템 슬롯 개수만큼 버튼에 인덱스 부여
            for (int i = 0; i < inventory.itemSlots.Count; i++)
            {
                inventoryButtons[i].SetIndex(i);  // 각 버튼에게 자신의 인벤토리 슬롯 인덱스를 알려줌
            }
        }

        // 인벤토리 데이터를 UI에 반영하는 메서드
        private void Show()
        {
            // 모든 슬롯에 대해 반복하면서
            for (int i = 0; i < inventory.itemSlots.Count; i++)
            {
                // 해당 슬롯에 아이템이 존재할 경우
                if (inventory.itemSlots[i].item != null)
                {
                    // 해당 인덱스의 버튼에 아이템 정보를 표시
                    inventoryButtons[i].SetItem(inventory.itemSlots[i]);
                }
                else
                {
                    // 아이템이 없으면 해당 버튼을 비워서 초기 상태로 복원
                    inventoryButtons[i].Clean();
                }
            }
        }

        // 드래그 시작 시 호출되는 메서드
        public void OnDragStart(int id)
        {
            // 드래그 앤 드롭 컨트롤러에 시작 슬롯 정보 전달
            GameManager.instance.dragAndDropController.OnDragStart(inventory.itemSlots[id]);

            // 드래그 시작 후에도 UI 갱신
            Show();
        }

        // 드래그 끝에 드롭할 때 호출되는 메서드
        public void OnDragEnd(int id)
        {
            // 드래그 앤 드롭 컨트롤러에 드롭할 슬롯 정보 전달
            GameManager.instance.dragAndDropController.DropInInventoryUI(inventory.itemSlots[id]);

            // 드래그 후 UI 갱신
            Show();
        }
    }
}
