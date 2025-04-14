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

        private void Awake()
        {
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        void Start()
        {
            maxRange = detectionRadius;
            InvokeRepeating("FireTowerBulletIfTargeted", 0f, fireRate);
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
            bullet.GetComponent<TowerBullet>().Fire(startPoint, targetPoint);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Enemy")) return;


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