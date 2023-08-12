namespace PickleClicker.Data.Upgrade
{
    [System.Serializable]
    public class UpgradeData
    {
        public int categoryId;
        public int id;
        public string alias;
        public string description;
        public ulong cost;
        public int amount;
        public int maxAmount;

        public UpgradeData(int categoryId, int id, string alias, string description, ulong cost, int amount, int maxAmount) 
        {
            this.categoryId = categoryId;
            this.id = id;
            this.alias = alias;
            this.description = description;
            this.cost = cost;
            this.amount = amount;
            this.maxAmount = maxAmount;
        }
    }
}
