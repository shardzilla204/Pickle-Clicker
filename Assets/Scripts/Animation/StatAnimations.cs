using UnityEngine;

namespace PickleClicker.Animation
{  
    public class StatAnimations : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public void SetUpMain()
        {
            animator.SetBool("Switch", false);
        }

        public void SetUpExtra()
        {
            animator.SetBool("Switch", true);
        }
    }
}
