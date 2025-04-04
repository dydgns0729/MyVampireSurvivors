using UnityEngine;

namespace MyVampireSurvivors
{
    public class Scanner : MonoBehaviour
    {
        #region Variables
        // 탐지 범위: 이 범위 내에서 타겟을 탐지합니다.
        public float scanRange;

        // 탐지할 타겟 레이어: 타겟이 포함된 레이어를 지정하여, 해당 레이어의 오브젝트만 탐지할 수 있게 합니다.
        public LayerMask targetLayer;

        // 탐지된 타겟들: 범위 내에서 탐지된 모든 타겟을 저장합니다.
        public RaycastHit2D[] targets;

        // 가장 가까운 타겟: 여러 타겟 중 가장 가까운 타겟을 저장합니다.
        public Transform nearestTarget;
        #endregion

        // 매 프레임마다 물리적 계산을 통해 탐지
        private void FixedUpdate()
        {
            if (!GameManager.instance.isLive) return;
            // Physics2D.CircleCastAll을 사용하여 범위 내의 모든 타겟을 탐지
            // 탐지된 타겟을 targets 배열에 저장
            targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);

            // 가장 가까운 타겟을 찾아 nearestTarget에 저장
            nearestTarget = GetNearest();
        }

        // 가장 가까운 타겟을 찾는 함수
        Transform GetNearest()
        {
            // 기본적으로 찾은 타겟은 없다고 가정하고 null로 설정
            Transform result = null;
            // 가장 가까운 타겟과의 거리 차이를 비교하기 위한 변수
            float diff = 100;

            // 탐지된 모든 타겟에 대해 반복
            foreach (RaycastHit2D target in targets)
            {
                // 현재 Scanner의 위치와 타겟의 위치를 가져옴
                Vector3 myPos = transform.position;
                Vector3 targetPos = target.transform.position;

                // 현재 타겟과의 거리 계산
                float curDiff = Vector3.Distance(myPos, targetPos);

                // 현재 타겟과의 거리가 이전에 저장된 거리보다 더 가까운 경우
                if (curDiff < diff)
                {
                    // 가장 가까운 타겟을 찾으면, 그 타겟을 result로 설정
                    diff = curDiff;
                    result = target.transform;
                }
            }

            // 가장 가까운 타겟을 반환
            return result;
        }

        // 에디터에서 기즈모로 범위를 그려서 시각적으로 확인할 수 있게 하는 함수
        private void OnDrawGizmos()
        {
            // 기즈모의 색상을 녹색으로 설정
            Gizmos.color = Color.green;

            // 원 형태로 탐지 범위를 그리기 (2D 환경에서 탐지 범위를 시각적으로 표시)
            Gizmos.DrawWireSphere(transform.position, scanRange);
        }
    }
}
