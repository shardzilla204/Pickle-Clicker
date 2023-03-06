using PickleClicker.Controller;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PickleClicker.Upgrade
{
    public class UpgradeCategory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public UpgradeCategoryScriptableObject upgradeCategoryScriptableObject;

        [SerializeField] private DescriptionController descriptionController;
        [SerializeField] private Text nameText;
        [SerializeField] private GameObject panelColor;

        private void Start() 
        {   
            descriptionController = GameObject.FindObjectOfType<DescriptionController>();
            nameText.text = upgradeCategoryScriptableObject.alias;
        }

        public void OnPointerEnter(PointerEventData eventData) 
        {
            // Debug.Log($"ID: {upgradeCategoryScriptableObject.id}");
            descriptionController.ShowUpgradeCategory(upgradeCategoryScriptableObject.id);

            panelColor.GetComponent<Image>().color = new Color32(66, 43, 20, 255);
        }

        public void OnPointerExit(PointerEventData eventData) 
        {
            panelColor.GetComponent<Image>().color = new Color32(76, 53, 30, 255);
        }
    }
}
