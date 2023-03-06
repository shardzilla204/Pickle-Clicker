namespace PickleClicker.Data
{
    [System.Serializable]
    public class PoglinVariantData 
    {
        public int id;
        public string alias;
        public long killed = 0;
        
        public PoglinVariantData(int id, string alias)
        {
            this.id = id;
            this.alias = alias;
        }

    }
}