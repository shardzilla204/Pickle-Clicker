using UnityEngine;

namespace PickleClicker.CanvasScripts
{
    public enum CanvasType
    {
        Login,
        LoginOptions,
        LoginConfirm,
        GameUI,
        Shop,
        UpgradeShop, 
        ItemShop,
        Inventory,
        Customize,
        Options,
        Stats,
        Confirm,
        Index,
        LoginSkip,
        StatsExtra,
        StatsMain
    }

    public class CanvasController : MonoBehaviour
    {
        public CanvasType desiredCanvasType;
        public static CanvasManager canvasManager;
        
        private void Start()
        {
            canvasManager = CanvasManager.canvasManager;
        }

        public static void SetUpCanvas(CanvasType buttonType) 
        {
            CanvasController.canvasManager.SwitchCanvas(buttonType);
        }

    }
}

