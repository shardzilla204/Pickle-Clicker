using PickleClicker.Manager;
using UnityEngine;

namespace PickleClicker.Controller
{
    public enum CanvasType
    {
        Game,
        AutoShop,
        UpgradeShop, 
        ItemShop,
        Inventory,
        Customize,
        Settings,
        Statistics,
        Confirm,
        Index,
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

