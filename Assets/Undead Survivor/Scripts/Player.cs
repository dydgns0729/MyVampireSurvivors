using UnityEngine;
using UnityEngine.InputSystem;      // New Input System을 사용하기 위해 추가

namespace MyVampireSurvivors
{
    public class Player : MonoBehaviour
    {
        #region Variables
        // 플레이어의 입력 벡터 (이동 방향)
        public Vector2 inputVec;
        // 플레이어의 이동 속도
        public float speed;

        // 물리적 처리를 위한 Rigidbody2D 컴포넌트
        Rigidbody2D rb2d;

        // 스프라이트 렌더러, 플레이어의 스프라이트를 다루는 컴포넌트
        SpriteRenderer spriteRenderer;

        // 애니메이터 컴포넌트, 애니메이션을 제어하는 데 사용
        Animator animator;
        #endregion

        // 초기화 작업, 필요한 컴포넌트들 참조
        private void Awake()
        {
            // Rigidbody2D 컴포넌트를 가져옴
            rb2d = GetComponent<Rigidbody2D>();
            // SpriteRenderer 컴포넌트를 가져옴
            spriteRenderer = GetComponent<SpriteRenderer>();
            // Animator 컴포넌트를 가져옴
            animator = GetComponent<Animator>();
        }

        // 물리적 업데이트 함수 (매 프레임 고정된 시간 간격으로 호출)
        private void FixedUpdate()
        {
            // 입력된 방향 벡터에 속도를 곱하여 다음 이동할 벡터 계산
            Vector2 nextVec = inputVec * speed * Time.fixedDeltaTime;

            // Rigidbody2D의 MovePosition을 사용하여 플레이어 위치 이동
            rb2d.MovePosition(rb2d.position + nextVec);
        }

        // 입력 시스템에서 입력값을 받을 때 호출되는 함수
        void OnMove(InputValue value)
        {
            // 입력된 벡터 값을 inputVec에 저장
            inputVec = value.Get<Vector2>();
        }

        // 프레임 후 업데이트 함수 (애니메이션과 스프라이트 회전 등)
        private void LateUpdate()
        {
            // 애니메이터에 "Speed" 파라미터를 설정하여 애니메이션 전환
            animator.SetFloat("Speed", inputVec.magnitude);

            // X축으로 이동 중이라면 스프라이트의 flipX 값 변경 (왼쪽/오른쪽 방향 전환)
            if (inputVec.x != 0)
            {
                spriteRenderer.flipX = inputVec.x < 0;
            }
        }
    }
}
