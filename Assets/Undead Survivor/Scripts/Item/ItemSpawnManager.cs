using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyVampireSurvivors
{
    public class ItemSpawnManager : MonoBehaviour
    {
        #region Variables
        public static ItemSpawnManager instance;

        //아이템을 생성할 프리팹
        [SerializeField] GameObject pickUpItemPrefab;

        int prefabId; // 프리팹 ID (풀에서 가져올 프리팹을 구분하는 ID)
        #endregion

        private void Awake()
        {
            instance = this;
        }

        // 위치, 아이템, 갯수를 받아서 해당 위치에 아이템을 생성하고 튕겨나가는 연출을 적용하는 함수
        public void SpawnItem(Vector3 position, ItemSO item, int count = 1)
        {
            // 풀에서 해당 아이템에 맞는 발사체 프리팹을 찾아 그 ID를 설정
            for (int i = 0; i < GameManager.instance.poolManager.prefabs.Length; i++)
            {
                if (pickUpItemPrefab == GameManager.instance.poolManager.prefabs[i])
                {
                    prefabId = i; // 해당 프리팹의 ID 설정
                    break; // 찾으면 반복문 종료
                }
            }

            // [1] 지정된 위치에 PickUpItem 프리팹을 생성
            GameObject itemGO = GameManager.instance.poolManager.Get(prefabId).gameObject;
            itemGO.transform.position = position; // 생성된 아이템의 위치 설정
            PickUpItem pickUp = itemGO.GetComponent<PickUpItem>();
            // [2] 생성된 PickUpItem에 아이템 정보와 수량을 설정
            pickUp.Set(item, count);
            pickUp.isBouncing = true; // 튕겨나가는 애니메이션을 시작하기 위해 isBouncing을 true로 설정

            // [3] 랜덤한 방향(좌우) + 위쪽 방향으로 튀어나가도록 오프셋 벡터 설정
            Vector3 randomOffset = new Vector3(
                Random.Range(-2f, 2f),     // 좌우로 랜덤하게
                Random.Range(-1f, 1.5f),  // 위쪽으로 좀 더 튀게
                0f                         // Z축은 0 (2D 기준)
            );

            // [4] 튕겨나갈 목표 위치 계산 (현재 위치 + 랜덤 오프셋)
            Vector3 targetPos = position + randomOffset;

            // [5] 부드럽게 튕겨나가는 코루틴 실행
            StartCoroutine(Bounce(itemGO.transform, targetPos, pickUp));
        }

        // 아이템을 주어진 위치로 부드럽게 튕기듯 이동시키는 코루틴
        IEnumerator Bounce(Transform target, Vector3 end, PickUpItem pickUp)
        {
            float duration = 0.3f; // 이동에 걸리는 시간
            float elapsed = 0f;    // 경과 시간 초기화

            Vector3 start = target.position; // 시작 위치 저장

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime; // 매 프레임마다 경과 시간 증가

                float t = elapsed / duration; // 현재 진행률 (0 ~ 1 사이)

                // Lerp와 사인 곡선을 조합하여 부드럽게 가속→감속하는 Ease-Out 효과를 줌
                target.position = Vector3.Lerp(start, end, Mathf.Sin(t * Mathf.PI * 0.5f));

                yield return null; // 다음 프레임까지 대기
            }

            // 루프 종료 후 정확히 목표 위치에 위치 고정
            target.position = end;
            pickUp.isBouncing = false; // 튕겨나가는 애니메이션 종료
        }

    }
}