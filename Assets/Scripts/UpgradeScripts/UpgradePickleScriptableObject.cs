using UnityEngine;

namespace PickleClicker.Upgrade
{

    [CreateAssetMenu(fileName = "UpgradePickle", menuName = "ScriptableObjects/UpgradePickle")]
    public class UpgradePickleScriptableObject : ScriptableObject
    {
        public int id;
        public string alias;
        [TextArea(6, 6)] public string description;
        public ulong cost;
        public int amount;
        public int maxAmount = 69;
    }
}
