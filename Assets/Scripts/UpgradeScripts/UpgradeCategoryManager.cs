using PickleClicker.Data;
using PickleClicker.Pickle;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Upgrade
{
    public class UpgradeCategoryManager : MonoBehaviour
    {
        public UpgradeCategoryData upgradeCategory;

        [SerializeField] private GameObject upgradeBoard;
        [SerializeField] private Transform upgradeBuyablesSection;
        [SerializeField] private Text nameText;
        [SerializeField] private Text descriptionText;

        private void Start() 
        {
            SetCategory(0);
        }

        public void SetCategory(int id) 
        {
            upgradeCategory = PlayerData.upgradeList.upgradeCategories.Find(category => category.id == id);

            nameText.text = upgradeCategory.alias;
            descriptionText.text = upgradeCategory.description;

            ClearUpgrades();

            SetUpgrades();
        }

        public void ClearUpgrades() 
        {
            foreach (Transform child in upgradeBuyablesSection.transform)
            {
                Destroy(child.gameObject);
            }
        }

        public void SetUpgrades() 
        {
            for (int iteration = 0; iteration < upgradeCategory.upgradeBuyables.Count; iteration++) {
                // Debug.Log($"Iteration: {iteration}");
                UpgradeData upgrade = upgradeCategory.upgradeBuyables.Find(upgrade => upgrade.id == iteration);
                GameObject upgradeBuyableClone = Instantiate(upgradeBoard);
                upgradeBuyableClone.name = "UpgradeBuyable";
                upgradeBuyableClone.transform.SetParent(upgradeBuyablesSection);
                upgradeBuyableClone.transform.localScale = new Vector3(1f, 1f, 1f);
                upgradeBuyableClone.GetComponentInChildren<UpgradePurchase>().SetUpgradePickle(upgrade);
            }
        }

        public void SetUpgradeText(int id) 
        {
            UpgradeData upgrade = upgradeCategory.upgradeBuyables.Find(upgrade => upgrade.id == id);
            this.nameText.text = upgrade.alias;
            this.descriptionText.text = upgrade.description;
        }
    }
}