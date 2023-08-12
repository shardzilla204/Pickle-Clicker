namespace PickleClicker.Data.Poglin
{
    [System.Serializable]
    public class PoglinData
    {
        public int id;
        public string alias;
        public long killed = 0;
        
        public PoglinData(int id, string alias)
        {
            this.id = id;
            this.alias = alias;
        }

    }
}