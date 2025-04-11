using System.Collections.Generic;
using UnityEngine;

namespace MyVampireSurvivors
{
    // 'ItemDropTableSO'는 아이템 드랍 테이블을 정의하는 ScriptableObject입니다.
    // 유니티에서 에디터를 통해 쉽게 생성할 수 있도록 [CreateAssetMenu] 속성을 사용합니다.
    [CreateAssetMenu(fileName = "IDT_NewMonster", menuName = "Data/Item Drop Table")]
    public class ItemDropTableSO : ScriptableObject
    {
        // 'Items' 클래스는 드랍할 아이템과 해당 아이템이 드랍될 확률(무게)을 나타냅니다.
        // 'ItemSO'는 드랍할 아이템의 데이터 클래스를 가리킵니다.
        [System.Serializable]
        public class Items
        {
            public ItemSO item;   // 드랍될 아이템
            public int weight;     // 아이템이 드랍될 확률을 나타내는 가중치
        }

        // 아이템과 그 가중치가 담긴 리스트
        public List<Items> items = new List<Items>();

        // 아이템을 랜덤하게 선택하는 메서드
        protected ItemSO PickItem()
        {
            int sum = 0;

            // 모든 아이템의 가중치를 합산하여 총합을 구함
            foreach (var item in items)
            {
                sum += item.weight;
            }

            // 랜덤 숫자(0 ~ 총합) 생성
            var rnd = Random.Range(0, sum);

            // 가중치를 기반으로 아이템을 선택
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                // 생성된 랜덤 숫자가 현재 아이템의 가중치보다 작으면 이 아이템을 선택
                if (item.weight > rnd)
                {
                    return items[i].item;
                }
                else   // 랜덤 숫자가 가중치보다 크다면, 이 아이템의 가중치를 빼서 새로운 기준을 설정
                {
                    rnd -= item.weight;
                }
            }

            // 만약 어떤 아이템도 선택되지 않았다면 null 반환
            return null;
        }

        // 아이템을 특정 위치에 드랍하는 메서드
        public void ItemDrop(Vector3 pos)
        {
            // 아이템을 랜덤하게 선택
            var item = PickItem();

            // 아이템이 선택되지 않으면 함수 종료
            if (item == null) return;

            // 선택된 아이템을 해당 위치에 인스턴스화하여 생성
            Instantiate(item.prefab, pos, Quaternion.identity);
        }
    }
}
