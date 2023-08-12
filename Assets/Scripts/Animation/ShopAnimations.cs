using PickleClicker.Controller.Auto;
using PickleClicker.Controller.Upgrade;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Animation
{  
    public class ShopAnimations : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        public AutoController autoShop;
        public Button autoShopButton;
        public UpgradeCategoryController upgradeShop;
        public Button upgradeShopButton;

        public void Start()
        {
            autoShopButton.interactable = false;
            upgradeShopButton.interactable = true;
        }

        public void SwtichShops()
        {
            autoShopButton.interactable = !autoShopButton.interactable;
            upgradeShopButton.interactable = !upgradeShopButton.interactable;
            if (autoShop.gameObject.activeInHierarchy)
            {
                StartCoroutine(WaitForAnimation(autoShop.gameObject));
            }
            else 
            {
                autoShop.gameObject.SetActive(!autoShop.gameObject.activeInHierarchy);
            }

            if (upgradeShop.gameObject.activeInHierarchy) 
            {
                StartCoroutine(WaitForAnimation(upgradeShop.gameObject));
            }
            else 
            {
                upgradeShop.gameObject.SetActive(!upgradeShop.gameObject.activeInHierarchy);
            }
            animator.SetTrigger("Switch");
        }

        IEnumerator WaitForAnimation(GameObject shop)
        {
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        }

        public void ExitShop()
        {
            animator.SetTrigger("Exit");
            autoShopButton.interactable = false;
            upgradeShopButton.interactable = true;
        }
    }
}