using System.Collections.Generic;
using UnityEngine;

namespace MyVampireSurvivors
{
    public class PoolManager : MonoBehaviour
    {
        #region Variables
        // ���� ������Ʈ �����յ��� ������ �迭 ����
        public GameObject[] prefabs;

        // �� �����տ� ���� ������ Ǯ�� �����ϴ� �迭 ����
        List<GameObject>[] pools;
        #endregion

        // �ʱ�ȭ �۾�: Ǯ�� ����Ʈ�� �غ�
        private void Awake()
        {
            // �������� ������ŭ Ǯ�� �����Ͽ� pools �迭�� �Ҵ�
            pools = new List<GameObject>[prefabs.Length];

            // �� �����տ� ���� �� Ǯ�� �ʱ�ȭ
            for (int i = 0; i < prefabs.Length; i++)
            {
                pools[i] = new List<GameObject>();
            }
        }

        // Ǯ���� ���� ������Ʈ�� �������� �޼���
        // index: ������ Ǯ�� �ε���
        public GameObject Get(int index)
        {
            GameObject select = null;

            // ������ Ǯ���� ��Ȱ��ȭ�� ������Ʈ�� ã�� ���� ��ȸ
            foreach (GameObject item in pools[index])
            {
                // ��Ȱ��ȭ�� ������Ʈ�� ã����
                if (!item.activeSelf)
                {
                    // �ش� ������Ʈ�� �����ϰ� Ȱ��ȭ
                    select = item;
                    select.SetActive(true);
                    break;
                }
            }

            // ���� ��Ȱ��ȭ�� ������Ʈ�� ã�� ���ϸ� ���� ����
            if (!select)
            {
                // ���ο� ������Ʈ�� �����Ͽ� Ǯ�� �߰�
                select = Instantiate(prefabs[index], transform);
                pools[index].Add(select);
            }

            // ���õ� ���� ������Ʈ�� ��ȯ
            return select;
        }
    }
}
