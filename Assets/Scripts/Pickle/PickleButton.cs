using PickleClicker.Manager;
using PickleClicker.Manager.Pickle;
using PickleClicker.Controller.Pickle;
using UnityEngine;

namespace PickleClicker.Pickle
{
    public class PickleButton : MonoBehaviour
    {
        [SerializeField]
        private PickleController pickleController;

        [SerializeField]
        private PickleObjectManager pickleObjectManager;

        private void Start()
        {
            pickleController = GameObject.FindObjectOfType<PickleController>();
            pickleObjectManager = GameObject.FindObjectOfType<PickleObjectManager>();
        }
        
        private void OnMouseDown() 
        {
            if (CanvasManager.lastActiveCanvas.gameObject.name != "MainCanvas") return;

            pickleController.PickleClick();
            pickleController.EmitValue();
            pickleObjectManager.CreatePickle();
        }
    }
}
