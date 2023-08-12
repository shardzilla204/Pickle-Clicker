using PickleClicker.Manager;
using PickleClicker.Data.ScriptableObjects.Auto;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PickleClicker.Game.Auto
{
    public class AutoBuyable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] 
        private AutoScriptableObject autoScriptableObject;

        [SerializeField] 
        private DescriptionManager descriptionManager;

        [SerializeField] 
        private Text aliasText;

        [SerializeField] 
        private Image panelImage;

        private Color32 COLOR_ON_ENTER = new Color32(66, 43, 20, 255);
        private Color32 COLOR_ON_EXIT = new Color32(76, 53, 30, 255);

        private void Start()
        {   
            descriptionManager = GameObject.FindObjectOfType<DescriptionManager>();
            panelImage = gameObject.GetComponentInChildren<Image>();
            aliasText = gameObject.GetComponentInChildren<Text>();
            aliasText.text = $"Pickle {autoScriptableObject.alias}";
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            panelImage.color = COLOR_ON_ENTER;
            Debug.Log(autoScriptableObject);
            descriptionManager.ShowAuto(autoScriptableObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            panelImage.color = COLOR_ON_EXIT;
        }
    }
}
