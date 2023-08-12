using PickleClicker.Data.Player;
using PickleClicker.CanvasScripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace PickleClicker.Manager
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] 
        private CanvasManager canvasManager;

        [SerializeField] 
        private PlayerData gameManager;

        [SerializeField] 
        private AudioSource musicPlayer;

        [SerializeField] 
        private AudioClip[] music;

        public int currentMusic = 0;
        private const int CONSTANT = 20;
        private const int ADDITION = 5;

        private void Awake()
        {
            float musicVolume = PlayerData.localData.musicVolume;
            float soundVolume = PlayerData.localData.soundVolume;

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
            musicSlider.value = PlayerData.localData.musicVolume;
            soundSlider.value = PlayerData.localData.soundVolume;
        }

        [Header("Audio Controls")]
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundSlider;

        public void SetMusicVolumeMain()
        {
            PlayerData.localData.musicVolume = musicSlider.value;
            float musicVolume = PlayerData.localData.musicVolume;
            audioMixer.SetFloat("Music", (Mathf.Log10(musicVolume) * CONSTANT) + ADDITION);
            gameManager.SaveUserData();
        }

        public void SetSoundVolumeMain()
        {
            PlayerData.localData.soundVolume = soundSlider.value;
            float soundVolume = PlayerData.localData.soundVolume;
            audioMixer.SetFloat("Sound", (Mathf.Log10(soundVolume) * CONSTANT) + ADDITION);
            gameManager.SaveUserData();
        }
    }
}
