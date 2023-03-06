using PickleClicker.Auto;
using PickleClicker.Data;
using System.Collections.Generic;
using UnityEngine;

namespace PickleClicker.Controller
{
    public class AutoBuyableController : MonoBehaviour
    {
        [SerializeField] private List<AutoPickleScriptableObject> autoPickleScriptableObjects;

        // Adds each auto pickle to the list 
        private void Awake() 
        {
            SetAutoBuyables();
        }

        public void SetAutoBuyables()
        {
            int currentAmountRequired = 0;
            foreach (AutoPickleScriptableObject autoPickle in autoPickleScriptableObjects)
            {
                int id = autoPickle.id;
                string alias = autoPickle.alias;
                string description = autoPickle.description;
                ulong purchaseCost = autoPickle.purchaseCost;
                long recieve = autoPickle.recieve;
                ulong upgradeCost = autoPickle.upgradeCost;
                currentAmountRequired += 5;
                PlayerData.autoList.autoBuyables.Add(new AutoData(id, alias, description, purchaseCost, recieve, upgradeCost, currentAmountRequired));
            }
        }
    }
}