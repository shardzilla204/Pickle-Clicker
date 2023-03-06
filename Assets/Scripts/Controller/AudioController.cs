using PickleClicker.Data;
using PickleClicker.CanvasScripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace PickleClicker.Controller
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private CanvasManager canvasManager;
        [SerializeField] private PlayerData gameManager;
        public AudioSource musicPlayer;
        public AudioClip[] music;
        public int currentMusic = 0;

        private void Awake()
        {
            float musicVolume = PlayerData.userData.musicVolume;
            float soundVolume = PlayerData.userData.soundVolume;
            audioMixer.SetFloat("Music", Mathf.Log10(musicVolume) * 20);
            audioMixer.SetFloat("Sound", Mathf.Log10(soundVolume) * 20);
        }

        private void Start() 
        {
            canvasManager = GameObject.FindObjectOfType<CanvasManager>();
            gameManager = GameObject.FindObjectOfType<PlayerData>();
        }

        private void Update()
        {
            if (!musicPlayer.isPlaying || musicPlayer.clip == null)
            {
                musicPlayer.clip = music[currentMusic];
                musicPlayer.Play();
                if (currentMusic < music.Length - 1)
                {
                    currentMusic += 1; 
                }
                else 
                {
                    currentMusic = 0; 
                }
            }
            musicSliderMain.value = PlayerData.userData.musicVolume;
            soundSliderMain.value = PlayerData.userData.soundVolume;
            musicSliderLogin.value = PlayerData.userData.musicVolume;
            soundSliderLogin.value = PlayerData.userData.soundVolume;
        }

        [Header("Audio Controls")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider musicSliderMain;
        [SerializeField] private Slider soundSliderMain;
        [SerializeField] private Slider musicSliderLogin;
        [SerializeField] private Slider soundSliderLogin;

        public void SetMusicVolumeMain()
        {
            PlayerData.userData.musicVolume = musicSliderMain.value;
            float musicVolume = PlayerData.userData.musicVolume;
            audioMixer.SetFloat("Music", Mathf.Log10(musicVolume) * 20);
            Debug.Log(audioMixer.GetFloat("Music", out musicVolume));
            gameManager.SaveUserData();
        }

        public void SetSoundVolumeMain()
        {
            PlayerData.userData.soundVolume = soundSliderMain.value;
            float soundVolume = PlayerData.userData.soundVolume;
            audioMixer.SetFloat("Sound", Mathf.Log10(soundVolume) * 20);
            gameManager.SaveUserData();
        }

        public void SetMusicVolumeLogin()
        {
            PlayerData.userData.musicVolume = musicSliderLogin.value;
            float musicVolume = PlayerData.userData.musicVolume;
            audioMixer.SetFloat("Music", Mathf.Log10(musicVolume) * 20);
            gameManager.SaveUserData();
        }

        public void SetSoundVolumeLogin()
        {
            PlayerData.userData.soundVolume = soundSliderLogin.value;
            float soundVolume = PlayerData.userData.soundVolume;
            audioMixer.SetFloat("Sound", Mathf.Log10(soundVolume) * 20);
            gameManager.SaveUserData();
        }
    }
}
