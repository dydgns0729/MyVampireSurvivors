using UnityEngine;

namespace MyVampireSurvivors
{
    public class GameManager : MonoBehaviour
    {
        #region Variables
        // �̱��� ������ ���� �ν��Ͻ� ����
        public static GameManager instance;

        [Header("# Game Control")]
        // ���� �ð� ����, ������ ����� �ð�
        public float gameTime;
        // �ִ� ���� �ð��� �����ϴ� ���� (2������ ����)
        public float maxGameTime = 2 * 10f;

        [Header("# Player Info")]
        public int level;
        public int kill;
        public int exp;
        public int[] nextExp = { 10, 30, 60, 100, 150, 200, 280, 400, 500, 600 };
        public int health;
        public int maxHealth = 100;

        [Header("# Game Object")]
        // Player ��ü�� �����ϴ� ����
        public Player player;
        // Ǯ ���� Ŭ���� (PoolManager) ����
        public PoolManager poolManager;
        #endregion

        // ���� ���� �� ȣ��Ǵ� �Լ�
        private void Awake()
        {
            // GameManager�� �ν��Ͻ��� �̱��� �������� ����
            instance = this;
        }

        private void Start()
        {
            health = maxHealth;
        }

        // �� �����Ӹ��� ȣ��Ǵ� �Լ�
        private void Update()
        {
            // ���� �ð��� �带 ������ deltaTime ��ŭ ����
            gameTime += Time.deltaTime;

            // ���� �ð��� �ִ� ���� �ð��� ���� �ʵ��� ����
            if (gameTime > maxGameTime)
            {
                gameTime = maxGameTime;
            }
        }

        public void GetExp()
        {
            exp++;
            if (exp >= nextExp[level])
            {
                level++;
                exp = 0;

            }
        }
    }
}
