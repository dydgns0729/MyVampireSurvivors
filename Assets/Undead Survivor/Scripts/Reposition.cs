using UnityEngine;

namespace MyVampireSurvivors
{
    public class Reposition : MonoBehaviour
    {
        #region Variables
        // 2D Collider 변수 (충돌 처리용)
        Collider2D coll;
        #endregion

        // Awake()는 객체가 초기화될 때 호출되는 메서드
        private void Awake()
        {
            // 해당 오브젝트의 2D Collider를 가져옴
            coll = GetComponent<Collider2D>();
        }

        // 2D 콜라이더가 영역을 벗어날 때 호출되는 메서드
        private void OnTriggerExit2D(Collider2D collision)
        {
            // 충돌한 오브젝트가 "Area" 태그를 가지고 있는지 확인
            if (!collision.CompareTag("Area"))
                return; // "Area" 태그가 아니면 메서드 종료

            // 플레이어의 현재 위치를 가져옴
            Vector3 playerPosition = GameManager.instance.player.transform.position;

            // 현재 오브젝트의 위치를 가져옴
            Vector3 myPosition = transform.position;

            // 태그에 따라 다른 동작을 수행
            switch (transform.tag)
            {
                // "Ground" 태그를 가진 오브젝트에 대한 처리
                case "Ground":
                    float diffX = playerPosition.x - myPosition.x; // X축 차이
                    float diffY = playerPosition.y - myPosition.y; // Y축 차이
                    float dirX = diffX < 0 ? -1 : 1; // X축 방향 (왼쪽 또는 오른쪽)
                    float dirY = diffY < 0 ? -1 : 1; // Y축 방향 (아래 또는 위)
                    diffX = Mathf.Abs(diffX); // X축 차이의 절대값
                    diffY = Mathf.Abs(diffY); // Y축 차이의 절대값

                    if (diffX > diffY)
                    {
                        transform.Translate(Vector3.right * dirX * 60);
                    }
                    // Y 차이가 더 큰 경우 (Y축으로만 이동)
                    else if (diffX < diffY)
                    {
                        transform.Translate(Vector3.up * dirY * 60);
                    }
                    break;

                // "Enemy" 태그를 가진 오브젝트에 대한 처리
                case "Enemy":
                    // Collider가 활성화된 경우
                    if (coll.enabled)
                    {
                        Vector3 dist = playerPosition - myPosition; // 플레이어와의 거리 계산
                        Vector3 rand = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0f);
                        // 플레이어 방향에 따른 이동 (약간의 랜덤한 이동 추가)
                        transform.Translate(rand + dist * 2);
                    }
                    break;
            }
        }
    }
}
