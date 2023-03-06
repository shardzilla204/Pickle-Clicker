using UnityEngine;

namespace PickleClicker.Animation
{  
    public class HoverAnimation : MonoBehaviour
    {

        public void MouseIn()
        {
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(125, 165);
        }

        public void MouseOut()
        {
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(125, 125);
        }

        public void Clicked()
        {
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(125, 125);
        }
    }
}
