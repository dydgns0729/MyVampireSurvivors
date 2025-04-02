using System;
using UnityEngine;

namespace MyVampireSurvivors
{
    public class Spawner : MonoBehaviour
    {
        #region Variables
        // 적을 생성할 위치들을 저장하는 배열 (자식 오브젝트로 있는 위치들)
        public Transform[] spawnPoints;

        // 적의 생성 데이터 (생성 시간, 스프라이트 타입, 체력, 속도 등)
        public SpawnData[] spawnData;

        // 타이머 변수, 일정 시간 간격으로 적을 생성하기 위한 변수
        float timer;
        // 게임 진행 레벨 (게임 시간에 따른 레벨)
        int level;
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
            // 레벨은 게임 시간을 10으로 나눈 몫을 내림한 값
            level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);

            // 타이머가 일정 시간(0.2초)을 넘으면 적을 생성
            if (timer > spawnData[level].spawnTime)
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
            GameObject enemy = GameManager.instance.poolManager.Get(0);

            // spawnPoints 배열에서 랜덤한 위치를 선택하여 적의 위치로 설정
            // spawnPoints[0]은 Spawner 자신이기 때문에 1부터 시작하여 랜덤 위치를 선택
            enemy.transform.position = spawnPoints[UnityEngine.Random.Range(1, spawnPoints.Length)].position;
            // 적에 대한 초기화 (SpawnData에 따라 속성 설정)
            enemy.GetComponent<Enemy>().Init(spawnData[level]);
        }
    }

    // 적의 생성 데이터에 대한 클래스
    [Serializable]
    public class SpawnData
    {
        // 적이 생성되는 시간 간격
        public float spawnTime;

        // 적의 스프라이트 타입 (어떤 스프라이트를 사용할지)
        public int spriteType;
        // 적의 체력
        public int health;
        // 적의 이동 속도
        public float speed;
    }
}
