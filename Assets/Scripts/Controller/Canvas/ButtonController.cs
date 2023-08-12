using PickleClicker.Controller;
using PickleClicker.Manager;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.CanvasScripts
{
    public class ButtonController : MonoBehaviour
    {
        public CanvasType buttonType;
        private Button button;

        private void Start() {
            button = gameObject.GetComponent<Button>();

            if (!button) return;

            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            CanvasController.SetUpCanvas(buttonType);
            AddressablesManager addressablesManager = GameObject.FindAnyObjectByType<AddressablesManager>();
            addressablesManager.CheckCanvas();
        }
    }
}
