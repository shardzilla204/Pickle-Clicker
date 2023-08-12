using PickleClicker.Manager;
using PickleClicker.Data.ScriptableObjects.Upgrade;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PickleClicker.Game.Upgrade
{
    public class UpgradeCategoryDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public UpgradeCategoryScriptableObject upgradeCategoryScriptableObject;

        [SerializeField] private DescriptionManager descriptionManager;
        [SerializeField] private Text nameText;
        [SerializeField] private GameObject panelColor;

        private void Start() 
        {   
            descriptionManager = GameObject.FindObjectOfType<DescriptionManager>();
            nameText.text = upgradeCategoryScriptableObject.alias;
        }

        public void OnPointerEnter(PointerEventData eventData) 
        {
            descriptionManager.ShowUpgradeCategory(upgradeCategoryScriptableObject.id);

            panelColor.GetComponent<Image>().color = new Color32(66, 43, 20, 255);
        }

        public void OnPointerExit(PointerEventData eventData) 
        {
            panelColor.GetComponent<Image>().color = new Color32(76, 53, 30, 255);
        }
    }
}
