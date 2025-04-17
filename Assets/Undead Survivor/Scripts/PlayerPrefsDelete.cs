using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyVampireSurvivors
{
    public class PlayerPrefsDelete : MonoBehaviour
    {
        #region Variables

        #endregion

        public void DeletePlayerPrefs()
        {
            // 모든 PlayerPrefs 삭제
            PlayerPrefs.DeleteAll();
            // PlayerPrefs 저장
            PlayerPrefs.Save();
            SceneManager.LoadScene(0); // 메인 메뉴 씬으로 이동
        }
    }
}