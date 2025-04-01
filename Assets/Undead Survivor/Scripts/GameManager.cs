using UnityEngine;

namespace MyVampireSurvivors
{
    public class GameManager : MonoBehaviour
    {
        #region Variables
        public static GameManager instance;
        public Player player;
        #endregion

        private void Awake()
        {
            instance = this;
        }
    }
}