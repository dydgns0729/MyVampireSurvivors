using System.Collections.Generic;
using UnityEngine;

namespace MyVampireSurvivors
{
    public class PoolManager : MonoBehaviour
    {
        #region Variables
        // 게임 오브젝트 프리팹들을 보관할 배열 변수
        public GameObject[] prefabs;

        // 각 프리팹에 대해 관리할 풀을 저장하는 배열 변수
        List<GameObject>[] pools;
        #endregion

        // 초기화 작업: 풀의 리스트를 준비
        private void Awake()
        {
            // 프리팹의 개수만큼 풀을 생성하여 pools 배열에 할당
            pools = new List<GameObject>[prefabs.Length];

            // 각 프리팹에 대해 빈 풀을 초기화
            for (int i = 0; i < prefabs.Length; i++)
            {
                pools[i] = new List<GameObject>();
            }
        }

        // 풀에서 게임 오브젝트를 가져오는 메서드
        // index: 가져올 풀의 인덱스
        public GameObject Get(int index)
        {
            GameObject select = null;

            // 선택한 풀에서 비활성화된 오브젝트를 찾기 위해 순회
            foreach (GameObject item in pools[index])
            {
                // 비활성화된 오브젝트를 찾으면
                if (!item.activeSelf)
                {
                    // 해당 오브젝트를 선택하고 활성화
                    select = item;
                    select.SetActive(true);
                    break;
                }
            }

            // 만약 비활성화된 오브젝트를 찾지 못하면 새로 생성
            if (!select)
            {
                // 새로운 오브젝트를 생성하여 풀에 추가
                select = Instantiate(prefabs[index], transform);
                pools[index].Add(select);
            }

            // 선택된 게임 오브젝트를 반환
            return select;
        }
    }
}
