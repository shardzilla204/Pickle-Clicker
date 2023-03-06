using PickleClicker.Data;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Tracker
{  
    public class StatsTracker : MonoBehaviour
    {
        [SerializeField] private Text totalPickles;
        [SerializeField] private Text totalCoins;
        [SerializeField] private Text totalClicks;
        [SerializeField] private Text totalPoglins;
        [SerializeField] private Text totalSpent;
        [SerializeField] private Text autoPicklesPicked;
        [SerializeField] private Text autoPicklesUpgrade;
        [SerializeField] private Text upgradePicklesPicked;
        [SerializeField] private Text pickleJarsBroken;

        private void Update() 
        {
            totalPickles.text = $"Highest Pickles Picked:\n{PlayerData.pickleData.totalPicklesPicked.ToString("N0")}";
            totalCoins.text = $"Highest Pickle Coins Collected:\n{PlayerData.pickleData.totalCoinsCollected.ToString("N0")}";
            totalClicks.text = $"Pickle Clicks:\n{PlayerData.pickleData.totalClicks.ToString("N0")}";
            totalPoglins.text = $"Poglins Slayed:\n{PlayerData.pickleData.totalPoglinsSlayed.ToString("N0")}";
            totalSpent.text = $"Pickles Spent:\n{PlayerData.pickleData.totalPicklesSpent.ToString("N0")}";
            autoPicklesPicked.text = $"Auto Pickles Picked:\n{PlayerData.pickleData.totalAutoPicklesPicked.ToString("N0")}";
            autoPicklesUpgrade.text = $"Auto Pickles Upgraded:\n{PlayerData.pickleData.totalAutoPicklesUpgraded.ToString("N0")}";
            upgradePicklesPicked.text = $"Upgrade Pickles Picked:\n{PlayerData.pickleData.totalUpgradePicklesPicked.ToString("N0")}";
            pickleJarsBroken.text = $"Pickle Jars Broken:\n{PlayerData.pickleData.pickleJarsBroken.ToString("N0")}";
        }
    }
}
