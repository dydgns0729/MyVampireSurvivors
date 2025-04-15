using System;
using System.Collections;
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
        //public float health;
        //public float maxHealth;

        // 적이 추적할 타겟 (플레이어)
        public Rigidbody2D target;

        // 물리적 이동을 위한 Rigidbody2D 컴포넌트
        Rigidbody2D rb2d;

        Collider2D collider2d;

        // 애니메이션을 위한 Animator 컴포넌트
        Animator animator;

        // 스프라이트 렌더링을 위한 SpriteRenderer 컴포넌트
        SpriteRenderer spriter;

        WaitForFixedUpdate wait;

        Health health;

        bool isKnockBack;

        [Header("# Test IDT")]
        public ItemDropTableSO[] itemDropTables;
        ItemDropTableSO currentIDT;
        #endregion

        // 초기화 작업: 컴포넌트들을 가져오기
        private void Awake()
        {
            // Rigidbody2D 컴포넌트를 가져옴 (적의 물리적 이동을 담당)
            rb2d = GetComponent<Rigidbody2D>();
            collider2d = GetComponent<Collider2D>();
            // SpriteRenderer 컴포넌트를 가져옴 (적의 스프라이트 렌더링을 담당)
            spriter = GetComponent<SpriteRenderer>();
            // Animator 컴포넌트를 가져옴 (애니메이션 제어를 담당)
            animator = GetComponent<Animator>();

            wait = new WaitForFixedUpdate();

            health = GetComponent<Health>();
        }

        // 물리적 업데이트: 매 FixedUpdate() 호출 시 적의 이동을 처리
        private void FixedUpdate()
        {
            if (!GameManager.instance.isLive) return;
            // Hit Animation이 재생 중이면 이동하지 않음
            if (isKnockBack)
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
            // 플레이어나 몬스터가 죽었다면 리턴
            if (!health.isLive || !GameManager.instance.isLive)
                return;

            // 타겟(플레이어)의 X 좌표가 적의 X 좌표보다 작으면 스프라이트를 뒤집음
            // 이는 적이 플레이어를 바라보는 방향을 반영하기 위한 처리
            spriter.flipX = target.position.x < rb2d.position.x;
        }

        // 적이 활성화될 때 호출되는 메서드
        private void OnEnable()
        {
            // 적의 타겟을 플레이어로 설정
            target = GameManager.instance.player.GetComponent<Rigidbody2D>();

            // 적이 활성화될 때 살아있는 상태로 초기화
            collider2d.enabled = true;
            rb2d.simulated = true;
            spriter.sortingOrder = 2;
            animator.SetBool("Dead", false);

            health.OnHealthChanged += OnDamaged;
            health.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            // 적이 비활성화될 때 이벤트 구독 해제
            health.OnHealthChanged -= OnDamaged;
            health.OnDeath -= OnDeath;
        }

        // 충돌 처리: 총알과 충돌 시 처리
        public void OnTriggerEnter2D(Collider2D collision)
        {
            // 충돌한 오브젝트가 "Bullet" 태그를 가지지 않으면 처리하지 않음
            if (!collision.CompareTag("Bullet") || !health.isLive)
                return;

            // 총알의 피해를 받아 체력 감소
            float damage = collision.GetComponent<Bullet>().damage;

            health.TakeDamage(damage);
        }

        private void OnDamaged()
        {
            StartCoroutine(KnockBack());

            animator.SetTrigger("Hit");
            AudioManager.instance.PlaySFX(AudioManager.SFX.Hit);
        }

        private void OnDeath()
        {
            // 죽음 상태로 전환
            collider2d.enabled = false;
            // 물리 계산 비활성화
            rb2d.simulated = false;
            spriter.sortingOrder = 1;
            animator.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            // 아이템 드랍
            currentIDT.ItemDrop(transform.position);

            if (GameManager.instance.isLive)
            {
                AudioManager.instance.PlaySFX(AudioManager.SFX.Dead);
            }
        }

        // 적 초기화 메서드: SpawnData에 따라 적의 상태 초기화
        public void Init(SpawnData spawnData)
        {
            // 애니메이션 컨트롤러를 스폰 데이터에 맞게 설정
            animator.runtimeAnimatorController = animatorControllers[spawnData.enemyIndex];

            // 현재 IDT를 스폰 데이터에 맞게 설정
            currentIDT = itemDropTables[spawnData.enemyIndex];

            // 이동 속도와 체력을 스폰 데이터에 맞게 설정
            speed = spawnData.speed;
            health.Init(spawnData.health);
        }

        IEnumerator KnockBack()
        {
            isKnockBack = true;
            Vector3 playerPos = GameManager.instance.player.transform.position;
            Vector3 dirVec = transform.position - playerPos;
            rb2d.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

            //다음 물리 프레임까지 딜레이
            yield return new WaitForSeconds(0.05f);
            isKnockBack = false;
        }
    }
}
