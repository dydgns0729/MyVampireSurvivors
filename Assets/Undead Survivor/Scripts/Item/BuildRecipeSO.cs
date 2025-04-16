using System.Collections.Generic;
using UnityEngine;

namespace MyVampireSurvivors
{
    [CreateAssetMenu(fileName = "NewBuildRecipe", menuName = "Data/Build Recipe")]
    public class BuildRecipeSO : ScriptableObject
    {
        // 재료 클래스: 레시피에 필요한 재료를 정의
        [System.Serializable]
        public class Ingredient
        {
            public ItemSO item;
            public int amount;
        }

        public ItemSO result; // 이 레시피로 완성될 아이템 (예: 타워)
        public List<Ingredient> materials;  //재료 리스트
    }
}
