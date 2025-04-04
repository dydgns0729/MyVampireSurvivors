using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyVampireSurvivors
{
    public class Item : MonoBehaviour
    {
        #region Variables
        // 아이템의 데이터 정보 (아이템의 종류, 이름, 아이콘 등)
        public ItemData data;

        // 아이템의 레벨
        public int level;

        // 무기 아이템이 있을 경우, 해당 무기를 관리하는 변수
        public Weapon weapon;

        // 장갑이나 신발 아이템이 있을 경우, 해당 장비를 관리하는 변수
        public Gear gear;

        // 아이템 아이콘을 표시할 이미지 컴포넌트
        Image icon;

        // 아이템 레벨을 표시할 텍스트 컴포넌트
        TextMeshProUGUI textLevel;
        TextMeshProUGUI textName;
        TextMeshProUGUI textDesc;
        #endregion

        // 초기화 작업: 아이템 아이콘과 레벨 텍스트 초기화
        private void Awake()
        {
            // 아이템 아이콘을 화면에 표시할 Image 컴포넌트를 가져옴 (자식 오브젝트에서 두 번째 Image)
            icon = GetComponentsInChildren<Image>()[1];
            // 아이템의 아이콘을 설정
            icon.sprite = data.itemIcon;

            // 자식 오브젝트에 있는 텍스트 컴포넌트를 가져옴
            TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
            // 첫 번째 텍스트 컴포넌트를 레벨 텍스트로 설정
            textLevel = texts[0];
            textName = texts[1];
            textDesc = texts[2];
            textName.text = data.itemName;
        }

        private void OnEnable()
        {
            // 현재 레벨을 "Lv.1", "Lv.2" 등 형식으로 텍스트로 표시
            textLevel.text = $"Lv.{(level + 1)}";
            // 아이템의 종류에 따라 처리 로직을 다르게 실행
            switch (data.itemType)
            {
                // 근접 무기나 원거리 무기일 경우
                case ItemData.ItemType.Melee:
                case ItemData.ItemType.Range:
                    // 
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                    break;
                case ItemData.ItemType.Glove:
                case ItemData.ItemType.Shoe:
                    // 장비 아이템의 설명을 설정
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                    break;
                default:
                    textDesc.text = string.Format(data.itemDesc);
                    break;
            }

        }

        // 아이템 클릭 시 실행되는 함수 (아이템 사용/레벨업 처리)
        public void OnClick()
        {
            // 아이템의 종류에 따라 처리 로직을 다르게 실행
            switch (data.itemType)
            {
                // 근접 무기나 원거리 무기일 경우
                case ItemData.ItemType.Melee:
                case ItemData.ItemType.Range:
                    // 레벨이 0일 경우, 새로운 무기를 생성하여 초기화
                    if (level == 0)
                    {
                        // 새로운 GameObject를 생성하여 Weapon 컴포넌트를 추가
                        GameObject newWeapon = new GameObject();
                        weapon = newWeapon.AddComponent<Weapon>();
                        weapon.Init(data);
                    }
                    else
                    {
                        // 레벨업 시, 피해량과 총알 수를 계산하여 무기를 강화
                        float nextDamage = data.baseDamage;
                        int nextCount = 0;

                        // 레벨에 맞는 피해량과 총알 수 증가
                        nextDamage += data.baseDamage * data.damages[level];
                        nextCount += data.counts[level];

                        // 레벨업된 무기로 업데이트
                        weapon.LevelUp(nextDamage, nextCount);
                    }
                    // 레벨을 증가시킴
                    level++;
                    break;

                // 장갑이나 신발일 경우
                case ItemData.ItemType.Glove:
                case ItemData.ItemType.Shoe:
                    // 레벨이 0일 경우, 새로운 장비를 생성하여 초기화
                    if (level == 0)
                    {
                        // 새로운 GameObject를 생성하여 Gear 컴포넌트를 추가
                        GameObject newGear = new GameObject();
                        gear = newGear.AddComponent<Gear>();
                        gear.Init(data);
                    }
                    else
                    {
                        // 레벨업 시, 장비의 능력치를 증가시킴
                        float nextRate = data.damages[level];
                        gear.LevelUp(nextRate);
                    }
                    // 레벨을 증가시킴
                    level++;
                    break;

                // 회복 아이템일 경우
                case ItemData.ItemType.Heal:
                    // 게임 내 플레이어의 체력을 최대 체력으로 회복
                    GameManager.instance.health = GameManager.instance.maxHealth;
                    break;
            }

            // 아이템의 레벨이 최대 레벨에 도달하면 버튼을 비활성화
            if (level == data.damages.Length)
            {
                GetComponent<Button>().interactable = false;
            }
        }
    }
}
