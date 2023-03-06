using PickleClicker.Controller;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PickleClicker.Auto
{
    public class AutoBuyable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public AutoPickleScriptableObject autoPickleScriptableObject;

        [SerializeField] private DescriptionController descriptionController;

        public Text nameText;
        public GameObject panelColor;

        private void Awake()
        {   
            descriptionController = GameObject.FindObjectOfType<DescriptionController>();
            nameText.text = $"Pickle {autoPickleScriptableObject.alias}";
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            panelColor.GetComponent<Image>().color = new Color32(66, 43, 20, 255);
            descriptionController.ShowAuto(autoPickleScriptableObject.id);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            panelColor.GetComponent<Image>().color = new Color32(76, 53, 30, 255);
        }
    }
}
