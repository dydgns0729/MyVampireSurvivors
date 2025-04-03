using UnityEngine;

namespace MyVampireSurvivors
{
    [CreateAssetMenu(menuName = "Scriptable Object/Item Data", fileName = "Item")]
    public class ItemData : ScriptableObject
    {
        public enum ItemType
        {
            Melee,
            Range,
            Glove,
            Shoe,
            Heal
        }

        #region Variables
        [Header("# Main Info")]
        public ItemType itemType;
        public int itemId;
        public string itemName;
        public string itemDesc;
        public Sprite itemIcon;

        [Header("# Level Data")]
        public float baseDamage;
        public int baseCount;
        public float[] damages;
        public int[] counts;

        [Header("# Weapon")]
        public GameObject projectile;
        #endregion

    }
}