using System.Collections.Generic;

namespace PickleClicker.Data.Upgrade
{
    [System.Serializable]
    public class UpgradeCategoryData
    {
        public int id;
        public string alias;
        public string description;
        public List<UpgradeData> upgrades;

        public UpgradeCategoryData(int id, string alias, string description, List<UpgradeData> upgrades) 
        {
            this.id = id;
            this.alias = alias;
            this.description = description;
            this.upgrades = upgrades;
        }
    }
}
