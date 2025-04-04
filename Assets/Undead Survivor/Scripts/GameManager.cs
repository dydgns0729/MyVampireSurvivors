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

        public bool isLive;
        // 게임 시간 변수, 게임이 진행된 시간
        public float gameTime;
        // 최대 게임 시간을 설정하는 변수 (2분으로 설정)
        public float maxGameTime = 2 * 10f;

        [Header("# Player Info")]
        public int level;
        public int kill;
        public int exp;
        public int[] nextExp = { 10, 30, 60, 100, 150, 200, 280, 400, 500, 600 };
        public float health;
        public int maxHealth = 100;

        [Header("# Game Object")]
        // Player 객체를 관리하는 변수
        public Player player;
        // 풀 관리 클래스 (PoolManager) 변수
        public PoolManager poolManager;
        // UI 레벨업 오브젝트
        public LevelUp uiLevelUp;
        public Result uiResult;
        public GameObject enemyCleaner;
        #endregion

        // 게임 시작 시 호출되는 함수
        private void Awake()
        {
            // GameManager의 인스턴스를 싱글톤 패턴으로 설정
            instance = this;
        }

        public void GameStart()
        {
            health = maxHealth;

            // 임시 스크립트 (첫번째 캐릭터 선택)
            uiLevelUp.Select(0);

            Resume();
        }

        public void GameOver()
        {
            StartCoroutine(GameOverRoutine());
        }

        IEnumerator GameOverRoutine()
        {
            isLive = false;

            yield return new WaitForSeconds(0.5f);

            uiResult.gameObject.SetActive(true);
            uiResult.Lose();
            Stop();
        }

        public void GameVictory()
        {
            StartCoroutine(GameVictoryRoutine());
        }

        IEnumerator GameVictoryRoutine()
        {
            isLive = false;
            enemyCleaner.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            uiResult.gameObject.SetActive(true);
            uiResult.Win();
            Stop();
        }

        public void GameRetry()
        {
            SceneManager.LoadScene(0);
        }

        // 매 프레임마다 호출되는 함수
        private void Update()
        {
            if (!isLive) return;

            // 게임 시간이 흐를 때마다 deltaTime 만큼 증가
            gameTime += Time.deltaTime;

            // 게임 시간이 최대 게임 시간을 넘지 않도록 제한
            if (gameTime > maxGameTime)
            {
                gameTime = maxGameTime;
                GameVictory();
            }
        }

        public void GetExp()
        {
            exp++;
            if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
            {
                level++;
                exp = 0;
                uiLevelUp.Show();
            }
        }

        public void Stop()
        {
            isLive = false;
            Time.timeScale = 0;
        }

        public void Resume()
        {
            isLive = true;
            Time.timeScale = 1;
        }
    }
}
