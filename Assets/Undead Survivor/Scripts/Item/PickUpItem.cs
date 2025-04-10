using UnityEngine;

namespace MyVampireSurvivors
{
    public class PickUpItem : MonoBehaviour
    {
        #region Variables
        //따라갈 타겟 (플레이어)오브젝트의 트랜스폼
        Transform player;
        //이동속도
        [SerializeField] float speed = 5f;
        //반응할 범위(캐릭터와의 거리)
        [SerializeField] float pickUpDistance = 1.5f;
        //이 오브젝트가 존재하는 시간
        [SerializeField] float timeToLive = 10f;
        #endregion

        private void Awake()
        {
            player = GameManager.instance.player.transform;
        }

        private void Update()
        {
            //10초 타이머 시작
            timeToLive -= Time.deltaTime;
            //10초가 지나면 오브젝트 파괴
            if (timeToLive < 0f)
            {
                Destroy(gameObject);
            }

            //플레이어와의 거리를 계산
            float distance = Vector3.Distance(transform.position, player.position);
            //*가드 절 (distance가 pickUpDistance보다 크면 이 아래의 코드들을 실행하지 않게 설계)
            if (distance > pickUpDistance) return;
            //오브젝트가 플레이어의 위치로 speed의 속도로 이동
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            //0.1보다 거리가 작아지면 파괴
            if (distance < 0.1f)
            {
                Destroy(gameObject);
            }

        }

    }
}