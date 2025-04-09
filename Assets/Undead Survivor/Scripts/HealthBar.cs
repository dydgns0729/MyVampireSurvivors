using System;
using UnityEngine;
using UnityEngine.UI;

namespace MyVampireSurvivors
{
    public class HealthBar : MonoBehaviour
    {
        #region Variables
        public GameObject healthBar;
        public Image fillAmount;

        private Health health;
        #endregion

        private void Awake()
        {
            health = GetComponentInParent<Health>();
        }

        private void OnEnable()
        {
            // 시작시 체력바를 비활성화
            healthBar.SetActive(false);
            health.OnHealthChanged += UpdateHealthBar;
            health.OnDeath += OnDeath;
            fillAmount.fillAmount = health.GetHealthRatio();
        }

        private void OnDisable()
        {
            health.OnHealthChanged -= UpdateHealthBar;
            health.OnDeath -= OnDeath;
        }

        private void UpdateHealthBar()
        {
            if (!healthBar.activeSelf)
            {
                healthBar.SetActive(true);
            }

            fillAmount.fillAmount = health.GetHealthRatio();
        }

        private void OnDeath()
        {
            // 죽음 시 체력바 비활성화
            healthBar.SetActive(false);
        }
    }
}