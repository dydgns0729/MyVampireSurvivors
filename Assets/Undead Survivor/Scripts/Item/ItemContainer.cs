using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace MyVampireSurvivors
{
    // ItemSlot 클래스: 아이템을 컨테이너에 저장하기 위한 슬롯
    [System.Serializable]
    public class ItemSlot
    {
        // 이 슬롯에 저장된 아이템
        public ItemSO item;

        // 이 슬롯에 저장된 아이템의 개수
        public int amount;

        public void Copy(ItemSlot itemSlot)
        {
            item = itemSlot.item;
            amount = itemSlot.amount;
        }

        //아이템 슬롯에 아이템과 개수를 설정하는 함수
        public void Set(ItemSO item, int amount)
        {
            this.item = item;
            this.amount = amount;
        }

        //아이템 슬롯을 초기화하는 함수
        public void Clear()
        {
            item = null;
            amount = 0;
        }
    }

    // ScriptableObject인 ItemContainer 클래스: 여러 아이템을 저장하고 관리하는 컨테이너
    [CreateAssetMenu(fileName = "new ItemContainer", menuName = "Data/Item Container")]
    public class ItemContainer : ScriptableObject
    {
        // 컨테이너에 저장된 아이템 슬롯들을 리스트로 관리
        public List<ItemSlot> itemSlots;

        // 아이템을 추가하는 함수
        public void Add(ItemSO item, int count = 1)
        {
            // 아이템이 스택 가능한 경우
            if (item.stackable)
            {
                // 이미 동일한 아이템이 존재하는 슬롯 찾기
                ItemSlot itemSlot = itemSlots.Find(slot => slot.item == item);
                if (itemSlot != null)
                {
                    // 같은 아이템이 존재하면 개수만 증가
                    itemSlot.amount += count;
                }
                else
                {
                    // 동일한 아이템이 없으면 빈 슬롯(아이템이 null인 곳) 찾기
                    itemSlot = itemSlots.Find(slot => slot.item == null);
                    if (itemSlot != null)
                    {
                        // 빈 슬롯에 아이템 추가 및 개수 설정
                        itemSlot.item = item;
                        itemSlot.amount = count;
                    }
                }
            }
            else  // 아이템이 스택 불가능한 경우 (예: 무기, 도구 등)
            {
                // 빈 슬롯(아이템이 null인 곳) 찾기
                ItemSlot itemSlot = itemSlots.Find(slot => slot.item == null);
                if (itemSlot != null)
                {
                    // 빈 슬롯에 아이템 추가 (개수는 필요 없음)
                    itemSlot.item = item;
                }
            }
        }
    }
}
