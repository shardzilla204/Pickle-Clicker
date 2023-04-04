using PickleClicker.Upgrade;
using PickleClicker.Data;
using System.Collections.Generic;
using UnityEngine;

namespace PickleClicker.Controller
{
    public class UpgradeCategoryController : MonoBehaviour
    {
        [SerializeField] private List<UpgradeCategoryScriptableObject> upgradeCategories; 

        private void Awake() 
        {
            SetUpgradeCategories();
        }

        public void SetUpgradeCategories()
        {
            foreach (UpgradeCategoryScriptableObject upgradeCategory in upgradeCategories)
            {
                int categoryId = upgradeCategory.id;
                string categoryAlias = upgradeCategory.alias;
                string categoryDescription = upgradeCategory.description;
                List<UpgradeData> upgradeBuyables = new List<UpgradeData>();
                List<UpgradeData> upgrades = new List<UpgradeData>();

                foreach(UpgradePickleScriptableObject upgradePickle in upgradeCategory.upgradePickleScriptableObjects)
                {
                    int id = upgradePickle.id;
                    string alias = upgradePickle.alias;
                    string description = upgradePickle.description;
                    ulong cost = upgradePickle.cost;
                    int amount = upgradePickle.amount;
                    int maxAmount = upgradePickle.maxAmount;

                    upgradeBuyables.Add(new UpgradeData(categoryId, id, alias, description, cost, amount, maxAmount));

                    upgrades.Add(new UpgradeData(categoryId, id, alias, description, cost, amount, maxAmount));
                }

                PlayerData.upgradeList.upgradeCategories.Add(new UpgradeCategoryData(categoryId, categoryAlias, categoryDescription, upgradeBuyables, upgrades));
            }
        }
    }
}