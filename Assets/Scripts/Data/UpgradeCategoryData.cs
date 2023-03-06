using System.Collections.Generic;

namespace PickleClicker.Data
{
    [System.Serializable]
    public class UpgradeCategoryData
    {
        public int id;
        public string alias;
        public string description;
        public List<UpgradeData> upgradeBuyables;

        public UpgradeCategoryData(int id, string alias, string description, List<UpgradeData> upgradeBuyables) 
        {
            this.id = id;
            this.alias = alias;
            this.description = description;
            this.upgradeBuyables = upgradeBuyables;
        }
    }
}
