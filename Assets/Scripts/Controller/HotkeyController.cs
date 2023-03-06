using PickleClicker.CanvasScripts;
using PickleClicker.Animation;
using PickleClicker.Data;
using UnityEngine;

namespace PickleClicker.Controller
{  
    public class HotkeyController : MonoBehaviour
    {
        [SerializeField] private CanvasManager canvasManager;
        [SerializeField] private PlayerData gameManager;
        [SerializeField] private AudioController audioController;
        [SerializeField] private ShopAnimations shopAnimations;
        [SerializeField] private StatAnimations statAnimations;

        private void Start() 
        {
            canvasManager = GameObject.FindObjectOfType<CanvasManager>();
            gameManager = GameObject.FindObjectOfType<PlayerData>();
            audioController = GameObject.FindObjectOfType<AudioController>();
        }
        // Update is called once per frame
        void Update()
        {
            string canvasName = CanvasManager.lastActiveCanvas.gameObject.name;
            Debug.Log(canvasName);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (canvasName == "ShopsCanvas" || 
                    canvasName == "StatsCanvas" || 
                    canvasName == "OptionsCanvas" || 
                    canvasName == "ConfirmCanvas" || 
                    canvasName == "CustomizeCanvas" || 
                    canvasName == "IndexCanvas")
                {
                    canvasManager.SwitchCanvas(CanvasType.GameUI);
                    shopAnimations.ExitShop();
                }
                else if (canvasName == "MainCanvas")
                {
                    canvasManager.SwitchCanvas(CanvasType.Options);
                } 
                else if (canvasName == "LoginOptionsCanvas" || 
                    canvasName == "LoginSkipCanvas" || 
                    canvasName == "LoginConfirmCanvas")
                {
                    canvasManager.SwitchCanvas(CanvasType.Login);
                }
                else if (canvasName == "LoginMainCanvas")
                {
                    canvasManager.SwitchCanvas(CanvasType.LoginOptions);
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                if (audioController.currentMusic < audioController.music.Length - 1)
                {
                    audioController.currentMusic += 1; 
                }
                else 
                {
                    audioController.currentMusic = 0; 
                }
                audioController.musicPlayer.clip = audioController.music[audioController.currentMusic];
            }
            else if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                if (audioController.currentMusic > audioController.music.Length + 1)
                {
                    audioController.currentMusic -= 1; 
                }
                else 
                {
                    audioController.currentMusic = 0; 
                }
                audioController.musicPlayer.clip = audioController.music[audioController.currentMusic];
            }
        }
    }
}
