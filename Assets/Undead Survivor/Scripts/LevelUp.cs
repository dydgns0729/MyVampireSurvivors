using UnityEngine;

namespace MyVampireSurvivors
{
    public class LevelUp : MonoBehaviour
    {
        #region Variables
        RectTransform rect;

        Item[] items;
        #endregion

        private void Awake()
        {
            // RectTransform 컴포넌트를 가져옴
            rect = GetComponent<RectTransform>();
            items = GetComponentsInChildren<Item>(true);
        }

        public void Show()
        {
            Next();
            rect.localScale = Vector3.one;
            GameManager.instance.Stop();
        }

        public void Hide()
        {
            rect.localScale = Vector3.zero;
            GameManager.instance.Resume();
        }

        public void Select(int index)
        {
            items[index].OnClick();
        }

        void Next()
        {
            //1. 모든 아이템 비활성화
            foreach (Item item in items)
            {
                item.gameObject.SetActive(false);
            }

            //2. 랜덤으로 아이템 3개 활성화
            int[] ran = new int[3];
            while (true)
            {
                ran[0] = Random.Range(0, items.Length);
                ran[1] = Random.Range(0, items.Length);
                ran[2] = Random.Range(0, items.Length);
                if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                {
                    break;
                }
            }

            for (int i = 0; i < ran.Length; i++)
            {
                Item ranItem = items[ran[i]];

                //3. 만렙 아이템의 경우는 소비아이템으로 대체
                if (ranItem.level == ranItem.data.damages.Length)
                {
                    items[4].gameObject.SetActive(true);
                }
                else
                {
                    ranItem.gameObject.SetActive(true);
                }
            }

        }
    }
}