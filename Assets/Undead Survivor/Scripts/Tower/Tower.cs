using MyVampireSurvivors;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyVampireSurvivors
{
    public class Tower : MonoBehaviour
    {
        public GameObject towerBulletPrefab; // 타워 총알 프리팹
        public float detectionRadius = 10f; // 적 감지 반경
        public float fireRate = 2f; // 발사 주기 (초)
        public float minRange = 3f; // 최소 감지 범위
        public float maxRange; // 최대 감지 범위 (detectionRadius와 같게 설정됨)

        private Transform currentTarget; // 현재 타겟

        private Animator animator; // 애니메이터 컴포넌트

        Health health; // 체력 컴포넌트

        public float maxDamage = 20f; // 폭발로 인한 데미지


        private void Awake()
        {
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        // 타워의 최대 감지 범위를 설정하고, 발사 주기에 따라 총알을 발사하는 메서드 호출
        void OnEnable()
        {
            maxRange = detectionRadius;
            InvokeRepeating("FireTowerBulletIfTargeted", 0f, fireRate);

            // 현재 오브젝트의 모든 자식 오브젝트를 비활성화
            int childCount = gameObject.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                GameObject child = gameObject.transform.GetChild(i).gameObject;
                child.SetActive(true);
            }

            gameObject.GetComponent<Collider2D>().enabled = true; // 충돌체 비활성화

            health.OnDeath += OnTowerDestroy; // 타워가 파괴될 때 호출되는 이벤트 등록
        }

        private void OnDisable()
        {
            health.OnDeath -= OnTowerDestroy; // 타워가 파괴될 때 호출되는 이벤트 해제
        }

        void Update()
        {
            // 적 감지 범위 내에 적이 있을 경우 타겟으로 설정
            DetectTarget();
        }

        private void DetectTarget()
        {
            Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
            Transform closestEnemy = null;
            float closestDistance = float.MaxValue; // 가장 가까운 적의 거리

            foreach (var enemy in enemiesInRange)
            {
                if (enemy.CompareTag("Enemy"))
                {
                    float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

                    // 적이 minRange 내에 있지 않으면, minRange와 maxRange 사이에서 가장 가까운 적을 찾음
                    if (distanceToEnemy > minRange && distanceToEnemy <= maxRange)
                    {
                        if (distanceToEnemy < closestDistance)
                        {
                            closestDistance = distanceToEnemy;
                            closestEnemy = enemy.transform; // 가장 가까운 적을 타겟으로 설정
                        }
                    }
                }
            }

            // 가장 가까운 적이 있으면 currentTarget을 그로 설정
            if (closestEnemy != null)
            {
                currentTarget = closestEnemy;
            }
            else
            {
                currentTarget = null; // 타겟이 없으면 null로 설정
            }
        }

        // 타겟이 있으면 타워 총알을 발사
        private void FireTowerBulletIfTargeted()
        {
            if (currentTarget != null)
            {
                Vector3 targetPosition = currentTarget.position;
                FireTowerBullet(transform.position, targetPosition);
            }
        }

        // 타워 총알 발사
        private void FireTowerBullet(Vector3 startPoint, Vector3 targetPoint)
        {
            animator.SetTrigger("Fire"); // 애니메이션 트리거 설정

            // 타워 총알을 생성하고(오브젝트 풀링 프리팹 3번), Fire 메서드 호출
            GameObject bullet = GameManager.instance.poolManager.Get(3);
            bullet.transform.localScale = Vector3.one; // 스케일 초기화
            bullet.GetComponent<TowerBullet>().Fire(startPoint, targetPoint, maxDamage);
        }

        public void ChangeDamage(float damage)
        {
            maxDamage = damage; // 타워의 최대 데미지 변경
        }

        // 타워의 발사 속도 변경
        public void ChangeFireRate(float newFireRate)
        {
            // 현재 발사 속도를 변경
            fireRate = newFireRate;
            // InvokeRepeating을 사용하여 발사 속도 변경(Update 지양)
            CancelInvoke("FireTowerBulletIfTargeted");
            InvokeRepeating("FireTowerBulletIfTargeted", 0f, fireRate);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            // Enemy와 충돌시, 게임이 진행 중인지 확인
            if (!GameManager.instance.isLive || !collision.gameObject.CompareTag("Enemy") || !health.isLive) return;

            // 포탑의 체력을 감소시킴
            health.TakeDamage(Time.deltaTime * 10f);
        }

        private void OnTowerDestroy()
        {
            CancelInvoke("FireTowerBulletIfTargeted"); // 비활성화 시 발사 중지

            // 현재 오브젝트의 모든 자식 오브젝트를 비활성화
            int childCount = gameObject.transform.childCount;

            for (int i = 0; i < childCount; i++)
            {
                GameObject child = gameObject.transform.GetChild(i).gameObject;
                child.SetActive(false);
            }
            animator.SetBool("Destroy", true); // 파괴 애니메이션 실행

            gameObject.GetComponent<Collider2D>().enabled = false; // 충돌체 비활성화

        }

        // 타겟을 감지하는 반경을 시각적으로 보여주는 Gizmos
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius); // 감지 범위
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, minRange); // 최소 감지 범위
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, maxRange); // 최대 감지 범위
        }
    }
}