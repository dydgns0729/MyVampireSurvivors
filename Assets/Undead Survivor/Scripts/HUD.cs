using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyVampireSurvivors
{
    public class HUD : MonoBehaviour
    {
        // UI에 표시할 정보의 종류를 나타내는 열거형 (Exp, Level, Kill, Time, Health)
        public enum InfoType
        {
            Exp,    // 경험치
            Level,  // 레벨
            Kill,   // 처치 수
            Time,   // 남은 시간
            Health  // 체력
        }

        #region Variables
        // UI에 표시할 정보의 종류를 지정하는 변수 (Exp, Level, Kill, Time, Health 중 하나)
        public InfoType type;

        // 텍스트 UI 요소를 참조하는 변수 (TextMeshProUGUI는 텍스트를 화면에 출력하는 UI 컴포넌트)
        TextMeshProUGUI myText;

        // 슬라이더 UI 요소를 참조하는 변수 (체력, 경험치 등 비율을 표현하는 데 사용)
        Slider mySlider;
        #endregion

        // 초기화 작업: UI 컴포넌트들 (TextMeshProUGUI, Slider)을 가져오는 함수
        private void Awake()
        {
            // 현재 오브젝트에서 TextMeshProUGUI 컴포넌트를 찾음
            myText = GetComponent<TextMeshProUGUI>();

            // 현재 오브젝트에서 Slider 컴포넌트를 찾음
            mySlider = GetComponent<Slider>();
        }

        // 매 프레임 후 호출되는 함수 (UI를 업데이트하는 역할)
        private void LateUpdate()
        {
            if (!GameManager.instance.isLive) return;
            // type 값에 따라 해당 정보를 업데이트
            switch (type)
            {
                // 경험치(Exp) 표시
                case InfoType.Exp:
                    // 현재 경험치 (GameManager에서 가져옴)
                    float curExp = GameManager.instance.exp;

                    // 다음 레벨까지 필요한 경험치 (GameManager에서 가져옴)
                    float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];

                    // 경험치를 비율로 계산하여 슬라이더 값에 반영
                    mySlider.value = curExp / maxExp;
                    break;

                // 레벨(Level) 표시
                case InfoType.Level:
                    // 레벨 값을 텍스트로 표시 (예: Lv.1)
                    myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                    break;

                // 처치 수(Kill) 표시
                case InfoType.Kill:
                    // 처치 수를 텍스트로 표시
                    myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                    break;

                // 남은 시간(Time) 표시
                case InfoType.Time:
                    // 남은 시간 계산 (최대 게임 시간에서 현재 게임 시간을 빼서 남은 시간 구하기)
                    float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;

                    // 남은 시간의 분과 초를 계산
                    int min = Mathf.FloorToInt(remainTime / 60);
                    int sec = Mathf.FloorToInt(remainTime % 60);

                    // 남은 시간을 텍스트로 표시 (예: 02:30)
                    myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                    break;

                // 체력(Health) 표시
                case InfoType.Health:
                    // 현재 체력과 최대 체력 값 가져오기
                    float curHealth = GameManager.instance.health;
                    float maxHealth = GameManager.instance.maxHealth;

                    // 체력을 비율로 계산하여 슬라이더 값에 반영
                    mySlider.value = curHealth / maxHealth;
                    break;

                // 기본적으로 아무것도 하지 않음
                default:
                    break;
            }
        }
    }
}
