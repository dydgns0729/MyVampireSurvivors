using UnityEngine;

namespace MyVampireSurvivors
{
    public class Gear : MonoBehaviour
    {
        #region Variables
        // 장비의 종류 (장갑, 신발 등) 아이템 유형을 저장
        public ItemData.ItemType type;

        // 장비의 효과를 적용하는 비율 (예: 장갑 효과 비율, 신발 속도 증가 비율 등)
        public float rate;
        #endregion

        // 장비를 초기화하는 함수 (아이템 데이터 기반으로 설정)
        public void Init(ItemData data)
        {
            // 장비 이름을 "Gear" + 아이템 ID로 설정
            name = "Gear " + data.itemId;

            // 장비의 부모를 플레이어로 설정하여 플레이어와 함께 이동하도록 함
            transform.parent = GameManager.instance.player.transform;

            // 장비의 위치를 플레이어의 위치와 동일하게 설정
            transform.localPosition = Vector3.zero;

            // 아이템 데이터에서 장비 유형과 효과 비율을 가져와서 설정
            type = data.itemType;
            rate = data.damages[0];

            // 장비의 효과를 적용
            ApplyGear();
        }

        // 장비 레벨업 함수 (효과 비율을 업데이트)
        public void LevelUp(float rate)
        {
            // 레벨업 시, 새로운 효과 비율을 설정하고 장비 효과를 적용
            this.rate = rate;
            ApplyGear();
        }

        // 장비의 효과를 적용하는 함수
        // 장비 종류에 따라 다르게 처리
        void ApplyGear()
        {
            switch (type)
            {
                // 장갑일 경우, 장비의 효과를 증가시키는 함수 호출
                case ItemData.ItemType.Glove:
                    RateUp();
                    break;
                // 신발일 경우, 플레이어의 속도를 증가시키는 함수 호출
                case ItemData.ItemType.Shoe:
                    SpeedUp();
                    break;
            }
        }

        // 장갑 장비의 효과를 적용하는 함수 (무기의 공격 속도 증가)
        void RateUp()
        {
            // 플레이어의 자식 오브젝트로 있는 모든 무기들을 가져옴
            Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

            // 각 무기의 속도를 장갑의 효과에 맞게 증가
            foreach (Weapon weapon in weapons)
            {
                switch (weapon.id)
                {
                    // 무기의 id가 0일 경우, 속도를 150에 비례하여 증가
                    case 0:
                        float speed = 150 * Character.WeaponSpeed;
                        weapon.speed = 150 + (150 * rate);
                        break;
                    // 그 외의 무기들은 기본 속도의 비율을 rate에 맞춰 변경
                    default:
                        speed = 0.5f * Character.WeaponRate;
                        weapon.speed = 0.5f * (1f - rate);
                        break;
                }
            }
        }

        // 신발 장비의 효과를 적용하는 함수 (플레이어의 이동 속도 증가)
        void SpeedUp()
        {
            // 기본 속도 설정
            float speed = 5 * Character.Speed;

            // 플레이어의 이동 속도를 신발의 효과 비율에 맞춰 증가
            GameManager.instance.player.speed = speed + speed * rate;
        }
    }
}
