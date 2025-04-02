using System;
using UnityEngine;

namespace MyVampireSurvivors
{
    public class Spawner : MonoBehaviour
    {
        #region Variables
        // ���� ������ ��ġ���� �����ϴ� �迭 (�ڽ� ������Ʈ�� �ִ� ��ġ��)
        public Transform[] spawnPoints;

        // ���� ���� ������ (���� �ð�, ��������Ʈ Ÿ��, ü��, �ӵ� ��)
        public SpawnData[] spawnData;

        // Ÿ�̸� ����, ���� �ð� �������� ���� �����ϱ� ���� ����
        float timer;
        // ���� ���� ���� (���� �ð��� ���� ����)
        int level;
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
            // ������ ���� �ð��� 10���� ���� ���� ������ ��
            level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);

            // Ÿ�̸Ӱ� ���� �ð�(0.2��)�� ������ ���� ����
            if (timer > spawnData[level].spawnTime)
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
            GameObject enemy = GameManager.instance.poolManager.Get(0);

            // spawnPoints �迭���� ������ ��ġ�� �����Ͽ� ���� ��ġ�� ����
            // spawnPoints[0]�� Spawner �ڽ��̱� ������ 1���� �����Ͽ� ���� ��ġ�� ����
            enemy.transform.position = spawnPoints[UnityEngine.Random.Range(1, spawnPoints.Length)].position;
            // ���� ���� �ʱ�ȭ (SpawnData�� ���� �Ӽ� ����)
            enemy.GetComponent<Enemy>().Init(spawnData[level]);
        }
    }

    // ���� ���� �����Ϳ� ���� Ŭ����
    [Serializable]
    public class SpawnData
    {
        // ���� �����Ǵ� �ð� ����
        public float spawnTime;

        // ���� ��������Ʈ Ÿ�� (� ��������Ʈ�� �������)
        public int spriteType;
        // ���� ü��
        public int health;
        // ���� �̵� �ӵ�
        public float speed;
    }
}
