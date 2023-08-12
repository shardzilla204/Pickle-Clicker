using PickleClicker.Game.Cosmetic;
using UnityEngine;

namespace PickleClicker.Data.ScriptableObjects.Cosmetic
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Cosmetic")]
    public class CosmeticScriptableObject : ScriptableObject
    {
        public string alias;
        [TextArea(4, 4)]
        public string description;
        public Sprite itemIcon;
        public Sprite item;
        public Sprite typeIcon;
        public CosmeticType cosmeticType;
    }
}
