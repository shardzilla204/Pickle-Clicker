using PickleClicker.Data;
using PickleClicker.Poglin;
using UnityEngine;

namespace PickleClicker.Animation
{  
    public class UseBomb : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) 
        {
            PickleBombSpawner pickleBombSpawner = GameObject.FindObjectOfType<PickleBombSpawner>();
            PickleBomb pickleBomb = animator.gameObject.GetComponent<PickleBomb>();
            pickleBomb.SetUpCountdown();
            Destroy(animator.gameObject, animatorStateInfo.length);
            pickleBombSpawner.RemoveBombCount(pickleBombSpawner);
        }
    }
}
