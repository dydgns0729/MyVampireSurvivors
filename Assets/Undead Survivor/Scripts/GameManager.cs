using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyVampireSurvivors
{
    public class GameManager : MonoBehaviour
    {
        #region Variables
        // �̱��� ������ ���� �ν��Ͻ� ����
        public static GameManager instance;

        [Header("# Game Control")]

        public bool isLive;
        // ���� �ð� ����, ������ ����� �ð�
        public float gameTime;
        // �ִ� ���� �ð��� �����ϴ� ���� (2������ ����)
        public float maxGameTime = 2 * 10f;

        [Header("# Player Info")]
        public int level;
        public int kill;
        public int exp;
        public int[] nextExp = { 10, 30, 60, 100, 150, 200, 280, 400, 500, 600 };
        public float health;
        public int maxHealth = 100;

        [Header("# Game Object")]
        // Player ��ü�� �����ϴ� ����
        public Player player;
        // Ǯ ���� Ŭ���� (PoolManager) ����
        public PoolManager poolManager;
        // UI ������ ������Ʈ
        public LevelUp uiLevelUp;
        public Result uiResult;
        public GameObject enemyCleaner;
        #endregion

        // ���� ���� �� ȣ��Ǵ� �Լ�
        private void Awake()
        {
            // GameManager�� �ν��Ͻ��� �̱��� �������� ����
            instance = this;
        }

        public void GameStart()
        {
            health = maxHealth;

            // �ӽ� ��ũ��Ʈ (ù��° ĳ���� ����)
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

        // �� �����Ӹ��� ȣ��Ǵ� �Լ�
        private void Update()
        {
            if (!isLive) return;

            // ���� �ð��� �带 ������ deltaTime ��ŭ ����
            gameTime += Time.deltaTime;

            // ���� �ð��� �ִ� ���� �ð��� ���� �ʵ��� ����
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
