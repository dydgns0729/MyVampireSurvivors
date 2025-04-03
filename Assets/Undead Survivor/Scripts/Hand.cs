using UnityEngine;

namespace MyVampireSurvivors
{
    public class Hand : MonoBehaviour
    {
        #region Variables
        // 왼손인지 오른손인지를 나타내는 변수
        public bool isLeft;

        // 손에 해당하는 스프라이트 렌더러 (손의 이미지가 표시되는 UI 요소)
        public SpriteRenderer spriter;

        // 플레이어의 스프라이트 렌더러 (플레이어의 방향에 따라 손의 방향을 결정)
        SpriteRenderer player;

        // 오른손 위치 (기본 위치와 반전된 위치)
        Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
        Vector3 rightPosReverse = new Vector3(0f, -0.15f, 0);

        // 왼손 회전 (기본 회전과 반전된 회전)
        Quaternion leftRot = Quaternion.Euler(0, 0, -35);
        Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);
        #endregion

        // 초기화 작업: 플레이어의 스프라이트 렌더러를 가져옴
        private void Awake()
        {
            // 플레이어의 스프라이트 렌더러를 부모 오브젝트에서 가져옴
            player = GetComponentsInParent<SpriteRenderer>()[1];
        }

        // 매 프레임 후 호출되는 함수 (손의 위치와 방향을 업데이트)
        private void LateUpdate()
        {
            // 플레이어가 반전된 방향인지 확인 (플레이어의 `flipX` 값에 따라 손의 방향을 반전)
            bool isReverse = player.flipX;

            // 왼손인 경우 (근접 무기일 경우)
            if (isLeft)
            {
                // 플레이어가 반전된 방향이면 왼손의 회전을 반전된 값으로 설정, 그렇지 않으면 기본 회전값 사용
                transform.localRotation = isReverse ? leftRotReverse : leftRot;
                // 손의 스프라이트 방향을 반전시킴 (플레이어가 반전되면 손도 반전)
                spriter.flipY = isReverse;
                // 손의 렌더링 순서를 설정 (반전되면 다른 레이어에 배치)
                spriter.sortingOrder = isReverse ? 4 : 6;
            }
            else // 오른손 (원거리 무기일 경우)
            {
                // 플레이어가 반전된 방향이면 오른손의 위치를 반전된 위치로 설정, 그렇지 않으면 기본 위치 사용
                transform.localPosition = isReverse ? rightPosReverse : rightPos;
                // 손의 스프라이트 방향을 반전시킴 (플레이어가 반전되면 손도 반전)
                spriter.flipX = isReverse;
                // 손의 렌더링 순서를 설정 (반전되면 다른 레이어에 배치)
                spriter.sortingOrder = isReverse ? 6 : 4;
            }
        }
    }
}
