using PickleClicker.Data.Player;
using PickleClicker.Data.Upgrade;
using PickleClicker.Game.Upgrade;
using PickleClicker.Manager.Upgrade;
using PickleClicker.Data.ScriptableObjects.Upgrade;

using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Controller.Upgrade
{
    public class UpgradeCategoryController : MonoBehaviour
    {
        [SerializeField] private UpgradeCategoryData upgradeCategory;

        [SerializeField] private GameObject upgradeBoard;
        [SerializeField] private Transform upgradeBuyablesSection;
        [SerializeField] private Text aliasText;
        [SerializeField] private Text descriptionText;

        public void SetDefaultCategory() 
        {
            UpgradeCategoryData upgradeCategoryData = PlayerData.upgradeCategoryDataList.Find(upgradeCategory => upgradeCategory.id == 0);
            aliasText.text = upgradeCategoryData.alias;
            descriptionText.text = upgradeCategoryData.description;

            ClearUpgrades();

            SetUpgrades();
        }

        public void SetCategory(UpgradeCategoryData upgradeCategoryData) 
        {
            aliasText.text = upgradeCategoryData.alias;
            descriptionText.text = upgradeCategoryData.description;

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
            for (int iteration = 0; iteration < upgradeCategory.upgrades.Count; iteration++) {
                UpgradeData upgrade = upgradeCategory.upgrades.Find(upgrade => upgrade.id == iteration);
                GameObject upgradeBuyableClone = Instantiate(upgradeBoard);
                upgradeBuyableClone.name = "UpgradeBuyable";
                upgradeBuyableClone.transform.SetParent(upgradeBuyablesSection);
                upgradeBuyableClone.transform.localScale = new Vector3(1f, 1f, 1f);
                upgradeBuyableClone.GetComponentInChildren<UpgradePurchase>().SetUpgradePickle(upgrade);
            }
        }

        public void SetUpgradeText(int id) 
        {
            UpgradeData upgrade = upgradeCategory.upgrades.Find(upgrade => upgrade.id == id);
            aliasText.text = upgrade.alias;
            descriptionText.text = upgrade.description;
        }
    }
}