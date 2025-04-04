using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyVampireSurvivors
{
    public class Item : MonoBehaviour
    {
        #region Variables
        // �������� ������ ���� (�������� ����, �̸�, ������ ��)
        public ItemData data;

        // �������� ����
        public int level;

        // ���� �������� ���� ���, �ش� ���⸦ �����ϴ� ����
        public Weapon weapon;

        // �尩�̳� �Ź� �������� ���� ���, �ش� ��� �����ϴ� ����
        public Gear gear;

        // ������ �������� ǥ���� �̹��� ������Ʈ
        Image icon;

        // ������ ������ ǥ���� �ؽ�Ʈ ������Ʈ
        TextMeshProUGUI textLevel;
        TextMeshProUGUI textName;
        TextMeshProUGUI textDesc;
        #endregion

        // �ʱ�ȭ �۾�: ������ �����ܰ� ���� �ؽ�Ʈ �ʱ�ȭ
        private void Awake()
        {
            // ������ �������� ȭ�鿡 ǥ���� Image ������Ʈ�� ������ (�ڽ� ������Ʈ���� �� ��° Image)
            icon = GetComponentsInChildren<Image>()[1];
            // �������� �������� ����
            icon.sprite = data.itemIcon;

            // �ڽ� ������Ʈ�� �ִ� �ؽ�Ʈ ������Ʈ�� ������
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            // ù ��° �ؽ�Ʈ ������Ʈ�� ���� �ؽ�Ʈ�� ����
            textLevel = texts[0];
            textName = texts[1];
            textDesc = texts[2];
            textName.text = data.itemName;
        }

        private void OnEnable()
        {
            // ���� ������ "Lv.1", "Lv.2" �� �������� �ؽ�Ʈ�� ǥ��
            textLevel.text = $"Lv.{(level + 1)}";
            // �������� ������ ���� ó�� ������ �ٸ��� ����
            switch (data.itemType)
            {
                // ���� ���⳪ ���Ÿ� ������ ���
                case ItemData.ItemType.Melee:
                case ItemData.ItemType.Range:
                    // 
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                    break;
                case ItemData.ItemType.Glove:
                case ItemData.ItemType.Shoe:
                    // ��� �������� ������ ����
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                    break;
                default:
                    textDesc.text = string.Format(data.itemDesc);
                    break;
            }

        }

        // ������ Ŭ�� �� ����Ǵ� �Լ� (������ ���/������ ó��)
        public void OnClick()
        {
            // �������� ������ ���� ó�� ������ �ٸ��� ����
            switch (data.itemType)
            {
                // ���� ���⳪ ���Ÿ� ������ ���
                case ItemData.ItemType.Melee:
                case ItemData.ItemType.Range:
                    // ������ 0�� ���, ���ο� ���⸦ �����Ͽ� �ʱ�ȭ
                    if (level == 0)
                    {
                        // ���ο� GameObject�� �����Ͽ� Weapon ������Ʈ�� �߰�
                        GameObject newWeapon = new GameObject();
                        weapon = newWeapon.AddComponent<Weapon>();
                        weapon.Init(data);
                    }
                    else
                    {
                        // ������ ��, ���ط��� �Ѿ� ���� ����Ͽ� ���⸦ ��ȭ
                        float nextDamage = data.baseDamage;
                        int nextCount = 0;

                        // ������ �´� ���ط��� �Ѿ� �� ����
                        nextDamage += data.baseDamage * data.damages[level];
                        nextCount += data.counts[level];

                        // �������� ����� ������Ʈ
                        weapon.LevelUp(nextDamage, nextCount);
                    }
                    // ������ ������Ŵ
                    level++;
                    break;

                // �尩�̳� �Ź��� ���
                case ItemData.ItemType.Glove:
                case ItemData.ItemType.Shoe:
                    // ������ 0�� ���, ���ο� ��� �����Ͽ� �ʱ�ȭ
                    if (level == 0)
                    {
                        // ���ο� GameObject�� �����Ͽ� Gear ������Ʈ�� �߰�
                        GameObject newGear = new GameObject();
                        gear = newGear.AddComponent<Gear>();
                        gear.Init(data);
                    }
                    else
                    {
                        // ������ ��, ����� �ɷ�ġ�� ������Ŵ
                        float nextRate = data.damages[level];
                        gear.LevelUp(nextRate);
                    }
                    // ������ ������Ŵ
                    level++;
                    break;

                // ȸ�� �������� ���
                case ItemData.ItemType.Heal:
                    // ���� �� �÷��̾��� ü���� �ִ� ü������ ȸ��
                    GameManager.instance.health = GameManager.instance.maxHealth;
                    break;
            }

            // �������� ������ �ִ� ������ �����ϸ� ��ư�� ��Ȱ��ȭ
            if (level == data.damages.Length)
            {
                GetComponent<Button>().interactable = false;
            }
        }
    }
}
