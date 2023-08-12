using PickleClicker.Manager;
using PickleClicker.Data.ScriptableObjects.Poglin;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PickleClicker.Game.Poglin
{  
    public class PoglinVariant : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private PoglinScriptableObject poglinScriptableObject;
        
        [SerializeField]
        private DescriptionManager descriptionManager;

        public Text nameText;
        public GameObject panel;

        private void Start()
        {   
            descriptionManager = GameObject.FindObjectOfType<DescriptionManager>();
            nameText.text = $"{poglinScriptableObject.alias} Poglin";

            if (poglinScriptableObject.id != 0) return;

            descriptionManager.ShowPoglin(poglinScriptableObject);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            panel.GetComponent<Image>().color = new Color32(66, 43, 20, 255);
            descriptionManager.ShowPoglin(poglinScriptableObject);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            panel.GetComponent<Image>().color = new Color32(76, 53, 30, 255);
        }
    }
}
