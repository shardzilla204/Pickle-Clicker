using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Buyable
{  
    public enum BuyableType {
        One,
        Five,
        Ten,
        Max
    }

    public class BuyableButton : MonoBehaviour {
        public BuyableType type;
    }
}
