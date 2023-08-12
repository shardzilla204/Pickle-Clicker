using PickleClicker.Manager;
using PickleClicker.Controller.Auto;
using PickleClicker.Data.Auto;
using PickleClicker.Data.Player;
using PickleClicker.Data.ScriptableObjects.Auto;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Game.Auto
{
    public class AutoPurchase : MonoBehaviour
    {
        private double totalCost;
        private ulong cost;
        private int amountPurchasing;
        private bool notEnough = false;
        private AutoAmountType autoAmountType;

        private const double MULTIPLIER = 1.025;
        private const int LINEAR = 1;

        [SerializeField] 
        private AutoController autoController;

        [SerializeField] 
        private DescriptionManager descriptionManager;
        
        [SerializeField] 
        private List<AutoAmountButton> autoAmountButtons = new List<AutoAmountButton>(); 

        [SerializeField] 
        private GameObject buttonContainer;

        [SerializeField] 
        private Button upgradeButton;

        private void Awake()
        {
            autoAmountButtons = GameObject.FindObjectsOfType<AutoAmountButton>().ToList();
            descriptionManager = GameObject.FindObjectOfType<DescriptionManager>();
            autoController = GameObject.FindObjectOfType<AutoController>();
        }

        private void Update() 
        {
            if (!notEnough) return;

            if (PlayerData.pickleData.pickles >= totalCost)
            {
                autoController.purchaseCost.text = $"{totalCost.ToString("N0")}\nPickles";
                notEnough = !notEnough;
                return;
            }

            autoController.purchaseCost.text = $"Need {(totalCost - PlayerData.pickleData.pickles).ToString("N0")} More Pickles"; 
        }

        public void BuyAutoPickle()
        {
            if (PlayerData.pickleData.pickles < totalCost || autoController.autoData.maxAmount <= autoController.autoData.currentAmount) return;

            PlayerData.pickleData.pickles -= totalCost;
            PlayerData.pickleData.totalPicklesSpent += totalCost;
            autoController.autoData.purchaseCost = cost;

            autoController.autoData.currentAmount += amountPurchasing;
            autoController.amount.text = $"{(autoController.autoData.currentAmount).ToString("N0")}/{autoController.autoData.maxAmount.ToString("N0")}\nPicked";
            PlayerData.pickleData.totalAutoPickles += amountPurchasing;
            
            CalculateTotal(autoAmountType);
            ToggleButtons();
        }

        public void SelectOne()
        {
            CalculateTotal(AutoAmountType.One);
        }

        public void SelectFive()
        {
            CalculateTotal(AutoAmountType.Five);
        }

        public void SelectTwentyFive()
        {
            CalculateTotal(AutoAmountType.TwentyFive);
        }

        public void SelectMax()
        {
            CalculateTotal(AutoAmountType.Max);
        }
        
        public void CalculateTotal(AutoAmountType autoAmountType)
        {
            if (!buttonContainer.activeSelf) return;

            double currentPickles = PlayerData.pickleData.pickles;
            notEnough = false;
            this.autoAmountType = autoAmountType;

            int amountToPurchase = 0;
            if (autoAmountType.Equals(AutoAmountType.One)) amountToPurchase = 1;
            if (autoAmountType.Equals(AutoAmountType.Five)) amountToPurchase = 5;
            if (autoAmountType.Equals(AutoAmountType.TwentyFive)) amountToPurchase = 25;
            if (autoAmountType.Equals(AutoAmountType.Max)) amountToPurchase = GetRemainingAmount();

            AutoAmountButton autoAmountButton = autoAmountButtons.Find(button => button.type == autoAmountType);
            
            amountPurchasing = amountToPurchase;
            totalCost = GetPicklesLeft(autoAmountButton, amountPurchasing);

            if (autoAmountButton.type.Equals(AutoAmountType.Max)) totalCost -= cost;

            autoController.purchaseCost.text = $"{totalCost.ToString("N0")}\nPickles";

            if (currentPickles < this.totalCost) notEnough = true;
        }

        private int GetRemainingAmount()
        {
            return autoController.autoData.maxAmount - autoController.autoData.currentAmount;
        }

        private ulong GetPicklesLeft(AutoAmountButton autoAmountButton, int amount)
        {
            ulong cost = autoController.autoData.purchaseCost;

            if (autoAmountType.Equals(AutoAmountType.Max)) autoAmountButton.GetComponentInChildren<Text>().text = "Max";

            ulong accumulation = 0;
            for (int index = 0; index < amount; index++)
            {
                accumulation += cost;
                cost = (ulong) Math.Floor((cost + (ulong) LINEAR) * MULTIPLIER);

                if (autoAmountType.Equals(AutoAmountType.Max)) autoAmountButton.GetComponentInChildren<Text>().text = $"x{amount.ToString("N0")}";
            }
            this.cost = cost;
            return accumulation;
        }

        public void ToggleButtons()
        {
            foreach (AutoAmountButton autoAmountButton in autoAmountButtons)
            {
                int amountToPurchase = 0;

                autoAmountButton.gameObject.SetActive(false);

                if (autoAmountButton.type == AutoAmountType.One) amountToPurchase = 1;
                if (autoAmountButton.type == AutoAmountType.Five) amountToPurchase = 5;
                if (autoAmountButton.type == AutoAmountType.TwentyFive) amountToPurchase = 10;
                if (autoAmountButton.type == AutoAmountType.Max) amountToPurchase = GetRemainingAmount();
                
                if (autoController.autoData.maxAmount - autoController.autoData.currentAmount >= amountToPurchase) autoAmountButton.gameObject.SetActive(true);
            }

            autoController.recieve.text = $"+{(Math.Floor(autoController.autoData.recieve * autoController.autoData.recieveMultiplier)).ToString("N0")}\nPickles";

            if (autoController.autoData.currentAmount < autoController.autoData.maxAmount) return;

            CalculateTotal(AutoAmountType.One);
        }

        public void DisableUpgradeButton()
        {
            ulong upgradeCost = autoController.autoData.upgradeCost;
            int currentAmount = autoController.autoData.currentAmount;
            int amountRequiredForUpgrade = autoController.autoData.amountRequiredForUpgrade;
            upgradeButton.interactable = false;

            if (PlayerData.pickleData.pickles >= upgradeCost && currentAmount >= amountRequiredForUpgrade) upgradeButton.interactable = true;
        }
    }
}
