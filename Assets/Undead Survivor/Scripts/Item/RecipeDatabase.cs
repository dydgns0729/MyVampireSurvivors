using System.Collections.Generic;
using UnityEngine;

namespace MyVampireSurvivors
{
    // 제작 레시피 데이터베이스 클래스
    // 특정 아이템을 만들기 위해 필요한 재료 레시피(BuildRecipeSO)를 검색하는 기능을 제공
    public class RecipeDatabase : MonoBehaviour
    {
        #region Variables
        // 모든 제작 레시피를 저장하는 리스트
        // 인스펙터에서 수동으로 레시피들을 등록해 사용
        public List<BuildRecipeSO> recipes;
        #endregion

        /// <summary>
        /// 주어진 결과 아이템(ItemSO)에 해당하는 제작 레시피(BuildRecipeSO)를 반환하는 함수
        /// </summary>
        /// <param name="resultItem">제작 결과로 나올 아이템</param>
        /// <returns>해당 아이템을 만드는 레시피(BuildRecipeSO), 없으면 null</returns>
        public BuildRecipeSO GetRecipe(ItemSO resultItem)
        {
            // 결과 아이템과 일치하는 레시피를 리스트에서 검색
            return recipes.Find(r => r.result == resultItem);
        }
    }
}