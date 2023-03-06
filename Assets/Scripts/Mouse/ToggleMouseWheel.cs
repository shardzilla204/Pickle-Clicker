using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PickleClicker 
{  
    public class ToggleMouseWheel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public MouseWheel mouseWheel;

        public void OnPointerEnter(PointerEventData eventData)
        {
            mouseWheel.enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            mouseWheel.enabled = false;
        }
    }
}
