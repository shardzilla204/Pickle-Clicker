using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PickleClicker.Data.ScriptableObjects.Upgrade
{
    [CreateAssetMenu(fileName = "UpgradeCategory", menuName = "ScriptableObjects/UpgradeCategory")]
    public class UpgradeCategoryScriptableObject : ScriptableObject
    {
        public int id;
        public string alias;
        [TextArea(6, 6)] 
        public string description;

        public AssetLabelReference upgradeAssetReference;
    }
}
