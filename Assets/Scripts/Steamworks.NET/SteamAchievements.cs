using PickleClicker.Data.Player;
using PickleClicker.Data.Auto;
using PickleClicker.Data.Upgrade;
using Steamworks;
using UnityEngine;

namespace PickleClicker.Game.Achievements
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

            if (PlayerData.pickleData.pickles >= 50) SteamUserStats.SetAchievement("ACH_50");
            if (PlayerData.pickleData.pickles >= 100) SteamUserStats.SetAchievement("ACH_100");

            long totalAutoAmount = 0;
            foreach (AutoData auto in PlayerData.autoDataList)
            {
                totalAutoAmount += auto.currentAmount;
            }

            if (totalAutoAmount >= 50) SteamUserStats.SetAchievement("ACH_AUTO_50");
            if (totalAutoAmount >= 100) SteamUserStats.SetAchievement("ACH_AUTO_100");
            if (totalAutoAmount >= 250) SteamUserStats.SetAchievement("ACH_AUTO_250");

            int totalUpgradeAmount = 0;
            foreach (UpgradeCategoryData upgradeCategory in PlayerData.upgradeCategoryDataList) 
            {
                foreach (UpgradeData upgrade in upgradeCategory.upgrades)
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

            if (PlayerData.pickleData.level >= 50) SteamUserStats.SetAchievement("ACH_LEVEL_50");
            if (PlayerData.pickleData.level >= 100) SteamUserStats.SetAchievement("ACH_LEVEL_100");
            if (PlayerData.pickleData.level >= 250) SteamUserStats.SetAchievement("ACH_LEVEL_250");

            UpgradeCategoryData clickCategory = PlayerData.upgradeCategoryDataList.Find(category => category.id == 0);
            UpgradeData increaseClick = clickCategory.upgrades.Find(upgrade => upgrade.id == 0);

            if (increaseClick.amount >= 100)
            {
                SteamUserStats.SetAchievement("ACH_MAX_CLICKS");
            }

            SteamUserStats.StoreStats();
        }
    }
}