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

            if (button == null) return;

            button.onClick.AddListener(OnButtonClicked);
        }

        private void OnButtonClicked()
        {
            if (CanvasManager.lastActiveCanvas.gameObject.name != "LoginMainCanvas" || 
                (button.gameObject.name == "Settings" || 
                button.gameObject.name == "SkipLogin" || 
                button.gameObject.name == "BackButton")
            )
            {
                CanvasController.SetUpCanvas(buttonType);
            }
        }
    }
}
