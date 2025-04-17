using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyVampireSurvivors
{
    public class InventoryButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        #region Variables
        [SerializeField]
        Image icon;
        [SerializeField]
        TextMeshProUGUI text;

        [SerializeField] int myIndex;

        public ItemSO GetItem                     // 현재 선택한 아이템을 가져오는 프로퍼티
        {
            get
            {
                // GameManager의 인벤토리 데이터에서 현재 선택된 툴의 아이템을 가져옴
                ItemSO itemSO = GameManager.instance.inventory.itemSlots[myIndex].item;
                // 아이템이 없으면 null을 반환
                if (itemSO == null) return null;

                return itemSO;
            }
        }
        #endregion

        public void SetIndex(int index)
        {
            myIndex = index;
        }

        public void SetItem(ItemSlot slot)
        {
            icon.gameObject.SetActive(true);
            icon.sprite = slot.item.icon;
            text.gameObject.SetActive(true);
            text.text = slot.amount.ToString();
        }

        public void Clean()
        {
            icon.sprite = null;
            icon.gameObject.SetActive(false);
            text.text = "";
            text.gameObject.SetActive(false);
        }

        #region 드래그 앤 드롭 기능 구현
        // 드래그 시작 이벤트 처리 메서드
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (GetItem == null && GameManager.instance.dragAndDropController.itemSlot.item == null) return; // 아이템이 없으면 리턴
            #region 250328 인벤토리 별로 드래그 관리
            // 부모 오브젝트에서 ItemPanel 컴포넌트를 가져옵니다.
            InventoryPanel itemPanel = transform.parent.GetComponent<InventoryPanel>();

            itemPanel.OnDragStart(myIndex);
            #endregion
        }

        // 드래그 중 이벤트 처리 메서드 (현재는 빈 메서드)
        public void OnDrag(PointerEventData eventData) { }

        // 드래그 종료 이벤트 처리 메서드
        public void OnEndDrag(PointerEventData eventData)
        {
            if (GetItem == null && GameManager.instance.dragAndDropController.itemSlot.item == null)
                return; // 아이템이 없으면 리턴
            // 드래그 중이 아니면 리턴
            if (!GameManager.instance.dragAndDropController.isDraging) return;
            if (eventData.pointerEnter == null)
            {
                // 드래그 앤 드롭 컨트롤러의 드래그 종료 메서드 호출
                GameManager.instance.dragAndDropController.OnEndDrag();
                return;
            }
            // 부모 오브젝트에서 ItemPanel 컴포넌트를 가져옵니다.
            InventoryPanel itemPanel = transform.parent.GetComponent<InventoryPanel>();

            itemPanel.OnDragEnd(myIndex);
        }

        // 드롭 이벤트 처리 메서드
        public void OnDrop(PointerEventData eventData)
        {
            if (GetItem == null && GameManager.instance.dragAndDropController.itemSlot.item == null) return; // 아이템이 없으면 리턴
            #region 250328 인벤토리 별로 드래그 관리
            // 부모 오브젝트에서 ItemPanel 컴포넌트를 가져옵니다.
            InventoryPanel itemPanel = transform.parent.GetComponent<InventoryPanel>();

            itemPanel.OnDragEnd(myIndex);
            #endregion
        }
        #endregion
    }
}