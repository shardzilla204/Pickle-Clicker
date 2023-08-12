using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Game.Auto
{  
    public enum AutoAmountType {
        One,
        Five,
        TwentyFive,
        Max
    }

    public class AutoAmountButton : MonoBehaviour {
        public AutoAmountType type;
    }
}
