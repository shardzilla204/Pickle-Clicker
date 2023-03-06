using System.Collections.Generic;

namespace PickleClicker.Data
{
    [System.Serializable]
    public class UpgradeList
    {
        public List<UpgradeCategoryData> upgradeCategories = new List<UpgradeCategoryData>();
    }
}