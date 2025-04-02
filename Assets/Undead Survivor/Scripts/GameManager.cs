using UnityEngine;

namespace MyVampireSurvivors
{
    public class GameManager : MonoBehaviour
    {
        #region Variables
        // �̱��� ������ ���� �ν��Ͻ� ����
        public static GameManager instance;

        // ���� �ð� ����, ������ ����� �ð�
        public float gameTime;
        // �ִ� ���� �ð��� �����ϴ� ���� (2������ ����)
        public float maxGameTime = 2 * 10f;

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
    }
}
