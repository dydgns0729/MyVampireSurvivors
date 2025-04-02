using UnityEngine;

namespace MyVampireSurvivors
{
    public class GameManager : MonoBehaviour
    {
        #region Variables
        // 싱글톤 패턴을 위한 인스턴스 변수
        public static GameManager instance;

        // 게임 시간 변수, 게임이 진행된 시간
        public float gameTime;
        // 최대 게임 시간을 설정하는 변수 (2분으로 설정)
        public float maxGameTime = 2 * 10f;

        // Player 객체를 관리하는 변수
        public Player player;
        // 풀 관리 클래스 (PoolManager) 변수
        public PoolManager poolManager;
        #endregion

        // 게임 시작 시 호출되는 함수
        private void Awake()
        {
            // GameManager의 인스턴스를 싱글톤 패턴으로 설정
            instance = this;
        }

        // 매 프레임마다 호출되는 함수
        private void Update()
        {
            // 게임 시간이 흐를 때마다 deltaTime 만큼 증가
            gameTime += Time.deltaTime;

            // 게임 시간이 최대 게임 시간을 넘지 않도록 제한
            if (gameTime > maxGameTime)
            {
                gameTime = maxGameTime;
            }
        }
    }
}
