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
            Destroy(animator.gameObject, animatorStateInfo.length);
            pickleBombSpawner.RemoveBombCount(pickleBombSpawner);
        }
    }
}
