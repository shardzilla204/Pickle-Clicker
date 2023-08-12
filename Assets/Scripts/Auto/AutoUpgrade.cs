using PickleClicker.Data.Player;
using PickleClicker.Data.Auto;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Game.Auto
{
    public class AutoUpgrade : MonoBehaviour
    {
        [SerializeField] 
        private int id;
        
        [SerializeField] 
        private Text amountText; 

        [SerializeField] 
        private Text amountNeededText;

        [SerializeField] 
        private Text upgradeCostText;

        [SerializeField] 
        private Button upgradeButton;

        [SerializeField] 
        private Image borderButton;

        [SerializeField] 
        private Text levelText;

        private bool isPressed = false;

        public AutoData autoData;

        [SerializeField]
        private AutoPurchase autoPurchase;
        
        private void Start() 
        {
            UpdateText();
        }

        private void Update() 
        {
            CheckStatus();
        }

        public void CheckStatus()
        {
            borderButton.fillAmount -= 0.005f;

            if (!isPressed) return;
            
            borderButton.fillAmount += 0.005f;

            if (borderButton.fillAmount < 1) return;

            borderButton.fillAmount = 0;
            UpgradeAutoPickle();
        }

        public void OnPointerDown() 
        {
            if (upgradeButton.interactable) isPressed = true;
        }

        public void OnPointerUp() 
        {
            isPressed = false;
        }

        //Upgrades the auto pickle
        public void UpgradeAutoPickle()
        {
            if (PlayerData.pickleData.pickles < autoData.upgradeCost) return;
            if (autoData.currentAmount < autoData.amountRequiredForUpgrade) return; 
            if (borderButton.gameObject.activeSelf == false) return;

            PlayerData.pickleData.pickles -= autoData.upgradeCost;
            PlayerData.pickleData.totalPicklesSpent += autoData.upgradeCost;
            
            PlayerData.pickleData.totalAutoPicklesUpgraded++;
            autoData.currentAmount -= autoData.amountRequiredForUpgrade;

            double numerator = autoData.amountRequiredForUpgrade * 5;
            double denominator = autoData.amountRequiredForUpgrade;
            autoData.amountRequiredForUpgrade = (int) (numerator/denominator) + autoData.amountRequiredForUpgrade;

            autoData.upgradeCost = (ulong) Math.Round(autoData.upgradeCost * 2.42);
            autoData.upgradeLevel++;
            autoData.recieveMultiplier = (autoData.recieveMultiplier + 0.1) * autoData.upgradeLevel;

            autoPurchase.ToggleButtons();

            if (autoData.upgradeLevel >= 5) 
            {
                levelText.text = $"Max Level";
                levelText.fontSize = 30;
                upgradeCostText.gameObject.SetActive(false);
                amountNeededText.gameObject.SetActive(false);
                borderButton.gameObject.SetActive(false);
                isPressed = false;
            }
            else 
            {
                levelText.text = $"Level\n{autoData.upgradeLevel}";
                levelText.fontSize = 20;
                upgradeCostText.gameObject.SetActive(true);
                amountNeededText.gameObject.SetActive(true);
                borderButton.gameObject.SetActive(true);
            }

            UpdateText();
        }

        private void UpdateText()
        {
            amountText.text = $"{(autoData.currentAmount).ToString("N0")}/{autoData.maxAmount.ToString("N0")}\nPicked";
            upgradeCostText.text = $"{autoData.upgradeCost.ToString("N0")}\nPickles";
            amountNeededText.text = $"{autoData.amountRequiredForUpgrade.ToString("N0")}\nRequired";
            levelText.text = $"Level\n{autoData.upgradeLevel.ToString("N0")}";
        }
    }
}