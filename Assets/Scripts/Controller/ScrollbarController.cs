using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Controller
{
    public class ScrollbarController : MonoBehaviour
    {
        public Scrollbar bar;

        private void Start() 
        {
            bar.value = 1f;
        }
    }
}