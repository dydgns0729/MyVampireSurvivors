using UnityEngine;

namespace MyVampireSurvivors
{
    public class Spawner : MonoBehaviour
    {
        #region Variables
        // ���� ������ ��ġ���� �����ϴ� �迭 (�ڽ� ������Ʈ�� �ִ� ��ġ��)
        public Transform[] spawnPoints;

        // Ÿ�̸� ����, ���� �ð� �������� ���� �����ϱ� ���� ����
        float timer;
        #endregion

        // �ʱ�ȭ: �ڽ� ������Ʈ�� �ִ� spawnPoints�� ������
        private void Awake()
        {
            // ���� ������Ʈ�� �ڽĵ� �� Transform ������Ʈ�� ��� ������
            spawnPoints = GetComponentsInChildren<Transform>();
        }

        private void Update()
        {
            // Ÿ�̸Ӹ� deltaTime ��ŭ ���������� (�����Ӵ� �ð� ����)
            timer += Time.deltaTime;

            // Ÿ�̸Ӱ� ���� �ð�(0.2��)�� ������ ���� ����
            if (timer > 0.2f)
            {
                // Spawn() �޼��带 ȣ���Ͽ� �� ����
                Spawn();
                // Ÿ�̸� ����
                timer = 0f;
            }
        }

        // ���� �����ϴ� �޼���
        void Spawn()
        {
            // Ǯ���� �� ������Ʈ�� ������ enemy ������ �Ҵ�
            GameObject enemy = GameManager.instance.poolManager.Get(Random.Range(0, GameManager.instance.poolManager.prefabs.Length));

            // spawnPoints �迭���� ������ ��ġ�� �����Ͽ� ���� ��ġ�� ����
            // spawnPoints[0]�� Spawner �ڽ��̱� ������ 1���� �����Ͽ� ���� ��ġ�� ����
            enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
        }
    }
}
