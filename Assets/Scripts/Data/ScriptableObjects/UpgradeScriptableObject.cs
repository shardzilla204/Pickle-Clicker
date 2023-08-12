using UnityEngine;

namespace PickleClicker.Data.ScriptableObjects.Upgrade
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/Upgrade")]
    public class UpgradeScriptableObject : ScriptableObject
    {
        [HideInInspector]
        public int categoryID;

        [HideInInspector]
        public int id;

        [HideInInspector]
        public string alias;

        [HideInInspector]
        public string description;

        [HideInInspector]
        public ulong purchaseCost;

        [HideInInspector]
        public int currentAmount;

        [HideInInspector]
        public int maxAmount;

        [HideInInspector]
        public bool enableEditing;
    }
}
