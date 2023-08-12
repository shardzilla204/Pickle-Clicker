using PickleClicker.Data.Player;
using PickleClicker.Data.Auto;
using PickleClicker.Game.Auto;
using PickleClicker.Data.ScriptableObjects.Auto;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Controller.Auto
{  
    public class AutoController : MonoBehaviour
    {
        public AutoData autoData;
        public Text alias;
        public Text description;
        public Text purchaseCost;
        public Text amount;
        public Text recieve;
        public Text upgradeCost;
        public Text level;
        public Text amountRequired;

        public AutoPurchase autoPurchase;
        public AutoUpgrade autoUpgrade;

        [SerializeField]
        private AutoDatabase autoDatabase;

        private void Start() 
        {
            autoPurchase = GameObject.FindObjectOfType<AutoPurchase>();
            autoUpgrade = GameObject.FindObjectOfType<AutoUpgrade>();
        }

        public void SetDefaultBuyable() 
        {
            autoData = PlayerData.autoDataList.Find(auto => auto.id == 0);
            alias.text = $"Pickle {autoData.alias}";
            description.text = autoData.description;
            purchaseCost.text = $"{FormatString(autoData.purchaseCost)}\nPickles";
            amount.text = $"{FormatString(autoData.currentAmount)}/{FormatString(autoData.maxAmount)}\nPicked";
            recieve.text = $"+{FormatString(Math.Floor(autoData.recieve * autoData.recieveMultiplier))}\nPickles";
            upgradeCost.text = $"{FormatString(autoData.upgradeCost)}\nPickles";
            level.text = $"Level\n{FormatString(autoData.upgradeLevel)}";
            amountRequired.text = $"{FormatString(autoData.amountRequiredForUpgrade)}\nRequired";

            autoPurchase.CalculateTotal(AutoAmountType.One);
            autoPurchase.ToggleButtons();

            autoUpgrade.autoData = autoData;
        }

        public void SetBuyable(AutoScriptableObject autoScriptableObject) 
        {
            autoData = PlayerData.autoDataList.Find(auto => auto.id == autoScriptableObject.id);
            alias.text = $"Pickle {autoData.alias}";
            description.text = autoData.description;
            purchaseCost.text = $"{FormatString(autoData.purchaseCost)}\nPickles";
            amount.text = $"{FormatString(autoData.currentAmount)}/{FormatString(autoData.maxAmount)}\nPicked";
            recieve.text = $"+{FormatString(Math.Floor(autoData.recieve * autoData.recieveMultiplier))}\nPickles";
            upgradeCost.text = $"{FormatString(autoData.upgradeCost)}\nPickles";
            level.text = $"Level\n{FormatString(autoData.upgradeLevel)}";
            amountRequired.text = $"{FormatString(autoData.amountRequiredForUpgrade)}\nRequired";

            autoPurchase.CalculateTotal(AutoAmountType.One);
            autoPurchase.ToggleButtons();

            autoUpgrade.autoData = autoData;
        }

        private string FormatString(ulong valueToFormat)
        {
            return valueToFormat.ToString("N0");
        }

        private string FormatString(int valueToFormat)
        {
            return valueToFormat.ToString("N0");
        }

        private string FormatString(double valueToFormat)
        {
            return valueToFormat.ToString("N0");
        }
    }
}
