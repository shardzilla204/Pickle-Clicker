using PickleClicker.Game.Pickle;
using PickleClicker.Controller.Pickle;
using UnityEngine;

namespace PickleClicker.Animation
{  
    public class UseBomb : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) 
        {
            BombController pickleBombSpawner = GameObject.FindObjectOfType<BombController>();
            Bomb pickleBomb = animator.gameObject.GetComponent<Bomb>();
            pickleBomb.SetUpCountdown();
            Destroy(animator.gameObject, animatorStateInfo.length);
            pickleBombSpawner.RemoveBombCount(pickleBombSpawner);
        }
    }
}
