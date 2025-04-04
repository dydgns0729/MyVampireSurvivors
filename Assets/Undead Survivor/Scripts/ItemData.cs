using UnityEngine;

namespace MyVampireSurvivors
{
    [CreateAssetMenu(menuName = "Scriptable Object/Item Data", fileName = "Item")]
    public class ItemData : ScriptableObject
    {
        // 아이템의 종류를 구분하는 열거형 (근접, 원거리, 장갑, 신발, 회복 아이템 등)
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
        // 아이템의 종류
        public ItemType itemType;
        // 아이템의 고유 ID (각 아이템을 구분하는 고유 값)
        public int itemId;
        // 아이템의 이름
        public string itemName;
        // 아이템의 설명 (아이템의 효과나 사용법에 대한 간략한 설명)
        [TextArea]
        public string itemDesc;
        // 아이템 아이콘 (UI에 표시될 아이템의 아이콘 이미지)
        public Sprite itemIcon;

        [Header("# Level Data")]
        // 아이템의 기본 피해량 (기본 공격력 등)
        public float baseDamage;
        // 250403기준 근접 : 총알의 수, 원거리 : 관통력
        public int baseCount;
        // 아이템의 레벨에 따른 피해량 배열 (레벨마다 증가하는 피해량)
        public float[] damages;
        // 아이템의 레벨에 따른 총알 수 배열 (레벨마다 증가하는 총알 수)
        public int[] counts;

        [Header("# Weapon")]
        // 해당 아이템에 연결될 프리팹
        public GameObject projectile;
        // 아이템을 사용할 때 손에 표시될 스프라이트 (아이템이 장착될 때 손 모양을 나타냄)
        public Sprite hand;
        #endregion

    }
}