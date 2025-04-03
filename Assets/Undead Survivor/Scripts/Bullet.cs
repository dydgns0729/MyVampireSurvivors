using UnityEngine;

namespace MyVampireSurvivors
{
    public class Bullet : MonoBehaviour
    {
        #region Variables
        // 총알의 피해량
        public float damage;
        // 총알의 관통 횟수
        public int per;

        // Rigidbody2D 컴포넌트, 물리적 이동을 위해 사용
        Rigidbody2D rb2d;
        #endregion

        // 객체가 초기화될 때 호출되는 함수
        private void Awake()
        {
            // Rigidbody2D 컴포넌트를 가져옴
            rb2d = GetComponent<Rigidbody2D>();
        }

        // 총알을 초기화하는 함수 (피해량, 관통 횟수, 이동 방향)
        public void Init(float damage, int per, Vector3 dir)
        {
            // 피해량과 관통 횟수 초기화
            this.damage = damage;
            this.per = per;

            // 관통 횟수가 -1이 아닐 경우, 총알에 속도 방향을 설정
            if (per > -1)
            {
                // Rigidbody2D의 linearVelocity를 이용해 총알의 이동 방향과 속도를 설정
                rb2d.linearVelocity = dir * 15f;
            }
        }

        // 충돌이 발생했을 때 호출되는 함수
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 충돌한 오브젝트가 "Enemy" 태그가 아니거나, 관통 횟수가 -1인 경우는 무시
            if (!collision.CompareTag("Enemy") || per == -1)
                return;

            // 관통 횟수를 하나 줄임
            per--;

            // 관통 횟수가 -1이 되면, 총알이 멈추고 비활성화
            if (per == -1)
            {
                // 속도를 0으로 만들어서 총알의 이동을 멈춤
                rb2d.linearVelocity = Vector2.zero;
                // 총알을 비활성화 (게임 오브젝트를 꺼서 더 이상 충돌하지 않게 함)
                gameObject.SetActive(false);
            }
        }
    }
}
