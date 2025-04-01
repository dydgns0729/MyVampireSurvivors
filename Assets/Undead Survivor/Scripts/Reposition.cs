using UnityEngine;

namespace MyVampireSurvivors
{
    public class Reposition : MonoBehaviour
    {
        #region Variables
        Collider2D coll;
        #endregion

        private void Awake()
        {
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

            // 플레이어와 현재 오브젝트 간의 X, Y 좌표 차이 계산
            float diffX = Mathf.Abs(playerPosition.x - myPosition.x);
            float diffY = Mathf.Abs(playerPosition.y - myPosition.y);

            // 플레이어의 입력 방향 벡터
            Vector3 playerDir = GameManager.instance.player.inputVec;
            // 플레이어의 X, Y 방향을 각각 1 또는 -1로 설정 (이동 방향)
            float dirX = playerDir.x < 0 ? -1 : 1;
            float dirY = playerDir.y < 0 ? -1 : 1;

            // 태그에 따라 다른 동작을 수행
            switch (transform.tag)
            {
                // "Ground" 태그를 가진 오브젝트에 대한 처리
                case "Ground":
                    // X와 Y의 차이가 비슷한 경우 (대각선 방향으로 이동)
                    if (Mathf.Abs(diffX - diffY) <= 0.1f)
                    {
                        // 대각선 이동
                        transform.Translate(Vector3.up * dirY * 40); // Y축으로 이동
                        transform.Translate(Vector3.right * dirX * 40); // X축으로 이동
                    }
                    // X 차이가 더 큰 경우 (X축으로만 이동)
                    else if (diffX > diffY)
                    {
                        transform.Translate(Vector3.right * dirX * 40);
                    }
                    // Y 차이가 더 큰 경우 (Y축으로만 이동)
                    else if (diffX < diffY)
                    {
                        transform.Translate(Vector3.up * dirY * 40);
                    }
                    break;

                // "Enemy" 태그를 가진 오브젝트에 대한 처리 (현재는 빈 케이스)
                case "Enemy":
                    if (coll.enabled)
                    {
                        transform.Translate(playerDir * 25 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0));
                    }
                    break;
            }
        }
    }
}
