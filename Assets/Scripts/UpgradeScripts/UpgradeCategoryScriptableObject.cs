using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PickleClicker.Upgrade
{
    [CreateAssetMenu(fileName = "UpgradeCategory", menuName = "ScriptableObjects/UpgradeCategory")]
    public class UpgradeCategoryScriptableObject : ScriptableObject
    {
        public int id;
        public string alias;
        [TextArea(6, 6)] public string description;

        public List<UpgradePickleScriptableObject> upgradePickleScriptableObjects;
    }
}
