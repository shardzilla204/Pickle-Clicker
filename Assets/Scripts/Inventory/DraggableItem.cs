using PickleClicker.Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PickleClicker.Game.Cosmetic
{  
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        private Camera cam;
        private Vector3 dragOffset;
        private Image image;
        private CanvasGroup canvasGroup;
        [HideInInspector] public Transform parentAfterDrag;
        private int clicked = 0;
        private float clickTime = 0;
        private float clickDelay = 0.275f;
        
        public GameObject cosmeticsContainer;

        [SerializeField] 
        private List<EquipSlot> equipSlots = new List<EquipSlot>(); 

        [SerializeField] 
        private SpriteMerger spriteMerger;

        private void Awake() 
        {
            cam = Camera.main;
            equipSlots = GameObject.FindObjectsOfType<EquipSlot>().ToList();
            image = gameObject.GetComponent<Image>();
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            spriteMerger = GameObject.FindObjectOfType<SpriteMerger>();
        }

        private void Update() 
        {
            if (clicked > 2 || Time.time - clickTime > clickDelay) clicked = 0;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            canvasGroup.alpha = 0.5f;
            dragOffset = transform.position - GetMousePos();
            transform.SetParent(transform.parent.parent.parent.parent.parent.parent);
            parentAfterDrag = cosmeticsContainer.GetComponent<Transform>().transform;
            transform.SetAsLastSibling();
            image.raycastTarget = false;
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = GetMousePos() + dragOffset;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            canvasGroup.alpha = 1f;
            Transform cosmeticsContainerTransform = cosmeticsContainer.GetComponent<Transform>();
            foreach (EquipSlot equipSlot in equipSlots)
            {
                Transform equipSlotTransform = equipSlot.GetComponent<Transform>();
                if (parentAfterDrag == equipSlotTransform || parentAfterDrag == cosmeticsContainerTransform)
                {
                    transform.SetParent(parentAfterDrag);
                    AddressablesManager addressablesManager = GameObject.FindObjectOfType<AddressablesManager>();
                    addressablesManager.cosmeticItems.Remove(gameObject);
                }
            }

            if (transform.parent == cosmeticsContainer.GetComponent<Transform>())
            {
                GameObject.FindObjectOfType<CosmeticItem>().typePanel.SetActive(true);
                AddressablesManager addressablesManager = GameObject.FindObjectOfType<AddressablesManager>();
                addressablesManager.cosmeticItems.Add(gameObject);
            }

            string parent = gameObject.transform.parent.ToString();

            if (parent.Contains("Cosmetics"))
            {
                gameObject.GetComponent<CosmeticItem>().typePanel.SetActive(true);
                AddressablesManager addressablesManager = GameObject.FindObjectOfType<AddressablesManager>();
                addressablesManager.cosmeticItems.Add(gameObject);
            }

            image.raycastTarget = true; 
            canvasGroup.blocksRaycasts = true;  
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.clickCount == 1) clickTime = Time.time;

            if (eventData.clickCount > 1 && Time.time - clickTime < eventData.clickTime)
            {
                clicked = 0;
                Debug.Log($"Before Parent: {transform.parent}");
                Transform cosmeticsTransform = cosmeticsContainer.GetComponent<Transform>();

                foreach (EquipSlot equipSlot in equipSlots)
                {
                    string parent = gameObject.transform.parent.ToString();
                    Transform equipSlotTransform = equipSlot.GetComponent<Transform>();
                    CosmeticType cosmeticType = eventData.pointerClick.GetComponent<CosmeticItem>().cosmeticScriptableObject.cosmeticType;

                    if (equipSlotTransform.childCount == 1 && !parent.Contains("EquipSlot") && cosmeticType == equipSlot.slotType)
                    {
                        transform.SetParent(equipSlotTransform);
                        spriteMerger.AppendToList(gameObject.GetComponent<CosmeticItem>());
                        return;
                    }
                    else if (parent.Contains("EquipSlot"))
                    {
                        transform.SetParent(transform.root);
                        transform.SetAsLastSibling();
                        transform.SetParent(cosmeticsTransform);
                        spriteMerger.RemoveFromList(gameObject);
                        AddressablesManager addressablesManager = GameObject.FindObjectOfType<AddressablesManager>();
                        addressablesManager.cosmeticItems.Remove(gameObject);
                        return;
                    }
                }
                Debug.Log($"After Parent: {transform.parent}");
            }
        }

        private Vector3 GetMousePos()
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            return mousePos;
        }
    }
}
