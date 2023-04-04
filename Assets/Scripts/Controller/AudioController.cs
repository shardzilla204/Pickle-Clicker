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
        private int CONSTANT = 20;
        private int ADDITION = 5;

        private void Awake()
        {
            float musicVolume = PlayerData.userData.musicVolume;
            float soundVolume = PlayerData.userData.soundVolume;

            Debug.Log(musicVolume);
            Debug.Log(soundVolume);
            Debug.Log(musicVolume.Equals(null) || soundVolume.Equals(null));

            if (!musicVolume.Equals(null) || !soundVolume.Equals(null))
            {
                audioMixer.SetFloat("Music", (Mathf.Log10(musicVolume) * CONSTANT) + ADDITION);
                audioMixer.SetFloat("Sound", (Mathf.Log10(soundVolume) * CONSTANT) + ADDITION);
            };
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
        }

        [Header("Audio Controls")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider musicSliderMain;
        [SerializeField] private Slider soundSliderMain;

        public void SetMusicVolumeMain()
        {
            PlayerData.userData.musicVolume = musicSliderMain.value;
            float musicVolume = PlayerData.userData.musicVolume;
            audioMixer.SetFloat("Music", (Mathf.Log10(musicVolume) * CONSTANT) + ADDITION);
            gameManager.SaveUserData();
        }

        public void SetSoundVolumeMain()
        {
            PlayerData.userData.soundVolume = soundSliderMain.value;
            float soundVolume = PlayerData.userData.soundVolume;
            audioMixer.SetFloat("Sound", (Mathf.Log10(soundVolume) * CONSTANT) + ADDITION);
            gameManager.SaveUserData();
        }
    }
}
