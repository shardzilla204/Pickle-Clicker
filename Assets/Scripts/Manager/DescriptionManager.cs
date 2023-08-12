using PickleClicker.Controller.Auto;
using PickleClicker.Controller.Upgrade;
using PickleClicker.Controller.Poglin;
using PickleClicker.Data.ScriptableObjects.Poglin;
using PickleClicker.Data.ScriptableObjects.Auto;
using PickleClicker.Data.Upgrade;
using PickleClicker.Data.Player;

using UnityEngine;

namespace PickleClicker.Manager
{  
    public class DescriptionManager : MonoBehaviour
    {   
        [SerializeField]
        private AutoController autoController;

        [SerializeField]
        private UpgradeCategoryController upgradeCategoryController;

        [SerializeField]
        private PoglinController poglinController;

        private void Awake() 
        {
            autoController = GameObject.FindObjectOfType<AutoController>();
            upgradeCategoryController = GameObject.FindObjectOfType<UpgradeCategoryController>();
            poglinController = GameObject.FindObjectOfType<PoglinController>();
        }

        public void ShowAuto(AutoScriptableObject autoScriptableObject) 
        {
            autoController.SetBuyable(autoScriptableObject);
        }

        public void ShowUpgradeCategory(int id) 
        {
            UpgradeCategoryData upgradeCategoryData = PlayerData.upgradeCategoryDataList.Find(upgradeCategory => upgradeCategory.id == id);
            upgradeCategoryController.SetCategory(upgradeCategoryData);
        }

        public void ShowPoglin(PoglinScriptableObject scriptableObject) 
        {
            poglinController.SetPoglin(scriptableObject);
        }
    }
}
