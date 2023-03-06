using PickleClicker.Data;
using UnityEngine;

namespace PickleClicker.Pickle
{
    public class PickleJar : MonoBehaviour
    {
        [SerializeField] private Animator pickleJar; 
        [SerializeField] private AudioSource breakSound;
        private bool broken = false; 
        
        public void BreakPickleJar()
        {
            UpgradeCategoryData jarCategory = PlayerData.upgradeList.upgradeCategories.Find(category => category.id == 1);
            UpgradeData jarExperience = jarCategory.upgradeBuyables.Find(upgrade => upgrade.id == 1);
            UpgradeData jarCoins = jarCategory.upgradeBuyables.Find(upgrade => upgrade.id == 2);

            int generateExperience = Random.Range(25, 50);

            int pickleLevel = PlayerData.pickleData.pickleLevel;

            if (!broken)
            {
                broken = true;
                breakSound.Play();
                pickleJar.SetTrigger("Break");
                if (jarExperience.amount == 0)
                {
                    PlayerData.pickleData.picklesPicked += (ulong) (generateExperience * pickleLevel);
                    PlayerData.pickleData.currentPickleProgress += (int) Mathf.Round(generateExperience);
                }
                else 
                {
                    PlayerData.pickleData.picklesPicked += (ulong) (generateExperience * pickleLevel * jarExperience.amount);
                    PlayerData.pickleData.currentPickleProgress += (int) Mathf.Round(generateExperience * jarExperience.amount);
                }
                PlayerData.buyableData.pickleCoins += (5 + jarCoins.amount);
                PickleController.GetHighestAmountOfCoins();
            }
        }

        public void OnMouseDown() 
        {  
            PlayerData.pickleData.pickleJarsBroken++;
            BreakPickleJar();
        }
    }
}