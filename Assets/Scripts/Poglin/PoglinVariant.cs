using PickleClicker.Controller;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PickleClicker.Poglin
{  
    public class PoglinVariant : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private PoglinScriptableObject poglinScriptableObject;
        
        [SerializeField]
        private DescriptionController descriptionController;

        public Text nameText;
        public GameObject panel;

        private void Start()
        {   
            descriptionController = GameObject.FindObjectOfType<DescriptionController>();
            nameText.text = $"{poglinScriptableObject.alias} Poglin";

            if (poglinScriptableObject.id != 0) return;

            descriptionController.ShowPoglin(poglinScriptableObject);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            panel.GetComponent<Image>().color = new Color32(66, 43, 20, 255);
            descriptionController.ShowPoglin(poglinScriptableObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            panel.GetComponent<Image>().color = new Color32(76, 53, 30, 255);
        }
    }
}
