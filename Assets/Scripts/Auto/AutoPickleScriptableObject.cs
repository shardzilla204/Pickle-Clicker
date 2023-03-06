using UnityEngine;

namespace PickleClicker.Auto
{
    [CreateAssetMenu(fileName = "AutoPickle", menuName = "ScriptableObjects/AutoPickle")]
    public class AutoPickleScriptableObject : ScriptableObject
    {
        public int id;
        public string alias;
        [TextArea(8, 8)]
        public string description;
        public ulong purchaseCost;
        public ulong upgradeCost;
        public int recieve;
    }
}
