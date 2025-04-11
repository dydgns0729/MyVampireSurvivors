using UnityEngine;

namespace MyVampireSurvivors
{
    public class ItemSpawnManager : MonoBehaviour
    {
        #region Variables
        public static ItemSpawnManager instance;

        //아이템을 생성할 프리팹
        [SerializeField] GameObject pickUpItemPrefab;
        #endregion

        private void Awake()
        {
            instance = this;
        }

        //위치, 아이템, 갯수를 받아서 아이템을 생성한다.
        public void SpawnItem(Vector3 position, ItemSO item, int count = 1)
        {
            //prefab을 생성한다.
            GameObject itemGO = Instantiate(pickUpItemPrefab, position, Quaternion.identity, this.transform);
            //생성된 prefab에 아이템을 설정한다.
            itemGO.GetComponent<PickUpItem>().Set(item, count);
        }
    }
}