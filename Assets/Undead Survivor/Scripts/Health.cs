using System;
using UnityEngine;

namespace MyVampireSurvivors
{
    public class Health : MonoBehaviour
    {
        #region Variables
        public float health; // 현재 체력
        public float maxHealth; // 최대 체력

        public bool isLive; // 살아있는지 여부

        // 체력 변경 시 호출되는 이벤트
        public event Action OnHealthChanged; // 체력 변경 이벤트
        public event Action OnDeath; // 죽을 때 호출되는 이벤트

        // 애니메이션을 위한 Animator 컴포넌트
        Animator animator;
        #endregion

        private void Awake()
        {
            // Animator 컴포넌트를 가져옴 (애니메이션 제어를 담당)
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            isLive = true;
            if (health <= 0)
            {
                Init(maxHealth); // 오브젝트가 활성화될 때 체력 초기화
            }
        }

        // 체력을 초기화하는 메서드
        public void Init(float maxHealth)
        {
            this.maxHealth = maxHealth;
            health = maxHealth; // 현재 체력을 최대 체력으로 설정
        }

        public void TakeDamage(float damage)
        {
            // 총알의 피해를 받아 체력 감소
            health -= damage;

            // 체력이 0 이하가 되면 죽음 처리
            if (health <= 0)
            {
                Debug.Log("Death() = " + gameObject.name);
                // 죽음 상태로 전환
                isLive = false;
                OnDeath?.Invoke(); // 죽음 이벤트 호출
            }
            else
            {
                OnHealthChanged?.Invoke();
            }
        }

        // 체력의 비율을 반환 (0 ~ 1)
        public float GetHealthRatio() => health / maxHealth;

        // 적이 죽었을 때 호출되는 함수 (애니메이션 DeadEnemy에서 이벤트 호출)
        private void Dead()
        {
            Debug.Log("Dead() = " + gameObject.name);
            // 게임 오브젝트를 비활성화
            gameObject.SetActive(false);

        }
    }
}
