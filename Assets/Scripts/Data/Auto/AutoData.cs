namespace PickleClicker.Data.Auto
{
    [System.Serializable]
    public class AutoData
    {
        public int id;
        public string alias;
        public string description;
        public ulong purchaseCost;
        public int currentAmount = 0;
        public int maxAmount = 100;
        public long recieve;
        public ulong upgradeCost;
        public int upgradeLevel = 1;
        public double recieveMultiplier = 1;
        public int amountRequiredForUpgrade;
        
        public AutoData(int id, string alias, string description, ulong purchaseCost, long recieve, ulong upgradeCost, int amountRequiredForUpgrade)
        {
            this.id = id;
            this.alias = alias;
            this.description = description;
            this.purchaseCost = purchaseCost;
            this.recieve = recieve;
            this.upgradeCost = upgradeCost;
            this.amountRequiredForUpgrade = amountRequiredForUpgrade;
        }
    }
}