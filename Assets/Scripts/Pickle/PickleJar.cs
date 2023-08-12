using PickleClicker.Data.Player;
using PickleClicker.Data.Upgrade;
using PickleClicker.Controller.Pickle;
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
            UpgradeCategoryData jarCategory = PlayerData.upgradeCategoryDataList.Find(category => category.id == 1);
            UpgradeData jarExperience = jarCategory.upgrades.Find(upgrade => upgrade.id == 1);
            UpgradeData jarCoins = jarCategory.upgrades.Find(upgrade => upgrade.id == 2);

            int generateExperience = Random.Range(25, 50);

            double pickleLevel = PlayerData.pickleData.level;

            if (!broken)
            {
                broken = !broken;
                breakSound.Play();
                pickleJar.SetTrigger("Break");
                if (jarExperience.amount == 0)
                {
                    PlayerData.pickleData.pickles += (ulong) (generateExperience * pickleLevel);
                    PlayerData.pickleData.currentProgress += (int) Mathf.Round(generateExperience);
                }
                else 
                {
                    PlayerData.pickleData.pickles += (ulong) (generateExperience * pickleLevel * jarExperience.amount);
                    PlayerData.pickleData.currentProgress += (int) Mathf.Round(generateExperience * jarExperience.amount);
                }
                PlayerData.buyableData.pickleCoins += (5 + jarCoins.amount);
                if (PlayerData.buyableData.pickleCoins > PickleController.totalCoinsCollected) PickleController.GetHighestAmountOfCoins();
            }
        }

        public void OnMouseDown() 
        {  
            PlayerData.pickleData.jarsBroken++;
            BreakPickleJar();
        }
    }
}