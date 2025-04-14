using System.Collections.Generic;
using UnityEngine;

namespace MyVampireSurvivors
{
    public class InventoryPanel : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        ItemContainer inventory;
        [SerializeField]
        List<InventoryButton> inventoryButtons;
        #endregion
        private void Awake()
        {
            // 각 버튼에 인덱스를 설정
            SetIndex();
        }

        private void OnEnable()
        {
            // 현재 인벤토리 데이터를 UI에 반영
            Show();
            inventory.inventoryChanged += Show;  // 인벤토리 데이터가 변경될 때마다 UI를 갱신
        }

        private void OnDisable()
        {
            inventory.inventoryChanged -= Show;  // 인벤토리 데이터 변경 이벤트 구독 해제
        }

        // 인벤토리 슬롯의 인덱스를 InventoryButton에 설정
        private void SetIndex()
        {
            // InventoryPanel의 자식 오브젝트 중 InventoryButton 컴포넌트를 모두 찾아 리스트에 추가
            inventoryButtons.AddRange(this.transform.GetComponentsInChildren<InventoryButton>());
            for (int i = 0; i < inventory.itemSlots.Count; i++)
            {
                inventoryButtons[i].SetIndex(i);  // 각 버튼에 인덱스 부여
            }
        }

        private void Show()
        {
            for (int i = 0; i < inventory.itemSlots.Count; i++)
            {
                if (inventory.itemSlots[i].item != null)
                {
                    inventoryButtons[i].SetItem(inventory.itemSlots[i]);
                }
                else
                {
                    inventoryButtons[i].Clean();
                }
            }
        }

        public void OnClick(int id)
        {
            // 인벤토리 슬롯 클릭 시, 드래그 앤 드롭 컨트롤러의 OnClick 메서드 호출(아이템 이동)
            GameManager.instance.dragAndDropController.ItemMove(inventory.itemSlots[id]);
            // 인벤토리 UI 갱신
            Show();
        }

        public void OnDragStart(int id)
        {
            GameManager.instance.dragAndDropController.OnDragStart(inventory.itemSlots[id]);
            Show();
        }

        public void OnDragEnd(int id)
        {
            GameManager.instance.dragAndDropController.DropInInventoryUI(inventory.itemSlots[id]);
            Show();
        }
    }
}