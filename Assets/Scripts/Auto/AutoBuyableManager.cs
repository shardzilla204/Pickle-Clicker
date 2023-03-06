using PickleClicker.Buyable;
using PickleClicker.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Auto
{  
    public class AutoBuyableManager : MonoBehaviour
    {
        public AutoData autoBuyable;
        public Text nameText;
        public Text descriptionText;
        public Text costText;
        public Text amountText;
        public Text recieveText;
        public Text upgradeCostText;
        public Text upgradeLevelText;
        public Text amountRequiredText;

        public AutoPurchase autoPurchase;
        public AutoUpgrade autoUpgrade;

        private void Awake() 
        {
            autoPurchase = GameObject.FindObjectOfType<AutoPurchase>();
            autoUpgrade = GameObject.FindObjectOfType<AutoUpgrade>();
        }

        private void Start() 
        {
            SetBuyable(0);
        }

        public void SetBuyable(int id) 
        {
            this.autoBuyable = PlayerData.autoList.autoBuyables.Find(autoBuyable => autoBuyable.id == id);
            this.nameText.text = $"Pickle {autoBuyable.alias}";
            this.descriptionText.text = autoBuyable.description;
            this.costText.text = $"{autoBuyable.purchaseCost.ToString("N0")}\nPickles";
            this.amountText.text = $"{(autoBuyable.amount).ToString("N0")}/{autoBuyable.maxAmount.ToString("N0")}\nPicked";
            this.recieveText.text = $"+{(Math.Floor(autoBuyable.recieve * autoBuyable.multiplier)).ToString("N0")}\nPickles";
            this.upgradeCostText.text = $"{autoBuyable.upgradeCost.ToString("N0")}\nPickles";
            this.upgradeLevelText.text = $"Level\n{autoBuyable.level.ToString("N0")}";
            this.amountRequiredText.text = $"{autoBuyable.amountRequired.ToString("N0")}\nRequired";

            autoPurchase.autoBuyable = this.autoBuyable;
            autoPurchase.CalculateTotal(BuyableType.One);
            autoPurchase.ShowHideButtons();

            autoUpgrade.autoBuyable = this.autoBuyable;
        }
    }
}
