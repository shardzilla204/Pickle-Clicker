using UnityEngine;

namespace PickleClicker.Controller
{
    public class DisplayController : MonoBehaviour
    {
        public void WindowMode()
        {
            Screen.fullScreen = false;
        }

        public void FullscreenMode()
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            Screen.fullScreen = true;
        }
    }
}
