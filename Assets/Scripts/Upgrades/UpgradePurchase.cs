using PickleClicker.Data.Player;
using PickleClicker.Data.Upgrade;
using PickleClicker.Game.Auto;
using PickleClicker.Game.Pickle;
using PickleClicker.Manager.Upgrade;
using PickleClicker.Controller.Pickle;
using PickleClicker.Controller.Upgrade;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Game.Upgrade
{
    public class UpgradePurchase : MonoBehaviour
    {
        public int id;
        public double totalCost;
        private ulong cost;
        private int amount;

        [SerializeField] private Text aliasText;
        [SerializeField] private Text costText;
        [SerializeField] private Text levelText;
        [SerializeField] private UpgradeCategoryManager upgradeCategoryManager;
        [SerializeField] private UpgradeCategoryController upgradeCategoryController;
        [SerializeField] private GameObject buttonContainer;
        [SerializeField] private List<AutoAmountButton> autoAmountButtons = new List<AutoAmountButton>(); 
        private bool notEnough = false;
        private AutoAmountType type;

        public UpgradeData upgradeBuyable;

        [SerializeField] private BombController bombController;

        private double MULTIPLIER = 1.025;
        private int LINEAR = 3;

        private void Update() 
        {
            levelText.text = $"{upgradeBuyable.amount}/{upgradeBuyable.maxAmount} Levels";

            if (upgradeBuyable.maxAmount > upgradeBuyable.amount) return;

            levelText.text = "Max";
            // Here delete the circle + button.
            if (!notEnough) return;

            if (PlayerData.pickleData.pickles >= this.totalCost)
            {
                costText.text = $"{this.totalCost.ToString("N0")}\nPickles";
                notEnough = !notEnough;
                return;
            }

            costText.text = $"Need {(this.totalCost - PlayerData.pickleData.pickles).ToString("N0")} More Pickles"; 
        }

        private void Start() 
        {
            upgradeCategoryManager = GameObject.FindObjectOfType<UpgradeCategoryManager>();
            bombController = GameObject.FindObjectOfType<BombController>();

            costText.text = $"{upgradeBuyable.cost.ToString("N0")} Pickles";
        }

        public void BuyUpgradePickle() 
        {
            if (PlayerData.pickleData.pickles < upgradeBuyable.cost || upgradeBuyable.maxAmount <= upgradeBuyable.amount) return;

            PlayerData.pickleData.pickles -= upgradeBuyable.cost;
            PlayerData.pickleData.totalPicklesSpent += upgradeBuyable.cost;

            upgradeBuyable.cost = (ulong) Math.Floor((upgradeBuyable.cost + (ulong) LINEAR) * MULTIPLIER);
            costText.text = $"{upgradeBuyable.cost.ToString("N0")} Pickles";

            upgradeBuyable.amount++;
            PlayerData.pickleData.totalUpgradePickles++;

            CheckForBombUpgrade();
        }

        private void CheckForBombUpgrade()
        {
            if (upgradeBuyable.categoryId != 3) return;

            List<Bomb> bombs = GameObject.FindObjectsOfType<Bomb>().ToList();

            if (upgradeBuyable.id == 2)
            {
                bombController.GainExtraBombSlot();
            } 
            else if (upgradeBuyable.id == 1)
            {
                foreach(Bomb bomb in bombs)
                {
                    bomb.SetRadius();
                }
            }
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

        public void CalculateTotal(AutoAmountType type)
        {
            if (!buttonContainer.activeSelf) return;

            double currentPickles = PlayerData.pickleData.pickles;
            double cost = upgradeBuyable.cost;
            int amount = 0;
            notEnough = false;
            this.type = type;

            if (type == AutoAmountType.One) amount = 1;
            if (type == AutoAmountType.Five) amount = 5;
            if (type == AutoAmountType.TwentyFive) amount = 25;
            if (type == AutoAmountType.Max) amount = upgradeBuyable.maxAmount - upgradeBuyable.amount;

            AutoAmountButton autoAmountButton = autoAmountButtons.Find(button => button.type == type);
            
            this.amount = amount;
            this.totalCost = GetPicklesLeft(autoAmountButton, amount);

            if (autoAmountButton.type == AutoAmountType.Max) this.totalCost -= cost;

            costText.text = $"{this.totalCost.ToString("N0")} Pickles";

            Debug.Log(totalCost);

            if (currentPickles < this.totalCost) notEnough = true;
        }

        private ulong GetPicklesLeft(AutoAmountButton button, int amount)
        {
            ulong cost = upgradeBuyable.cost;
            ulong accumulation = 0;

            if (type == AutoAmountType.Max) button.GetComponentInChildren<Text>().text = "Max";

            for (int index = 0; index < amount; index++)
            {
                accumulation += cost;
                cost = (ulong) Math.Floor((cost + (ulong) LINEAR) * MULTIPLIER);

                if (type == AutoAmountType.Max) button.GetComponentInChildren<Text>().text = $"x{amount}";
            }
            this.cost = cost;
            return accumulation;
        }

        public void SetUpgradePickle(UpgradeData upgradeBuyable) 
        {
            this.upgradeBuyable = upgradeBuyable;

            if (upgradeBuyable == null) Debug.Log($"Pickle is null: {upgradeBuyable}");

            id = upgradeBuyable.id;
            aliasText.text = upgradeBuyable.alias;
        }

        public void SetUpgradeText()
        {
            upgradeCategoryController.SetUpgradeText(id);
        }
    }
}
