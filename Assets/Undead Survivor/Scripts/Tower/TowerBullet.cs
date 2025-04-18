using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyVampireSurvivors
{
    public class TowerBullet : MonoBehaviour
    {
        public float gravity = 9.8f; // 중력 값
        public float height = 5f; // 포물선의 높이
        public float duration = 2f; // 이동 시간 (고정값)
        public float explosionRadius = 2f; // 폭발 반경 (대충 5로 초기 설정)
        public float maxDamage; // 폭발로 인한 데미지(기본 20 // Tower.maxDamage)

        public LayerMask targetLayer;   // 적 레이어 마스크

        // 발사 시작 지점 (총알이 발사되는 위치)
        private Vector3 startPoint;

        // 목표 지점 (총알이 날아가는 목표 위치)
        private Vector3 targetPoint;

        // 총알을 발사하는 함수
        // 이 함수는 발사 시작 지점과 목표 지점을 받아, 총알의 이동을 시작합니다.
        public void Fire(Vector3 startPoint, Vector3 targetPoint, float maxDamage)
        {
            this.startPoint = startPoint;  // 시작 지점 설정
            this.targetPoint = targetPoint;  // 목표 지점 설정
            this.maxDamage = maxDamage;  // 최대 데미지 설정

            // 이동 시간(duration)을 고정값으로 사용하여, 총알을 포물선 경로로 이동시킴
            StartCoroutine(MoveBulletAlongParabola());
        }

        // 포물선 경로를 따라 총알을 이동시키는 코루틴 함수
        // 이 함수는 총알이 포물선을 그리며 목표 지점으로 이동하게 만듭니다.
        private IEnumerator MoveBulletAlongParabola()
        {
            float elapsedTime = 0f;  // 경과 시간 (이 값은 0부터 시작하여 duration까지 증가합니다.)

            // 총알이 목표 지점에 도달할 때까지 반복
            while (elapsedTime < duration)
            {
                // 현재 경과 시간에 따라 t 값을 계산 (0.0에서 1.0 사이)
                float t = elapsedTime / duration;

                // 포물선 경로 계산: 
                // x와 y 좌표를 보간하여 총알을 목표 지점까지 이동시킴
                float x = Mathf.Lerp(startPoint.x, targetPoint.x, t);  // x 좌표는 선형 보간
                float y = Mathf.Lerp(startPoint.y, targetPoint.y, t) + Mathf.Sin(t * Mathf.PI) * height;  // y 좌표는 포물선 형태로 보간

                // 총알의 위치 업데이트
                transform.position = new Vector3(x, y, startPoint.z);

                // 경과 시간 업데이트
                elapsedTime += Time.deltaTime;

                // 한 프레임 대기 후 다시 실행
                yield return null;
            }

            // 목표 지점에 정확히 도달하도록 설정 (이 부분은 주석 처리되어 있지만, 필요시 활성화할 수 있습니다.)
            //transform.position = targetPoint;

            // 목표 지점에 도달한 후, 폭발을 시작합니다.
            StartCoroutine(Explosion());
        }

        // 폭발 효과를 처리하는 코루틴 함수
        // 이 함수는 총알이 목표 지점에 도달한 후 폭발 효과를 발생시키고, 총알을 비활성화합니다.
        private IEnumerator Explosion()
        {
            //사운드 재생
            AudioManager.instance.PlaySFX(AudioManager.SFX.TowerExplosion);

            // 총알 크기를 0으로 설정하여 점차적으로 사라지는 효과를 줌
            gameObject.transform.localScale = Vector3.zero;

            // 폭발 효과를 풀에서 가져옴 (풀에서 4번 아이템을 가져옴)
            GameObject effectGO = GameManager.instance.poolManager.Get(4);

            // 폭발 효과의 위치를 현재 총알의 위치로 설정
            effectGO.transform.position = transform.position;

            // 폭발 후 범위 내 모든 적들에게 데미지 적용
            HitInRange();

            // 폭발 효과가 0.5초 동안 유지되도록 대기
            yield return new WaitForSeconds(0.5f);

            // 폭발 효과가 끝났으므로 해당 효과를 비활성화
            effectGO.SetActive(false);

            // 총알도 비활성화하여 더 이상 보이지 않도록 처리
            gameObject.SetActive(false);
        }

        // 폭발 범위 내 적들에게 데미지를 적용하는 함수
        private void HitInRange()
        {
            // 폭발 중심에서 반경 내의 모든 객체를 탐지 (CircleCollider2D 또는 다른 Collider2D를 사용)
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayer);

            // 범위 내 모든 콜라이더를 체크
            foreach (var collider in hitColliders)
            {
                // Collider가 Health 컴포넌트를 가지고 있다면 데미지를 적용
                Health health = collider.GetComponent<Health>();
                if (health != null)
                {
                    // 폭발 중심에서 해당 객체까지의 거리 계산
                    float distance = Vector3.Distance(transform.position, collider.transform.position);

                    // 거리가 가까울수록 더 많은 데미지 적용 (멀어질수록 데미지 감소)
                    float damageFactor = 1 - (distance / explosionRadius); // 거리 비례 감소
                    float finalDamage = Mathf.Max(maxDamage * damageFactor, 0); // 최소 데미지 0 설정

                    health.TakeDamage(finalDamage); // TakeDamage 메서드를 호출하여 데미지 적용
                }
            }
        }

        // 폭발 반경을 기즈모로 시각화
        private void OnDrawGizmos()
        {
            // 폭발 반경을 원으로 시각화
            Gizmos.color = Color.green; // 반경 색을 붉은색으로 설정
            Gizmos.DrawWireSphere(transform.position, explosionRadius); // 폭발 반경에 원을 그림
        }
    }
}