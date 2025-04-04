using UnityEngine;

namespace MyVampireSurvivors
{
    public class Result : MonoBehaviour
    {
        #region Variables
        public GameObject[] titles;
        #endregion

        public void Lose()
        {
            titles[0].SetActive(true);
        }

        public void Win()
        {
            titles[1].SetActive(true);
        }
    }
}