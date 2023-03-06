using PickleClicker.Pickle;
using PickleClicker.Data;
using PickleClicker.Upgrade;
using Steamworks;
using UnityEngine;

namespace PickleClicker 
{  
    public class SteamAchievements : MonoBehaviour
    {
        // MAKE SURE TO COMMENT OUT BEFORE PUBLISHING!!!
        // private void Start() 
        // {
        //     if (!SteamManager.Initialized) return;
        //     SteamUserStats.ResetAllStats(true);
        //     Debug.LogError("Reset All Stats!!!");
        // }

        private void Update()
        {

            if (!SteamManager.Initialized) return;

            if (PlayerData.pickleData.picklesPicked >= 50) SteamUserStats.SetAchievement("ACH_50");
            if (PlayerData.pickleData.picklesPicked >= 100) SteamUserStats.SetAchievement("ACH_100");

            long totalAutoAmount = 0;
            foreach (AutoData autoBuyable in PlayerData.autoList.autoBuyables)
            {
                totalAutoAmount += autoBuyable.amount;
            }

            if (totalAutoAmount >= 50) SteamUserStats.SetAchievement("ACH_AUTO_50");
            if (totalAutoAmount >= 100) SteamUserStats.SetAchievement("ACH_AUTO_100");
            if (totalAutoAmount >= 250) SteamUserStats.SetAchievement("ACH_AUTO_250");

            int totalUpgradeAmount = 0;
            foreach (UpgradeCategoryData upgradeCategory in PlayerData.upgradeList.upgradeCategories) 
            {
                foreach (UpgradeData upgrade in upgradeCategory.upgradeBuyables)
                {
                    totalUpgradeAmount += upgrade.amount;
                }
            }

            if (totalUpgradeAmount >= 50) SteamUserStats.SetAchievement("ACH_UPGRADE_50");
            if (totalUpgradeAmount >= 100) SteamUserStats.SetAchievement("ACH_UPGRADE_100");
            if (totalUpgradeAmount >= 250) SteamUserStats.SetAchievement("ACH_UPGRADE_250");
            
            if (PlayerData.pickleData.totalPoglinsSlayed >= 50) SteamUserStats.SetAchievement("ACH_KILL_50");
            if (PlayerData.pickleData.totalPoglinsSlayed >= 100) SteamUserStats.SetAchievement("ACH_KILL_100");
            if (PlayerData.pickleData.totalPoglinsSlayed >= 250) SteamUserStats.SetAchievement("ACH_KILL_250");

            if (PlayerData.pickleData.pickleLevel >= 50) SteamUserStats.SetAchievement("ACH_LEVEL_50");
            if (PlayerData.pickleData.pickleLevel >= 100) SteamUserStats.SetAchievement("ACH_LEVEL_100");
            if (PlayerData.pickleData.pickleLevel >= 250) SteamUserStats.SetAchievement("ACH_LEVEL_250");

            UpgradeCategoryData clickCategory = PlayerData.upgradeList.upgradeCategories.Find(category => category.id == 0);
            UpgradeData increaseClick = clickCategory.upgradeBuyables.Find(upgrade => upgrade.id == 0);

            if (increaseClick.amount >= 100)
            {
                SteamUserStats.SetAchievement("ACH_MAX_CLICKS");
            }

            SteamUserStats.StoreStats();
        }
    }
}