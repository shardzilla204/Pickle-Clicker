using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PickleClicker 
{

    public enum CosmeticType
    {
        Topper,
        Body,
        Accessory,
        Skin
    }

    public class Cosmetic : MonoBehaviour, IPointerClickHandler
    {

        public CosmeticScriptableObject cosmeticScriptableObject;
        public Image icon;
        public GameObject typePanel;
        public Image typeIcon;
        public int level = 0;
        public static Cosmetic currentItem;

        private void Awake()
        {
            icon.sprite = cosmeticScriptableObject.icon;
            typeIcon.sprite = cosmeticScriptableObject.typeIcon;
            // level = cosmeticScriptableObject.tiers[level].level;
            // Debug.Log(ItemData.tiers[level]);
        }

        // private void Start() 
        // {
        //     ItemSystem.HideUpgradeWindow();
        //     ItemSystem.HideSwitchWindow();
        // }

        public void OnPointerClick(PointerEventData eventData)
        {
            // currentItem = gameObject.GetComponent<CosmeticItem>();
            // if (eventData.button == PointerEventData.InputButton.Right)
            // {
            //     ItemSystem.ShowUpgradeWindow(CosmeticItemData.tiers[level]);
            // }

            // if (eventData.button == PointerEventData.InputButton.Left)
            // {
            //     ItemSystem.ShowSwitchWindow(ItemData.tiers, ItemData.combinedItems);
            // }
        }

        // public void UpgradeCosmetic()
        // {
        //     Int32.TryParse(ItemData.cost, out int cost);
        //     if (
        //         cost >= PickleController.buyableData.pickleCoins && 
        //         ItemData.tiers[level].unlocked &&
        //         ItemData.tiers[level] != null
        //     )
        //     {
        //         PickleController.buyableData.pickleCoins -= cost;
        //         level++;
        //         icon.sprite = ItemData.tiers[level].icon;
        //         ItemData.tiers[level].unlocked = true;
        //     }
        // }
    }
}
