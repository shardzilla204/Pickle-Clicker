using UnityEngine;

namespace PickleClicker.Poglin
{
    public class CollectPoglin : StateMachineBehaviour
    {
        private NormalPoglin[] poglinScripts;

        private void Update() 
        {
            poglinScripts = GameObject.FindObjectsOfType<NormalPoglin>();
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
            Destroy(animator.gameObject, animatorStateInfo.length);
            
            NormalPoglin poglinScript = animator.gameObject.GetComponent<NormalPoglin>();
            poglinScript.CollectPoglin();
        }
    }
}
