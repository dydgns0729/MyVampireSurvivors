using System;
using System.Collections.Generic;
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

        public Action inventoryChanged;  // 인벤토리 변경 시 호출할 델리게이트

        public void Add(ItemSO item, int count = 1)
        {
            if (item.stackable)
            {
                // 먼저 기존 슬롯에 채워 넣기
                for (int i = 0; i < itemSlots.Count && count > 0; i++)
                {
                    ItemSlot slot = itemSlots[i];

                    // 같은 아이템이고 아직 maxStack 이하인 슬롯
                    if (slot.item == item && slot.amount < item.maxStack)
                    {
                        int space = item.maxStack - slot.amount;
                        int toAdd = Mathf.Min(space, count);
                        slot.amount += toAdd;
                        count -= toAdd;
                    }
                }

                // 남은 개수가 있다면 빈 슬롯에 새로 추가
                for (int i = 0; i < itemSlots.Count && count > 0; i++)
                {
                    ItemSlot slot = itemSlots[i];

                    // 빈 슬롯 찾기
                    if (slot.item == null)
                    {
                        int toAdd = Mathf.Min(item.maxStack, count);
                        slot.item = item;
                        slot.amount = toAdd;
                        count -= toAdd;
                    }
                }

                // 슬롯이 부족해서 남은 count가 있다면 버려짐 (또는 로그로 알려줘도 됨)
            }
            else
            {
                // 스택 불가한 경우엔 슬롯 수 만큼만 채워짐
                for (int i = 0; i < itemSlots.Count && count > 0; i++)
                {
                    ItemSlot slot = itemSlots[i];
                    if (slot.item == null)
                    {
                        slot.item = item;
                        slot.amount = 1;
                        count--;
                    }
                }

                // 슬롯이 부족해서 남은 count가 있다면 버려짐
            }

            inventoryChanged?.Invoke();
        }

        /// <summary>
        /// 아이템을 인벤토리에 추가할 수 있는 여유 공간이 있는지 확인하는 함수
        /// </summary>
        /// <param name="itemSO">확인할 대상 아이템</param>
        /// <returns>여유 공간이 있으면 true, 없으면 false</returns>
        public bool CheckFreeSpace(ItemSO itemSO)
        {
            // [공통] 슬롯 중 '빈 슬롯'이 하나라도 있는지 확인하는 로컬 함수
            // 아이템이 null인 슬롯이 존재하면 true 반환
            bool HasEmptySlot() => itemSlots.Exists(slot => slot.item == null);

            // [1] 아이템이 스택 가능한 경우
            if (itemSO.stackable)
            {
                // 동일한 아이템이 이미 존재하고, 그 슬롯의 수량이 maxStack 미만인 경우가 있는지 확인
                // -> 즉, 기존 스택에 아이템을 추가할 수 있는지 확인
                bool hasStackableSlot = itemSlots.Exists(slot =>
                    slot.item == itemSO && slot.amount + 1 <= itemSO.maxStack);

                // 기존 스택에 추가 가능하거나, 새로운 스택을 만들 수 있는 빈 슬롯이 있다면 true
                return hasStackableSlot || HasEmptySlot();
            }

            // [2] 아이템이 스택 불가능한 경우
            // => 빈 슬롯이 하나라도 있으면 추가 가능
            return HasEmptySlot();
        }

    }
}
