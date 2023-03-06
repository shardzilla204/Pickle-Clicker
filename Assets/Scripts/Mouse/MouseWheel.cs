using UnityEngine;
using System.Collections;
 
namespace PickleClicker 
{  
    public class MouseWheel : MonoBehaviour {
        float timer;
        float translation;
        float position;
        float target;
        float falloff;
        float input;
    
        // Increases the target translation
        public float speed = 100.0f;
        // Maximum acceleration possible by scrolling the wheel faster
        public float maxAcceleration = 2.0f;
        // How quickly to follow the target
        public float followSpeed = 10.0f;
        // Across which Vector to translate (Default is Y axis)
        public Vector3 translationVector = new Vector3(0, 1, 0);
        // Whether or not to use the scrollwheel acceleration
        public bool scrollWheelAcceleration = true;
    
        void Update () {
            timer+=Time.deltaTime;
            input=Input.GetAxis("Mouse ScrollWheel");
    
            // This is the acceleration according to the time difference between the "clicks" of the mousewheel
            // If you leave that out, it will be more like Opera scrolling (larger discrete steps but smooth follow)
            // The "300" could be adjusted (lower means larger steps, stronger acceleration)
            if (scrollWheelAcceleration)
            {
                if (input != 0)
                {
                    target += Mathf.Clamp((input / (timer * 300)) * speed, maxAcceleration * -1, maxAcceleration);
                    timer = 0;
                }
            }
            else // Opera-Style
            {
                target += Mathf.Clamp(input * speed, maxAcceleration * -1, maxAcceleration);
            }
            // As a falloff we use the distance between position and target
            // results in faster Movement at higher distances
            falloff = Mathf.Abs(position-target);
    
            // Determine the amount of translation for this frame
            translation = Time.deltaTime * falloff * followSpeed;
    
            // 0.001 is our deadzone
            if (position+0.001 < target)
            {
                this.GetComponent<Transform>().Translate(translationVector*translation*-1);
                position += translation;
            }
            if (position-0.001 > target)
            {
                this.GetComponent<Transform>().Translate(translationVector*translation);
                position -= translation;
            }
        }

        private void OnMouseOver() 
        {
            Debug.Log("Mouse is over");
            this.GetComponent<MouseWheel>().enabled = true;
        }

        private void OnMouseExit() 
        {
            this.GetComponent<MouseWheel>().enabled = false;
        }
    }
}