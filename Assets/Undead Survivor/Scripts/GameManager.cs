using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyVampireSurvivors
{
    public class GameManager : MonoBehaviour
    {
        #region Variables
        // 싱글톤 패턴을 위한 인스턴스 변수
        public static GameManager instance;

        [Header("# Game Control")]
        public bool isLive;                // 게임이 진행 중인지 여부
        public float gameTime;            // 현재 게임이 진행된 시간
        public float maxGameTime = 2 * 10f; // 최대 게임 시간 (초 단위, 현재 20초)

        [Header("# Player Info")]
        public int playerId;              // 플레이어 ID (캐릭터 선택에 사용)
        public int level;                 // 현재 레벨
        public int kill;                  // 처치한 적의 수
        public int exp;                   // 현재 경험치
        public int[] nextExp = { 10, 30, 60, 100, 150, 200, 280, 400, 500, 600 }; // 다음 레벨까지 필요한 경험치
        public float health;             // 현재 체력
        public int maxHealth = 100;      // 최대 체력

        [Header("# Game Object")]
        public Player player;            // 플레이어 오브젝트 참조
        public PoolManager poolManager;  // 오브젝트 풀 매니저
        public LevelUp uiLevelUp;        // 레벨업 UI
        public Result uiResult;          // 결과 UI
        public GameObject enemyCleaner;  // 모든 적 제거 오브젝트
        public Transform uiJoy;          // 조이스틱 UI (모바일용?)
        public GameObject inventoryUI;   // 인벤토리 UI

        [Header("# Inventory")]
        public ItemContainer inventory;                    // 인벤토리 데이터 (ScriptableObject)
        public ItemDragAndDropController dragAndDropController; // 드래그 앤 드롭 컨트롤러

        [Header("# Recipe")]
        public RecipeDatabase recipeDatabase; // 조합법 데이터베이스
        #endregion

        // 게임 시작 시 호출되는 함수
        private void Awake()
        {
            instance = this;                      // 싱글톤 인스턴스 설정
            Application.targetFrameRate = 60;     // 프레임 고정
        }

        // 매 프레임마다 호출되는 함수
        private void Update()
        {
            if (!isLive) return;

            gameTime += Time.deltaTime;

            // 최대 게임 시간 도달 시 자동 승리 처리
            if (gameTime > maxGameTime)
            {
                gameTime = maxGameTime;
                GameVictory();
            }

            // 테스트용: V 키를 누르면 경험치 거의 가득 채우기
            if (Input.GetKeyDown(KeyCode.V))
            {
                exp = (nextExp[Mathf.Min(level, nextExp.Length - 1)]) - 1;
            }
        }

        private void OnApplicationQuit()
        {
            // 게임 종료 시 인벤토리 초기화
            inventory.ClearAllItems(); // 인벤토리 초기화
        }

        // 게임 시작 로직
        public void GameStart(int id)
        {
            playerId = id;
            health = maxHealth;

            player.gameObject.SetActive(true);                // 플레이어 활성화
            uiLevelUp.Select(playerId % 2);                  // 캐릭터 선택 반영

            Resume(); // 게임 재개

            AudioManager.instance.PlayBGM(true);             // BGM 재생
            AudioManager.instance.PlaySFX(AudioManager.SFX.Select); // 효과음 재생
        }

        // 게임 오버 처리
        public void GameOver()
        {
            inventory.ClearAllItems(); // 인벤토리 초기화
            StartCoroutine(GameOverRoutine());
        }

        // 게임 오버 시 연출 루틴
        IEnumerator GameOverRoutine()
        {
            isLive = false;

            yield return new WaitForSeconds(0.5f); // 약간의 딜레이 후

            uiResult.gameObject.SetActive(true);   // 결과 UI 활성화
            uiResult.Lose();                       // 패배 처리
            Stop();                                // 게임 정지
            AudioManager.instance.PlayBGM(false);  // BGM 정지
            AudioManager.instance.PlaySFX(AudioManager.SFX.Lose); // 패배 효과음
        }

        // 게임 승리 처리
        public void GameVictory()
        {
            inventory.ClearAllItems(); // 인벤토리 초기화
            StartCoroutine(GameVictoryRoutine());
        }

        // 게임 승리 시 연출 루틴
        IEnumerator GameVictoryRoutine()
        {
            isLive = false;
            enemyCleaner.SetActive(true);         // 화면상의 적 제거 오브젝트 활성화

            yield return new WaitForSeconds(0.5f);

            uiResult.gameObject.SetActive(true);  // 결과 UI 활성화
            uiResult.Win();                       // 승리 처리
            Stop();                               // 게임 정지
            AudioManager.instance.PlayBGM(false); // BGM 정지
            AudioManager.instance.PlaySFX(AudioManager.SFX.Win); // 승리 효과음
        }

        // 게임 다시 시작
        public void GameRetry()
        {
            inventory.ClearAllItems(); // 인벤토리 초기화
            AudioManager.instance.PlaySFX(AudioManager.SFX.Select); // 효과음 재생
            SceneManager.LoadScene(0); // 첫 번째 씬 다시 로드
        }

        // 일시 정지 및 재개
        public void GamePause(bool pause)
        {
            AudioManager.instance.PlaySFX(AudioManager.SFX.Select);
            AudioManager.instance.EffectBGM(pause);

            if (pause)
            {
                Stop();
            }
            else
            {
                Resume();
            }
        }

        // 게임 종료 (앱 종료)
        public void GameQuit()
        {
            Application.Quit();
        }

        // 경험치 획득 처리
        public void GetExp()
        {
            exp++;
            if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
            {
                level++;
                exp = 0;
                uiLevelUp.Show(); // 레벨업 UI 표시
            }
        }

        // 게임 정지
        public void Stop()
        {
            isLive = false;
            Time.timeScale = 0;            // 시간 정지
            uiJoy.localScale = Vector3.zero; // 조이스틱 숨김
        }

        // 게임 재개
        public void Resume()
        {
            isLive = true;
            Time.timeScale = 1;             // 시간 정상 속도
            uiJoy.localScale = Vector3.one; // 조이스틱 다시 보이기
        }

        // 인벤토리 UI 토글 (열기/닫기)
        public void InventoryToggle()
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }
}
