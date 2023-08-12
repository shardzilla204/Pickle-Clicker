using PickleClicker.Data.ScriptableObjects.Cosmetic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PickleClicker.Game.Cosmetic
{

    public enum CosmeticType
    {
        Topper,
        Body,
        Accessory,
        Skin
    }

    public class CosmeticItem : MonoBehaviour
    {

        public CosmeticScriptableObject cosmeticScriptableObject;
        public Image itemIcon;
        public GameObject typePanel;
        public Image typeIcon;
        public static CosmeticItem currentItem;

        public void UpdateIcon()
        {
            itemIcon.sprite = cosmeticScriptableObject.itemIcon;
            typeIcon.sprite = cosmeticScriptableObject.typeIcon;
        }
    }
}
