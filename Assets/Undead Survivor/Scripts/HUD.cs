using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyVampireSurvivors
{
    public class HUD : MonoBehaviour
    {
        // UI�� ǥ���� ������ ������ ��Ÿ���� ������ (Exp, Level, Kill, Time, Health)
        public enum InfoType
        {
            Exp,    // ����ġ
            Level,  // ����
            Kill,   // óġ ��
            Time,   // ���� �ð�
            Health  // ü��
        }

        #region Variables
        // UI�� ǥ���� ������ ������ �����ϴ� ���� (Exp, Level, Kill, Time, Health �� �ϳ�)
        public InfoType type;

        // �ؽ�Ʈ UI ��Ҹ� �����ϴ� ���� (TextMeshProUGUI�� �ؽ�Ʈ�� ȭ�鿡 ����ϴ� UI ������Ʈ)
        TextMeshProUGUI myText;

        // �����̴� UI ��Ҹ� �����ϴ� ���� (ü��, ����ġ �� ������ ǥ���ϴ� �� ���)
        Slider mySlider;
        #endregion

        // �ʱ�ȭ �۾�: UI ������Ʈ�� (TextMeshProUGUI, Slider)�� �������� �Լ�
        private void Awake()
        {
            // ���� ������Ʈ���� TextMeshProUGUI ������Ʈ�� ã��
            myText = GetComponent<TextMeshProUGUI>();

            // ���� ������Ʈ���� Slider ������Ʈ�� ã��
            mySlider = GetComponent<Slider>();
        }

        // �� ������ �� ȣ��Ǵ� �Լ� (UI�� ������Ʈ�ϴ� ����)
        private void LateUpdate()
        {
            if (!GameManager.instance.isLive) return;
            // type ���� ���� �ش� ������ ������Ʈ
            switch (type)
            {
                // ����ġ(Exp) ǥ��
                case InfoType.Exp:
                    // ���� ����ġ (GameManager���� ������)
                    float curExp = GameManager.instance.exp;

                    // ���� �������� �ʿ��� ����ġ (GameManager���� ������)
                    float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];

                    // ����ġ�� ������ ����Ͽ� �����̴� ���� �ݿ�
                    mySlider.value = curExp / maxExp;
                    break;

                // ����(Level) ǥ��
                case InfoType.Level:
                    // ���� ���� �ؽ�Ʈ�� ǥ�� (��: Lv.1)
                    myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                    break;

                // óġ ��(Kill) ǥ��
                case InfoType.Kill:
                    // óġ ���� �ؽ�Ʈ�� ǥ��
                    myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                    break;

                // ���� �ð�(Time) ǥ��
                case InfoType.Time:
                    // ���� �ð� ��� (�ִ� ���� �ð����� ���� ���� �ð��� ���� ���� �ð� ���ϱ�)
                    float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;

                    // ���� �ð��� �а� �ʸ� ���
                    int min = Mathf.FloorToInt(remainTime / 60);
                    int sec = Mathf.FloorToInt(remainTime % 60);

                    // ���� �ð��� �ؽ�Ʈ�� ǥ�� (��: 02:30)
                    myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                    break;

                // ü��(Health) ǥ��
                case InfoType.Health:
                    // ���� ü�°� �ִ� ü�� �� ��������
                    float curHealth = GameManager.instance.health;
                    float maxHealth = GameManager.instance.maxHealth;

                    // ü���� ������ ����Ͽ� �����̴� ���� �ݿ�
                    mySlider.value = curHealth / maxHealth;
                    break;

                // �⺻������ �ƹ��͵� ���� ����
                default:
                    break;
            }
        }
    }
}
