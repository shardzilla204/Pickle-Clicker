using PickleClicker.Animation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        ShopAnimations shopAnimations = GameObject.FindAnyObjectByType<ShopAnimations>();
        if (animatorStateInfo.IsName("UpgradeSetUp"))
        {
            shopAnimations.autoShop.gameObject.SetActive(false);
            shopAnimations.upgradeShop.gameObject.SetActive(true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        ShopAnimations shopAnimations = GameObject.FindAnyObjectByType<ShopAnimations>();
        if (animatorStateInfo.IsName("UpgradeSetUp"))
        {
            shopAnimations.autoShop.gameObject.SetActive(true);
            shopAnimations.upgradeShop.gameObject.SetActive(false);
        }
    }
}
