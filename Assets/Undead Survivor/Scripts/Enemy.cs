using UnityEngine;

namespace MyVampireSurvivors
{
    public class Enemy : MonoBehaviour
    {
        #region Variables
        // 적의 이동 속도
        public float speed;

        // 애니메이션 컨트롤러 배열 (적의 스프라이트 타입에 따라 다르게 설정)
        public RuntimeAnimatorController[] animatorControllers;

        // 적의 체력과 최대 체력
        public float health;
        public float maxHealth;

        // 적이 추적할 타겟 (플레이어)
        public Rigidbody2D target;

        // 적이 살아있는지 여부를 나타내는 변수
        bool isLive;

        // 물리적 이동을 위한 Rigidbody2D 컴포넌트
        Rigidbody2D rb2d;

        // 애니메이션을 위한 Animator 컴포넌트
        Animator animator;

        // 스프라이트 렌더링을 위한 SpriteRenderer 컴포넌트
        SpriteRenderer spriter;
        #endregion

        // 초기화 작업: 컴포넌트들을 가져오기
        private void Awake()
        {
            // Rigidbody2D 컴포넌트를 가져옴
            rb2d = GetComponent<Rigidbody2D>();
            // SpriteRenderer 컴포넌트를 가져옴
            spriter = GetComponent<SpriteRenderer>();
            // Animator 컴포넌트를 가져옴
            animator = GetComponent<Animator>();
        }

        // 물리적 업데이트: 매 FixedUpdate() 호출 시 적의 이동을 처리
        private void FixedUpdate()
        {
            // 적이 살아있지 않으면 이동을 처리하지 않음
            if (!isLive)
                return;

            // 타겟(플레이어)와 적 간의 방향 벡터 계산
            Vector2 dirVec = target.position - rb2d.position;

            // 방향 벡터를 정규화하고, 속도와 시간의 델타 값을 곱하여 이동할 거리 계산
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

            // Rigidbody2D의 MovePosition 메서드를 사용해 물리적으로 적을 이동시킴
            rb2d.MovePosition(rb2d.position + nextVec);

            // 속도 리셋 (선형 속도 값 초기화)
            rb2d.linearVelocity = Vector2.zero;
        }

        // 후처리 업데이트: 애니메이션이나 방향 전환을 처리
        private void LateUpdate()
        {
            // 적이 살아있지 않으면 방향 전환을 처리하지 않음
            if (!isLive)
                return;

            // 타겟(플레이어)의 X 좌표가 적의 X 좌표보다 작으면 스프라이트를 뒤집음
            // 이는 적이 플레이어를 바라보는 방향을 반영하기 위한 처리
            spriter.flipX = target.position.x < rb2d.position.x;
        }

        // 적이 활성화될 때 호출되는 메서드
        private void OnEnable()
        {
            // 적이 활성화될 때 살아있는 상태로 초기화
            isLive = true;
            // 적의 타겟을 플레이어로 설정
            target = GameManager.instance.player.GetComponent<Rigidbody2D>();

            // 적의 체력을 최대 체력으로 설정
            health = maxHealth;
        }

        // 적 초기화 메서드: SpawnData에 따라 적의 상태 초기화
        public void Init(SpawnData spawnData)
        {
            // 애니메이션 컨트롤러를 스폰 데이터에 맞게 설정
            animator.runtimeAnimatorController = animatorControllers[spawnData.spriteType];
            // 이동 속도와 체력을 스폰 데이터에 맞게 설정
            speed = spawnData.speed;
            maxHealth = spawnData.health;
            health = spawnData.health;
        }
    }
}
