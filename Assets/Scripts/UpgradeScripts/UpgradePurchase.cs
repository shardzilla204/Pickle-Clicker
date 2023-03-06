using PickleClicker.Data;
using PickleClicker.Poglin;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Upgrade
{
    public class UpgradePurchase : MonoBehaviour
    {
        public int id;

        [SerializeField] private Text nameText;
        [SerializeField] private Text costText;
        [SerializeField] private Text levelText;
        [SerializeField] private Button purchaseButton;
        [SerializeField] private UpgradeCategoryManager upgradeCategoryManager;

        public UpgradeData upgrade;

        [SerializeField] private PickleBombController pickleBombController;

        private double MULTIPLIER = 1.00242;
        private int LINEAR = 3;

        private void Update() 
        {
            levelText.text = $"{upgrade.amount}/{upgrade.maxAmount}\nLevels";
            costText.text = $"{upgrade.cost} Pickles";

            if (upgrade.maxAmount > upgrade.amount) return;

            levelText.text = "Max";
        }

        private void Start() 
        {
            upgradeCategoryManager = GameObject.FindObjectOfType<UpgradeCategoryManager>();
            pickleBombController = GameObject.FindObjectOfType<PickleBombController>();
        }

        public void BuyUpgradePickle() 
        {
            if (PlayerData.pickleData.picklesPicked < upgrade.cost || upgrade.maxAmount <= upgrade.amount) return;

            PlayerData.pickleData.picklesPicked -= upgrade.cost;
            PlayerData.pickleData.totalPicklesSpent += upgrade.cost;

            upgrade.cost = (ulong) Math.Floor((upgrade.cost + (ulong) LINEAR) * MULTIPLIER);
            costText.text = $"{upgrade.cost} Pickles";

            upgrade.amount++;
            PlayerData.pickleData.totalUpgradePicklesPicked++;
        }

        public void SetUpgradePickle(UpgradeData upgrade) 
        {
            this.upgrade = upgrade;

            if (upgrade == null) Debug.Log($"Pickle is null: {upgrade}");

            id = upgrade.id;
            nameText.text = upgrade.alias;
        }

        public void SetUpgradeText()
        {
            upgradeCategoryManager.SetUpgradeText(id);
        }
    }
}
