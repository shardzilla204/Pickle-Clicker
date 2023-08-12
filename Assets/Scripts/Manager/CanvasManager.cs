using PickleClicker.Controller;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PickleClicker.Manager
{
    public class CanvasManager : MonoBehaviour {

        List<CanvasController> canvasControllers;
        public static CanvasController lastActiveCanvas;
        public static CanvasController desiredCanvas;
        public static CanvasManager canvasManager;

        public GameObject pickleButton;

        [SerializeField] private CanvasController gameCanvas;

        [SerializeField] private CanvasController settingsCanvas;

        [SerializeField] private GameObject backButton;

        private void Awake()
        {
            canvasManager = gameObject.GetComponent<CanvasManager>();
            canvasControllers = canvasManager.GetComponentsInChildren<CanvasController>().ToList();
            canvasControllers.ForEach(canvas => canvas.gameObject.SetActive(false));
            SwitchCanvas(CanvasType.Game);
            backButton.gameObject.SetActive(false);

            gameCanvas = canvasControllers.Find(canvas => canvas.desiredCanvasType == CanvasType.Game);

            settingsCanvas = canvasControllers.Find(canvas => canvas.desiredCanvasType == CanvasType.Settings);
        }

        public void SwitchCanvas(CanvasType canvasType)
        {
            // Debug.Log($"Last Active: {lastActiveCanvas}");

            if (lastActiveCanvas != null) lastActiveCanvas.gameObject.SetActive(false);
            
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
