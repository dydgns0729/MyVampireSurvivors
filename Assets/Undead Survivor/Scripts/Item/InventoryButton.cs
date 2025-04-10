using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyVampireSurvivors
{
    public class InventoryButton : MonoBehaviour
    {
        #region Variables
        [SerializeField]
        Image icon;
        [SerializeField]
        TextMeshProUGUI text;

        int myIndex;
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
    }
}