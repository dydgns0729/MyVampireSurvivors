using System;
using UnityEngine;

namespace MyVampireSurvivors
{
    public class TowerSpawnManager : MonoBehaviour
    {
        #region Variables
        public static TowerSpawnManager instance;

        int prefabId; // 프리팹 ID (풀에서 가져올 프리팹을 구분하는 ID)
        #endregion
        private void Awake()
        {
            instance = this;
        }

        //위치, 아이템을 받아서 아이템을 생성한다.
        public void SpawnTower(Vector3 worldPosition, Transform prefab)
        {
            // 풀에서 해당 아이템에 맞는 발사체 프리팹을 찾아 그 ID를 설정
            for (int i = 0; i < GameManager.instance.poolManager.prefabs.Length; i++)
            {
                if (prefab.gameObject == GameManager.instance.poolManager.prefabs[i])
                {
                    prefabId = i; // 해당 프리팹의 ID 설정
                    break; // 찾으면 반복문 종료
                }
            }

            // 지정된 위치에 타워 프리팹을 생성
            GameObject itemGO = GameManager.instance.poolManager.Get(prefabId).gameObject;
            itemGO.transform.position = worldPosition; // 생성된 타워의 위치 설정

            // 플레이어 오브젝트안에 있는 모든 기어들의 ApplyGear 메서드를 호출하여 장비 효과를 적용
            GameManager.instance.player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        }
    }
}