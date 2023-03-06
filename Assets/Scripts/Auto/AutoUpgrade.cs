using PickleClicker.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Auto
{
    public class AutoUpgrade : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField] private Text amountText; 
        [SerializeField] private Text amountNeededText;
        [SerializeField] private Text upgradeCostText;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Image borderButton;
        [SerializeField] private Text levelText;

        private bool isPressed = false;

        public AutoData autoBuyable;
        public AutoPurchase autoPurchase;
        
        private void Start() 
        {
            UpdateText();
        }

        private void Update() 
        {
            if (autoBuyable.level >= 5) 
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
                levelText.text = $"Level\n{autoBuyable.level}";
                levelText.fontSize = 20;
                upgradeCostText.gameObject.SetActive(true);
                amountNeededText.gameObject.SetActive(true);
                borderButton.gameObject.SetActive(true);
            }

            CheckStatus();
        }

        public void CheckStatus()
        {
            if (isPressed)
            {
                borderButton.fillAmount += 0.005f;
                if (borderButton.fillAmount >= 1)
                {
                    borderButton.fillAmount = 0;
                    UpgradeAutoPickle();
                }
            }
            else if (!isPressed && borderButton.fillAmount >= 0)
            {
                borderButton.fillAmount -= 0.005f;
            }
        }

        public void OnPointerDown() 
        {
            if (upgradeButton.interactable)
            {
                isPressed = true;
            }
            else
            {
                isPressed = false;
            }
        }

        public void OnPointerUp() 
        {
            isPressed = false;
        }

        //Upgrades the auto pickle
        public void UpgradeAutoPickle()
        {
            if (PlayerData.pickleData.picklesPicked < autoBuyable.upgradeCost || 
                autoBuyable.amount < autoBuyable.amountRequired || 
                borderButton.gameObject.activeSelf == false) return;

            PlayerData.pickleData.picklesPicked -= autoBuyable.upgradeCost;
            PlayerData.pickleData.totalPicklesSpent += autoBuyable.upgradeCost;
            
            PlayerData.pickleData.totalAutoPicklesUpgraded += 1;
            autoBuyable.amount -= autoBuyable.amountRequired;

            double numerator = autoBuyable.amountRequired * 5;
            double denominator = autoBuyable.amountRequired;
            autoBuyable.amountRequired = (int) (numerator/denominator) + autoBuyable.amountRequired;

            autoBuyable.upgradeCost = (ulong) Math.Round(autoBuyable.upgradeCost * 2.42);
            autoBuyable.level += 1;
            autoBuyable.multiplier = (autoBuyable.multiplier + 0.1) * autoBuyable.level;

            autoPurchase.ShowHideButtons();
            UpdateText();
        }

        private void UpdateText()
        {
            amountText.text = $"{(autoBuyable.amount).ToString("N0")}/{autoBuyable.maxAmount.ToString("N0")}\nPicked";
            upgradeCostText.text = $"{autoBuyable.upgradeCost.ToString("N0")}\nPickles";
            amountNeededText.text = $"{autoBuyable.amountRequired.ToString("N0")}\nRequired";
            levelText.text = $"Level\n{autoBuyable.level.ToString("N0")}";
        }
    }
}