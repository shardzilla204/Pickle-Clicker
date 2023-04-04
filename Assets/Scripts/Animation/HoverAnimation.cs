using UnityEngine;

namespace PickleClicker.Animation
{  
    public class HoverAnimation : MonoBehaviour
    {

        public void MouseIn()
        {
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(100, 125);
        }

        public void MouseOut()
        {
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(100, 100);
        }

        public void Clicked()
        {
            RectTransform rect = gameObject.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(100, 100);
        }
    }
}
