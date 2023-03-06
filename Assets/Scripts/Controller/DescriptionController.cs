using PickleClicker.Upgrade;
using PickleClicker.Auto;
using PickleClicker.Poglin;
using UnityEngine;

namespace PickleClicker.Controller
{  
    public class DescriptionController : MonoBehaviour
    {   
        public AutoBuyableManager autoBuyableManager;
        public UpgradeCategoryManager upgradeCategoryManager;
        public PoglinVariantManager poglinVariantManager;

        // private void Start() 
        // {
        //     autoBuyableManager = GameObject.FindObjectOfType<AutoBuyableManager>();
        //     upgradeCategoryManager = GameObject.FindObjectOfType<UpgradeCategoryManager>();
        //     poglinVariantManager = GameObject.FindObjectOfType<PoglinVariantManager>();
        // }

        public void ShowAuto(int id) 
        {
            autoBuyableManager.SetBuyable(id);
        }

        public void ShowUpgradeCategory(int id) 
        {
            upgradeCategoryManager.SetCategory(id);
        }

        public void ShowPoglin(PoglinScriptableObject scriptableObject) 
        {
            poglinVariantManager.SetPoglin(scriptableObject);
        }
    }
}
