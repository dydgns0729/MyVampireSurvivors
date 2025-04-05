using System;
using UnityEngine;

namespace MyVampireSurvivors
{
    public class Weapon : MonoBehaviour
    {
        #region Variables
        // 무기의 고유 ID (무기의 종류나 특징을 구분할 때 사용)
        public int id;

        // 무기 프리팹 ID (풀에서 가져올 프리팹을 구분하는 ID)
        public int prefabId;

        // 무기의 피해량 (각 발사체가 적에게 입히는 피해)
        public float damage;

        // 발사되는 총알의 수 (한 번에 발사되는 총알의 수)
        public int count;

        // 무기의 회전 속도 (회전하는 무기의 속도)
        public float speed;

        // 시간 관련 변수 (총알 발사 간격을 조정하기 위해 사용)
        float timer;

        // 플레이어 객체 참조 (무기가 플레이어의 위치나 타겟을 사용하기 위해)
        Player player;
        #endregion

        // 부모 오브젝트에서 Player 컴포넌트를 가져와 player 변수에 저장
        private void Awake()
        {
            // 현재 무기의 부모에 해당하는 Player 컴포넌트를 찾고 할당
            player = GameManager.instance.player;
        }

        // 매 프레임마다 호출되는 업데이트 함수
        private void Update()
        {
            if (!GameManager.instance.isLive) return;

            // 무기의 id에 따라 회전 처리
            switch (id)
            {
                case 0:
                    // id가 0일 경우, 회전 속도에 따라 무기가 시계방향으로 회전
                    transform.Rotate(Vector3.back * speed * Time.deltaTime);
                    break;
                default:
                    // id가 0이 아닐 경우, 타이머를 증가시켜 일정 시간이 지나면 발사
                    timer += Time.deltaTime;

                    // 발사 간격이 끝났을 때 발사
                    if (timer > speed)
                    {
                        timer = 0; // 타이머 리셋
                        Fire(); // Fire() 함수 호출하여 총알 발사
                    }
                    break;
            }

            // V 키를 눌렀을 때 레벨업 처리
            if (Input.GetKeyDown(KeyCode.V))
            {
                // 레벨업: 피해량 10 증가, 총알 수 1 증가
                LevelUp(10, 1);
            }
        }

        // 무기 레벨업 함수 (피해량과 총알의 수를 증가시킴)
        public void LevelUp(float damage, int count)
        {
            // 레벨업에 따라 무기의 피해량을 업데이트
            this.damage = damage * Character.Damage;
            // 발사되는 총알의 수를 증가시킴
            this.count += count;

            // 만약 무기의 id가 0일 경우, 총알을 배치하는 Batch() 함수 호출
            if (id == 0)
                Batch();

            // "ApplyGear" 메시지를 플레이어에게 전달하여 무기 장착을 반영하도록 함
            player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        }

        // 무기 초기화 함수 (무기 id에 따라 초기 설정)
        public void Init(ItemData data)
        {
            // 무기의 이름을 아이템 ID에 따라 설정
            name = "Weapon " + data.itemId;

            // 무기의 부모를 플레이어로 설정하여, 무기가 플레이어와 함께 이동하도록 설정
            transform.parent = player.transform;

            // 무기의 초기 위치를 플레이어의 위치와 동일하게 설정
            transform.localPosition = Vector3.zero;

            // 아이템 데이터에서 가져온 itemId를 이용해 무기의 고유 ID 설정
            id = data.itemId;

            // 아이템의 기본 피해량을 설정
            damage = data.baseDamage * Character.Damage;

            // 아이템의 기본 총알 수를 설정
            count = data.baseCount + Character.Count;

            // 풀에서 해당 아이템에 맞는 발사체 프리팹을 찾아 그 ID를 설정
            for (int i = 0; i < GameManager.instance.poolManager.prefabs.Length; i++)
            {
                if (data.projectile == GameManager.instance.poolManager.prefabs[i])
                {
                    prefabId = i; // 해당 프리팹의 ID 설정
                    break; // 찾으면 반복문 종료
                }
            }

            // 무기 id에 따라 설정을 다르게 처리
            switch (id)
            {
                case 0:
                    // id가 0인 경우, 총알을 원형으로 배치
                    speed = 150f * Character.WeaponSpeed;
                    Batch(); // id가 0일 때, 원형으로 총알 배치
                    break;
                default:
                    speed = 0.5f * Character.WeaponRate;
                    Debug.Log("Weapon.cs Init() default"); // 디버그 로그 출력
                    break;
            }

            #region HandSet
            // 아이템 데이터에서 손에 해당하는 스프라이트를 가져와서 설정
            Hand hand = player.hands[(int)data.itemType];
            hand.spriter.sprite = data.hand; // 손에 스프라이트 설정
            hand.gameObject.SetActive(true); // 손 오브젝트를 활성화
            #endregion

            // "ApplyGear" 메시지를 플레이어에게 전달하여 무기를 장착하도록 함
            player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        }

        // 총알 배치 함수 (무기 id가 0일 때 총알을 원형으로 배치)
        void Batch()
        {
            // 지정된 수(count)만큼 총알을 배치
            for (int i = 0; i < count; i++)
            {
                Transform bullet;

                // 현재 무기 자식으로 이미 존재하는 총알을 사용
                if (i < transform.childCount)
                {
                    bullet = transform.GetChild(i); // 자식으로 있는 총알을 가져옴
                }
                else
                {
                    // 풀에서 새로운 총알을 가져와서 자식으로 추가
                    bullet = GameManager.instance.poolManager.Get(prefabId).transform;
                    bullet.parent = transform; // 총알을 무기의 자식으로 설정
                }

                // 총알 위치 초기화 (무기 위치에서 시작하도록 설정)
                bullet.localPosition = Vector3.zero;
                bullet.localRotation = Quaternion.identity;

                // 총알의 회전 각도를 계산하여 원형으로 배치
                Vector3 rotVec = Vector3.forward * 360 * i / count;
                bullet.Rotate(rotVec);

                // 총알을 원형으로 배치하기 위해 약간의 이동
                bullet.Translate(bullet.up * 1.5f, Space.World);

                // 총알 초기화 (피해량 설정, -1은 무한 관통을 의미)
                bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
            }
        }

        #region OriginFire()
        // 타겟을 향해 총알을 발사하는 함수
        private void Fire()
        {
            // 플레이어가 타겟을 가지고 있지 않으면 발사하지 않음
            if (!player.scanner.nearestTarget)
                return;

            // 타겟의 위치 계산
            Vector3 targetPos = player.scanner.nearestTarget.position;
            // 현재 무기 위치에서 타겟까지의 방향 벡터 계산
            Vector3 dir = targetPos - transform.position;
            // 방향 벡터를 정규화하여 일정한 속도로 발사
            dir = dir.normalized;

            // 풀에서 새로운 총알을 가져옴
            Transform bullet = GameManager.instance.poolManager.Get(prefabId).transform;

            // 총알의 위치를 무기의 위치로 설정
            bullet.position = transform.position;
            // 총알의 회전 방향을 타겟을 향하게 설정
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);

            // 총알 초기화 (피해량, 총알 수, 방향 설정)
            bullet.GetComponent<Bullet>().Init(damage, count, dir);
        }
        #endregion
    }
}
