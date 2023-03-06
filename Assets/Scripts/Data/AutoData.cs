namespace PickleClicker.Data
{
    [System.Serializable]
    public class AutoData
    {
        public int id;
        public string alias;
        public string description;
        public ulong purchaseCost;
        public int amount = 0;
        public long recieve;
        public ulong upgradeCost;
        public int level = 1;
        public double multiplier = 1;
        public int amountRequired;
        public int maxAmount = 100;
        
        public AutoData(int id, string alias, string description, ulong purchaseCost, long recieve, ulong upgradeCost, int amountRequired)
        {
            this.id = id;
            this.alias = alias;
            this.description = description;
            this.purchaseCost = purchaseCost;
            this.recieve = recieve;
            this.upgradeCost = upgradeCost;
            this.amountRequired = amountRequired;
        }
    }
}