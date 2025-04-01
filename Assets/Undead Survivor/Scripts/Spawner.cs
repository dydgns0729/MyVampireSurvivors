using UnityEngine;

namespace MyVampireSurvivors
{
    public class Spawner : MonoBehaviour
    {
        #region Variables
        // 적을 생성할 위치들을 저장하는 배열 (자식 오브젝트로 있는 위치들)
        public Transform[] spawnPoints;

        // 타이머 변수, 일정 시간 간격으로 적을 생성하기 위한 변수
        float timer;
        #endregion

        // 초기화: 자식 오브젝트로 있는 spawnPoints를 가져옴
        private void Awake()
        {
            // 현재 오브젝트의 자식들 중 Transform 컴포넌트를 모두 가져옴
            spawnPoints = GetComponentsInChildren<Transform>();
        }

        private void Update()
        {
            // 타이머를 deltaTime 만큼 증가시켜줌 (프레임당 시간 간격)
            timer += Time.deltaTime;

            // 타이머가 일정 시간(0.2초)을 넘으면 적을 생성
            if (timer > 0.2f)
            {
                // Spawn() 메서드를 호출하여 적 생성
                Spawn();
                // 타이머 리셋
                timer = 0f;
            }
        }

        // 적을 생성하는 메서드
        void Spawn()
        {
            // 풀에서 적 오브젝트를 가져와 enemy 변수에 할당
            GameObject enemy = GameManager.instance.poolManager.Get(Random.Range(0, GameManager.instance.poolManager.prefabs.Length));

            // spawnPoints 배열에서 랜덤한 위치를 선택하여 적의 위치로 설정
            // spawnPoints[0]은 Spawner 자신이기 때문에 1부터 시작하여 랜덤 위치를 선택
            enemy.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
        }
    }
}
