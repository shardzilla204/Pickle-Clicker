using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Animation
{  
    public class ShopAnimations : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Button autoShopButton;
        [SerializeField] private Button upgradeShopButton;

        public void SwtichShops()
        {
            animator.SetTrigger("Switch");
        }

        public void SwitchToUpgradeShop()
        {
            SwtichShops();
            autoShopButton.interactable = true;
            upgradeShopButton.interactable = false;
        }

        public void SwitchToAutoShop()
        {
            SwtichShops();
            autoShopButton.interactable = false;
            upgradeShopButton.interactable = true;
        }

        public void ExitShop()
        {
            animator.SetTrigger("Exit");
            autoShopButton.interactable = false;
            upgradeShopButton.interactable = true;
        }
    }
}