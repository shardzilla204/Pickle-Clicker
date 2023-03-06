using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PickleClicker.CanvasScripts
{
    public class CanvasManager : MonoBehaviour {

        List<CanvasController> canvasControllers;
        public static CanvasController lastActiveCanvas;
        public static CanvasController desiredCanvas;
        public static CanvasManager canvasManager;

        public GameObject pickleButton;

        [SerializeField] public CanvasController mainCanvas;
        [SerializeField] public CanvasController loginCanvas;

        [SerializeField] public CanvasController mainOptionsCanvas;
        [SerializeField] public CanvasController loginOptionsCanvas;

        public GameObject skipLoginButton;
        public GameObject backButton;

        private void Awake()
        {
            canvasManager = gameObject.GetComponent<CanvasManager>();
            canvasControllers = canvasManager.GetComponentsInChildren<CanvasController>().ToList();
            canvasControllers.ForEach(canvas => canvas.gameObject.SetActive(false));
            SwitchCanvas(CanvasType.GameUI);
            backButton.gameObject.SetActive(false);

            mainCanvas = canvasControllers.Find(canvas => canvas.desiredCanvasType == CanvasType.GameUI);
            loginCanvas = canvasControllers.Find(canvas => canvas.desiredCanvasType == CanvasType.Login);

            mainOptionsCanvas = canvasControllers.Find(canvas => canvas.desiredCanvasType == CanvasType.Options);
            loginOptionsCanvas = canvasControllers.Find(canvas => canvas.desiredCanvasType == CanvasType.LoginOptions);
        }

        public void SwitchCanvas(CanvasType canvasType)
        {
            // Debug.Log($"Last Active: {lastActiveCanvas}");

            // if (mainCanvas.gameObject.activeSelf && loginCanvas.gameObject.activeSelf && canvasType == CanvasType.Login)
            // {
            //     mainCanvas.gameObject.SetActive(false);
            //     mainOptionsCanvas.gameObject.SetActive(false);
            //     loginCanvas.gameObject.SetActive(true);
            //     return;
            // }

            if (lastActiveCanvas != null && lastActiveCanvas != mainCanvas)
            {
                if (lastActiveCanvas != mainCanvas)
                {
                    lastActiveCanvas.gameObject.SetActive(false);
                }
                
                if (lastActiveCanvas == loginCanvas)
                {
                    loginCanvas.gameObject.SetActive(true);
                }
            }
            
            desiredCanvas = canvasControllers.Find(canvas => canvas.desiredCanvasType == canvasType);

            // Debug.Log($"Desired Canvas: {desiredCanvas}");

            if (desiredCanvas != null)
            {
                desiredCanvas.gameObject.SetActive(true);
                lastActiveCanvas = desiredCanvas;
            } 
            else 
            {
                Debug.LogWarning("The desired canvas was not found");
            }

        }
    }
}
