using PickleClicker.Data.ScriptableObjects.Upgrade;
using PickleClicker.Data.Player;
using PickleClicker.Manager;
using PickleClicker.Controller;
using PickleClicker.Data.Upgrade;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PickleClicker.Manager.Upgrade
{
    public class UpgradeCategoryManager: MonoBehaviour
    {
        private AsyncOperationHandle<IList<UpgradeCategoryScriptableObject>> upgradeCategoryOperationHandle;

        private AsyncOperationHandle<IList<UpgradeScriptableObject>> upgradeOperationHandle;

        private List<string> keys = new List<string>() { "upgrades" };

        private void Start()
        {
            SetUpgradeCategories();
        }

        public void SetUpgrades(AssetLabelReference upgradeAssetReference, UpgradeCategoryScriptableObject upgradeCategory)
        {
            upgradeOperationHandle = Addressables.LoadAssetsAsync<UpgradeScriptableObject>(upgradeAssetReference.ToString(), upgrade => 
            {
                List<UpgradeData> upgradeList = new List<UpgradeData>();
                upgradeList.Add(new UpgradeData(
                    upgradeCategory.id, 
                    upgrade.id, 
                    upgrade.alias, 
                    upgrade.description, 
                    upgrade.purchaseCost, 
                    upgrade.currentAmount, 
                    upgrade.maxAmount
                ));
        
                PlayerData.upgradeCategoryDataList.Add(new UpgradeCategoryData(
                    upgradeCategory.id, 
                    upgradeCategory.alias, 
                    upgradeCategory.description, 
                    upgradeList
                ));
                Debug.Log(PlayerData.upgradeCategoryDataList);
            }, Addressables.MergeMode.Union, true);


        }

        public void SetUpgradeCategories()
        {
            upgradeCategoryOperationHandle = Addressables.LoadAssetsAsync<UpgradeCategoryScriptableObject>(keys, upgradeCategory => 
            {
                Debug.Log("Setting Upgrade");
                SetUpgrades(upgradeCategory.upgradeAssetReference, upgradeCategory);
            }, Addressables.MergeMode.Union, true);
        }
    }
}