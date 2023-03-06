using PickleClicker.Controller;
using PickleClicker.Buyable;
using PickleClicker.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Auto
{
    public class AutoPurchase : MonoBehaviour
    {
        public ulong totalCost;
        private ulong cost;
        private int amount;
        private bool notEnough = false;
        private BuyableType type;

        [SerializeField] private Text amountText;
        [SerializeField] private Text costText;
        [SerializeField] private Text recieveText;
        [SerializeField] private Button purchaseButton;
        [SerializeField] private Button upgradeButton;

        private double MULTIPLIER = 1.02;
        private int LINEAR = 1;

        public AutoData autoBuyable;
        public AutoBuyableManager autoBuyableManager;

        [SerializeField] private DescriptionController descriptionController;

        [SerializeField] private List<BuyableButton> buttons = new List<BuyableButton>(); 
        [SerializeField] private GameObject buttonContainer;

        private void Start()
        {
            buttons = GameObject.FindObjectsOfType<BuyableButton>().ToList();
            descriptionController = GameObject.FindObjectOfType<DescriptionController>();
            autoBuyableManager = GameObject.FindObjectOfType<AutoBuyableManager>();

            autoBuyable = descriptionController.autoBuyableManager.autoBuyable;
            amountText = descriptionController.autoBuyableManager.amountText;
            costText = descriptionController.autoBuyableManager.costText;
            recieveText = descriptionController.autoBuyableManager.recieveText;
        }

        private void Update() 
        {
            if (!notEnough) return;

            if (PlayerData.pickleData.picklesPicked >= this.totalCost)
            {
                costText.text = $"{this.totalCost.ToString("N0")}\nPickles";
                notEnough = !notEnough;
                return;
            }

            costText.text = $"Not Enough\nPickles\n\nNeed {this.totalCost - PlayerData.pickleData.picklesPicked} More Pickles"; 
        }

        public void BuyAutoPickle()
        {
            if (PlayerData.pickleData.picklesPicked < this.totalCost || autoBuyable.maxAmount <= autoBuyable.amount) return;

            PlayerData.pickleData.picklesPicked -= this.totalCost;
            PlayerData.pickleData.totalPicklesSpent += this.totalCost;
            autoBuyable.purchaseCost = this.cost;

            autoBuyable.amount += amount;
            amountText.text = $"{(autoBuyable.amount).ToString("N0")}/{autoBuyable.maxAmount.ToString("N0")}\nPicked";
            PlayerData.pickleData.totalAutoPicklesPicked += amount;
            
            CalculateTotal(this.type);
            ShowHideButtons();
        }

        public void SelectOne()
        {
            CalculateTotal(BuyableType.One);
        }

        public void SelectFive()
        {
            CalculateTotal(BuyableType.Five);
        }

        public void SelectTen()
        {
            CalculateTotal(BuyableType.Ten);
        }

        public void SelectMax()
        {
            CalculateTotal(BuyableType.Max);
        }
        
        public void CalculateTotal(BuyableType type)
        {
            if (!buttonContainer.activeSelf) return;

            ulong currentPickles = PlayerData.pickleData.picklesPicked;
            ulong cost = autoBuyable.purchaseCost;
            int amount = 0;
            notEnough = false;
            this.type = type;

            if (type == BuyableType.One) amount = 1;
            if (type == BuyableType.Five) amount = 5;
            if (type == BuyableType.Ten) amount = 10;
            if (type == BuyableType.Max) amount = autoBuyable.maxAmount - autoBuyable.amount;

            BuyableButton button = buttons.Find(button => button.type == type);
            
            this.amount = amount;
            this.totalCost = GetPicklesLeft(button, amount);

            if (button.type == BuyableType.Max) this.totalCost -= cost;

            costText.text = $"{this.totalCost.ToString("N0")}\nPickles";

            if (currentPickles < this.totalCost) notEnough = true;
        }

        private ulong GetPicklesLeft(BuyableButton button, int amount)
        {
            ulong cost = autoBuyable.purchaseCost;
            ulong accumulation = 0;

            if (type == BuyableType.Max) button.GetComponentInChildren<Text>().text = "Max";

            for (int index = 0; index < amount; index++)
            {
                accumulation += cost;
                cost = (ulong) Math.Floor((cost + (ulong) LINEAR) * MULTIPLIER);

                if (type == BuyableType.Max) button.GetComponentInChildren<Text>().text = $"x{amount}";
            }
            this.cost = cost;
            return accumulation;
        }

        public void ShowHideButtons()
        {
            foreach (BuyableButton button in buttons)
            {
                int amount = 0;

                button.gameObject.SetActive(false);

                if (button.type == BuyableType.One) amount = 1;
                if (button.type == BuyableType.Five) amount = 5;
                if (button.type == BuyableType.Ten) amount = 10;
                if (button.type == BuyableType.Max) amount = autoBuyable.maxAmount - autoBuyable.amount;
                
                if (autoBuyable.maxAmount - autoBuyable.amount >= amount) button.gameObject.SetActive(true);
            }

            recieveText.text = $"+{(Math.Floor(autoBuyable.recieve * autoBuyable.multiplier)).ToString("N0")}\nPickles";
            amountText.fontSize = 20;
            recieveText.fontSize = 20;
            costText.gameObject.SetActive(true);
            buttonContainer.gameObject.SetActive(true);

            if (autoBuyable.amount < autoBuyable.maxAmount) return;

            CalculateTotal(BuyableType.One);
            amountText.fontSize = 25;
            amountText.fontSize = 25;
            costText.gameObject.SetActive(false);
            buttonContainer.gameObject.SetActive(false);
        }


        public void DisablePurchaseButton()
        {
            ulong purchaseCost = autoBuyable.purchaseCost;
            ulong upgradeCost = autoBuyable.upgradeCost;

            purchaseButton.interactable = false;
            upgradeButton.interactable = false;

            if (PlayerData.pickleData.picklesPicked >= purchaseCost) purchaseButton.interactable = true;

            if (PlayerData.pickleData.picklesPicked >= upgradeCost && autoBuyable.amount >= autoBuyable.amountRequired) upgradeButton.interactable = true;
        }
    }
}
