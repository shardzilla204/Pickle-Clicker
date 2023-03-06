using UnityEngine;

namespace PickleClicker.Pickle
{  
    public class BreakJar : MonoBehaviour
    {
        [SerializeField] private Animator animate;

        private void OnMouseOver() 
        {
            if (Input.GetMouseButtonDown(0))
            {
                BreakOpen();
            }
        }

        private void BreakOpen()
        {
            animate.SetTrigger("Break Open");
            animate.SetBool("Broken", true);
        }
    }
}
