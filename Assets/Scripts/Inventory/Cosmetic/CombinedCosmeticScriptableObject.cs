using UnityEngine;

namespace PickleClicker 
{
    [CreateAssetMenu(menuName = "ScriptableObjects/CombinedCosmetic")]
    public class CombinedCosmeticScriptableObject : ScriptableObject
    {
        public int id;
        [TextArea(4, 4)]
        public string description;
        public GameObject prefab;
        public Sprite icon;
        public CosmeticType itemType;
        public CosmeticScriptableObject[] items;
    }
}

