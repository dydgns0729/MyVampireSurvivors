using UnityEngine;

namespace MyVampireSurvivors
{
    public class Follow : MonoBehaviour
    {
        #region Variables
        RectTransform rect;
        #endregion
        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }
        private void FixedUpdate()
        {
            rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
        }
    }
}