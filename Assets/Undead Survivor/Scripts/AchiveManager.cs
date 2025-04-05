using System;
using System.Collections;
using UnityEngine;

namespace MyVampireSurvivors
{
    public class AchiveManager : MonoBehaviour
    {
        #region Variables
        public GameObject[] lockCharacter;
        public GameObject[] unlockCharacter;
        public GameObject uiNotice;

        enum Achive { UnlockPotato, UnloackBean }
        Achive[] achives;
        WaitForSecondsRealtime wait;
        #endregion

        private void Awake()
        {
            achives = Enum.GetValues(typeof(Achive)) as Achive[];

            if (!PlayerPrefs.HasKey("MyData"))
            {
                Init();
            }

            wait = new WaitForSecondsRealtime(5f);
        }

        void Init()
        {
            PlayerPrefs.SetInt("MyData", 1);

            foreach (Achive achive in achives)
            {
                PlayerPrefs.SetInt(achive.ToString(), 0);
            }
        }

        private void Start()
        {
            UnlockCharacter();
        }

        void UnlockCharacter()
        {
            for (int i = 0; i < lockCharacter.Length; i++)
            {
                string achiveName = achives[i].ToString();
                bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
                lockCharacter[i].SetActive(!isUnlock);
                unlockCharacter[i].SetActive(isUnlock);
            }
        }

        private void LateUpdate()
        {
            foreach (Achive achive in achives)
            {
                CheckAchive(achive);
            }
        }

        void CheckAchive(Achive achive)
        {
            bool isAchive = false;
            switch (achive)
            {
                case Achive.UnlockPotato:
                    isAchive = GameManager.instance.kill >= 10;
                    break;
                case Achive.UnloackBean:
                    isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
                    break;
            }

            if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
            {
                PlayerPrefs.SetInt(achive.ToString(), 1);
                for (int i = 0; i < uiNotice.transform.childCount; i++)
                {
                    bool isActive = i == (int)achive;
                    uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
                }

                StartCoroutine(NoticeRoutine());

            }
        }

        IEnumerator NoticeRoutine()
        {
            uiNotice.SetActive(true);

            yield return wait;

            uiNotice.SetActive(false);
        }
    }
}