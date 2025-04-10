using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

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
    }

    // ScriptableObject인 ItemContainer 클래스: 여러 아이템을 저장하고 관리하는 컨테이너
    [CreateAssetMenu(fileName = "new ItemContainer", menuName = "Data/Item Container")]
    public class ItemContainer : ScriptableObject
    {
        // 컨테이너에 저장된 아이템 슬롯들을 리스트로 관리
        public List<ItemSlot> itemSlots;

        // 아이템을 컨테이너에 추가하는 메서드
        public void Add(ItemSO item, int amount = 1)
        {
            // 이미 존재하는 아이템 슬롯을 찾아봄
            ItemSlot itemSlot = itemSlots.Find(slot => slot.item == item);

            // 아이템이 이미 존재하면 개수를 증가시킴
            if (itemSlot != null)
            {
                itemSlot.amount += amount;
            }
            else
            {
                // 아이템이 없으면 비어있는 슬롯을 찾아봄
                itemSlot = itemSlots.Find(slot => slot.item == null);

                // 비어있는 슬롯이 있으면 아이템을 추가
                if (itemSlot != null)
                {
                    itemSlot.item = item;
                    itemSlot.amount = amount;
                }
            }
        }
    }
}
