using UnityEngine;

namespace MyVampireSurvivors
{
    [CreateAssetMenu(menuName = "Scriptable Object/Item Data", fileName = "Item")]
    public class ItemData : ScriptableObject
    {
        // �������� ������ �����ϴ� ������ (����, ���Ÿ�, �尩, �Ź�, ȸ�� ������ ��)
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
        // �������� ����
        public ItemType itemType;
        // �������� ���� ID (�� �������� �����ϴ� ���� ��)
        public int itemId;
        // �������� �̸�
        public string itemName;
        // �������� ���� (�������� ȿ���� ������ ���� ������ ����)
        [TextArea]
        public string itemDesc;
        // ������ ������ (UI�� ǥ�õ� �������� ������ �̹���)
        public Sprite itemIcon;

        [Header("# Level Data")]
        // �������� �⺻ ���ط� (�⺻ ���ݷ� ��)
        public float baseDamage;
        // 250403���� ���� : �Ѿ��� ��, ���Ÿ� : �����
        public int baseCount;
        // �������� ������ ���� ���ط� �迭 (�������� �����ϴ� ���ط�)
        public float[] damages;
        // �������� ������ ���� �Ѿ� �� �迭 (�������� �����ϴ� �Ѿ� ��)
        public int[] counts;

        [Header("# Weapon")]
        // �ش� �����ۿ� ����� ������
        public GameObject projectile;
        // �������� ����� �� �տ� ǥ�õ� ��������Ʈ (�������� ������ �� �� ����� ��Ÿ��)
        public Sprite hand;
        #endregion

    }
}