using UnityEngine;

namespace MyVampireSurvivors
{
    public class Gear : MonoBehaviour
    {
        #region Variables
        // ����� ���� (�尩, �Ź� ��) ������ ������ ����
        public ItemData.ItemType type;

        // ����� ȿ���� �����ϴ� ���� (��: �尩 ȿ�� ����, �Ź� �ӵ� ���� ���� ��)
        public float rate;
        #endregion

        // ��� �ʱ�ȭ�ϴ� �Լ� (������ ������ ������� ����)
        public void Init(ItemData data)
        {
            // ��� �̸��� "Gear" + ������ ID�� ����
            name = "Gear " + data.itemId;

            // ����� �θ� �÷��̾�� �����Ͽ� �÷��̾�� �Բ� �̵��ϵ��� ��
            transform.parent = GameManager.instance.player.transform;

            // ����� ��ġ�� �÷��̾��� ��ġ�� �����ϰ� ����
            transform.localPosition = Vector3.zero;

            // ������ �����Ϳ��� ��� ������ ȿ�� ������ �����ͼ� ����
            type = data.itemType;
            rate = data.damages[0];

            // ����� ȿ���� ����
            ApplyGear();
        }

        // ��� ������ �Լ� (ȿ�� ������ ������Ʈ)
        public void LevelUp(float rate)
        {
            // ������ ��, ���ο� ȿ�� ������ �����ϰ� ��� ȿ���� ����
            this.rate = rate;
            ApplyGear();
        }

        // ����� ȿ���� �����ϴ� �Լ�
        // ��� ������ ���� �ٸ��� ó��
        void ApplyGear()
        {
            switch (type)
            {
                // �尩�� ���, ����� ȿ���� ������Ű�� �Լ� ȣ��
                case ItemData.ItemType.Glove:
                    RateUp();
                    break;
                // �Ź��� ���, �÷��̾��� �ӵ��� ������Ű�� �Լ� ȣ��
                case ItemData.ItemType.Shoe:
                    SpeedUp();
                    break;
            }
        }

        // �尩 ����� ȿ���� �����ϴ� �Լ� (������ ���� �ӵ� ����)
        void RateUp()
        {
            // �÷��̾��� �ڽ� ������Ʈ�� �ִ� ��� ������� ������
            Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

            // �� ������ �ӵ��� �尩�� ȿ���� �°� ����
            foreach (Weapon weapon in weapons)
            {
                switch (weapon.id)
                {
                    // ������ id�� 0�� ���, �ӵ��� 150�� ����Ͽ� ����
                    case 0:
                        float speed = 150 * Character.WeaponSpeed;
                        weapon.speed = 150 + (150 * rate);
                        break;
                    // �� ���� ������� �⺻ �ӵ��� ������ rate�� ���� ����
                    default:
                        speed = 0.5f * Character.WeaponRate;
                        weapon.speed = 0.5f * (1f - rate);
                        break;
                }
            }
        }

        // �Ź� ����� ȿ���� �����ϴ� �Լ� (�÷��̾��� �̵� �ӵ� ����)
        void SpeedUp()
        {
            // �⺻ �ӵ� ����
            float speed = 5 * Character.Speed;

            // �÷��̾��� �̵� �ӵ��� �Ź��� ȿ�� ������ ���� ����
            GameManager.instance.player.speed = speed + speed * rate;
        }
    }
}
