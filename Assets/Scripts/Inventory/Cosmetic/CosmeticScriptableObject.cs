using UnityEngine;

namespace PickleClicker 
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Cosmetic")]
    public class CosmeticScriptableObject : ScriptableObject
    {
        public int id;
        public string displayName;
        public string cost;
        [TextArea(4, 4)]
        public string description;
        public GameObject prefab;
        public Sprite icon;
        public Sprite cosmetic;
        public Sprite typeIcon;
        public CosmeticType cosmeticType;
        public int level;
        public CosmeticScriptableObject[] tiers;
        public CombinedCosmeticScriptableObject[] combinedCosmetics;
        public bool unlocked;
    }
}
