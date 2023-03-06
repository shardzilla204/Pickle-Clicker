using PickleClicker.CanvasScripts;
using UnityEngine;

namespace PickleClicker.Pickle
{
    public class PickleButton : MonoBehaviour
    {
        PickleController pickleController;
        PickleObjectController pickleObjectController;

        private void Awake()
        {
            pickleController = GameObject.FindObjectOfType<PickleController>();
            pickleObjectController = GameObject.FindObjectOfType<PickleObjectController>();
        }
        
        private void OnMouseDown() 
        {
            if (CanvasManager.lastActiveCanvas.gameObject.name != "MainCanvas") return;

            pickleController.PickleClick();
            pickleController.EmitValue();
            pickleObjectController.CreatePickle();
        }
    }
}
