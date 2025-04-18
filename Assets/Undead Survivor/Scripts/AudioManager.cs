using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace MyVampireSurvivors
{
    public class AudioManager : MonoBehaviour
    {
        #region Variables
        public static AudioManager instance;

        [Header("#BGM")]
        public AudioClip bgmClip;
        public float bgmVolumn;
        AudioSource bgmPlayer;
        AudioHighPassFilter bgmEffect;

        [Header("#SFX")]
        public AudioClip[] sfxClips;
        public float sfxVolumn;
        public int channels;
        AudioSource[] sfxPlayers;
        int channelIndex;

        [Header("VolumeControl")]
        public AudioMixer audioMixer;
        public Slider bgmSlider;
        public Slider sfxSlider;
        #endregion

        public enum SFX
        {
            Dead,
            Hit,
            LevelUp = 3,
            Lose,
            Melee,
            Range = 7,
            Select,
            Win,
            TowerFire,
            TowerExplosion
        }

        private void Awake()
        {
            instance = this; // 싱글톤 인스턴스 설정

            bgmSlider.onValueChanged.AddListener((value) => SetVolume(value, "BGM")); // BGM 슬라이더 값 변경 시 볼륨 설정
            sfxSlider.onValueChanged.AddListener((value) => SetVolume(value, "SFX")); // SFX 슬라이더 값 변경 시 볼륨 설정

            Init();
        }

        private void Start()
        {
            //Init()에서 초기화시 모종의 이유로 값을 믹서에 적용되지 않음
            bgmSlider.value = GetVolume("BGM"); // BGM 슬라이더 초기화
            sfxSlider.value = GetVolume("SFX"); // SFX 슬라이더 초기화
        }

        private void Init()
        {
            //배경음 플레이어 초기화
            GameObject bgmObject = new GameObject("BGMPlayer");
            bgmObject.transform.SetParent(transform); // AudioManager의 자식으로 설정
            bgmPlayer = bgmObject.AddComponent<AudioSource>(); // AudioSource 컴포넌트 추가   
            bgmPlayer.playOnAwake = false; // 자동 재생 안 함
            bgmPlayer.loop = true; // 반복 재생
            bgmPlayer.volume = bgmVolumn; // 볼륨 설정
            bgmPlayer.clip = bgmClip; // BGM 클립 설정
            bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>(); // 카메라의 AudioHighPassFilter 컴포넌트 가져오기

            bgmPlayer.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0]; // BGM 믹서 그룹 설정
            //효과음 플레이어 초기화
            GameObject sfxObject = new GameObject("SFXPlayer");
            sfxObject.transform.SetParent(transform); // AudioManager의 자식으로 설정
            sfxPlayers = new AudioSource[channels]; // 채널 수만큼 AudioSource 배열 생성 
            for (int i = 0; i < sfxPlayers.Length; i++)
            {
                sfxPlayers[i] = sfxObject.AddComponent<AudioSource>(); // AudioSource 컴포넌트 추가
                sfxPlayers[i].playOnAwake = false; // 자동 재생 안 함
                sfxPlayers[i].loop = false; // 반복 재생 안 함
                sfxPlayers[i].volume = sfxVolumn; // 볼륨 설정
                sfxPlayers[i].bypassEffects = true; // SFX에는 사운드 이펙트를 적용하지 않음
                sfxPlayers[i].outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0]; // BGM 믹서 그룹 설정
            }
        }

        public void PlayBGM(bool isPlay)
        {
            if (isPlay)
            {
                bgmPlayer.Play(); // BGM 재생
            }
            else
            {
                bgmPlayer.Stop(); // BGM 정지
            }
        }
        public void EffectBGM(bool isPlay)
        {
            bgmEffect.enabled = isPlay; // BGM 효과음 활성화
        }

        public void PlaySFX(SFX sfx)
        {
            for (int i = 0; i < sfxPlayers.Length; i++)
            {
                int loopIndex = (i + channelIndex) % sfxPlayers.Length; // 현재 채널 인덱스 계산

                if (sfxPlayers[loopIndex].isPlaying) // 현재 채널이 사용 중이지 않으면
                {
                    continue; // 다음 채널로 이동
                }
                int ranIndex = 0;
                switch (sfx)
                {
                    case SFX.Hit:
                    case SFX.Melee:
                        ranIndex = UnityEngine.Random.Range(0, 2); // 0 또는 1
                        sfx += ranIndex; // 랜덤으로 Hit 또는 Melee 선택
                        break;
                    default:
                        break;
                }

                channelIndex = loopIndex; // 현재 채널 인덱스 업데이트

                sfxPlayers[loopIndex].clip = sfxClips[(int)sfx]; // 선택한 SFX 클립 설정
                sfxPlayers[loopIndex].Play(); // SFX 재생
                break; // 루프 종료
            }
        }

        public float GetVolume(string parameterName)
        {
            if (!PlayerPrefs.HasKey(parameterName))
            {
                return 1f; // 기본 볼륨 값
            }
            float valueInDb = PlayerPrefs.GetFloat(parameterName);
            return Mathf.Pow(10f, valueInDb / 20.0f);
        }


        public void SetVolume(float value, string parameterName)
        {
            if (value <= 0)
                value = 0.001f;

            float valueInDb = Mathf.Log10(value) * 20;

            audioMixer.SetFloat(parameterName, valueInDb); // 오디오 믹서에 볼륨 설정

            PlayerPrefs.SetFloat(parameterName, valueInDb); // 플레이어 프레퍼스에 저장
        }
    }
}