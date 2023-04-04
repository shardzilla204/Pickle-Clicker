using System.Collections.Generic;
namespace PickleClicker.Data
{
    [System.Serializable]
    public class AutoList
    {
        public List<AutoData> autoBuyables = new List<AutoData>();
        public List<AutoData> autos = new List<AutoData>();
    }
}