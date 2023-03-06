using PickleClicker.Pickle;
using UnityEngine;

namespace PickleClicker.Poglin
{  
    public class PickleBombSpot : MonoBehaviour
    {
        [SerializeField] private PickleButton pickleButton;
        private float speed = 15f;

        private void Start() 
        {
            pickleButton = GameObject.FindObjectOfType<PickleButton>();
        }

        private void Update() 
        {
            transform.RotateAround(pickleButton.transform.position, Vector3.forward, speed * Time.deltaTime);
            transform.Rotate(Vector3.back, speed * Time.deltaTime);
        }
    }
}
