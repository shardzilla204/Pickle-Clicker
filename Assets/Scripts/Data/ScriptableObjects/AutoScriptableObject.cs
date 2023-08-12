using UnityEngine;

namespace PickleClicker.Data.ScriptableObjects.Auto
{
    [CreateAssetMenu(fileName = "Auto", menuName = "ScriptableObjects/Auto")]
    public class AutoScriptableObject : ScriptableObject
    {
        [HideInInspector]
        public int id;

        [HideInInspector]
        public string alias;

        [HideInInspector]
        public string description;

        [HideInInspector]
        public ulong purchaseCost;

        [HideInInspector]
        public ulong upgradeCost;

        [HideInInspector]
        public int recieve;

        [HideInInspector]
        public int currentAmount = 0;

        [HideInInspector]
        public int maxAmount = 100;

        [HideInInspector]
        public int amountRequiredForUpgrade;

        [HideInInspector]
        public double recieveMultiplier;

        [HideInInspector]
        public int upgradeLevel;

        [HideInInspector]
        public bool enableEditing;
    }
}
