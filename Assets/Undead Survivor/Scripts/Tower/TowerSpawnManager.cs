using System;
using UnityEngine;

namespace MyVampireSurvivors
{
    public class TowerSpawnManager : MonoBehaviour
    {
        #region Variables
        public static TowerSpawnManager instance;

        #endregion
        private void Awake()
        {
            instance = this;
        }

        //위치, 아이템을 받아서 아이템을 생성한다.
        public void SpawnTower(Vector3 worldPosition, Transform prefab)
        {
            Instantiate(prefab, worldPosition, Quaternion.identity, this.transform);
        }
    }
}