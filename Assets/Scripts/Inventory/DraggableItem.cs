using PickleClicker.CanvasScripts;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PickleClicker 
{  
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        private Camera cam;
        private Vector3 dragOffset;
        private Image image;
        private Canvas canvas;
        private CanvasGroup canvasGroup;
        [HideInInspector] public Transform parentAfterDrag;
        private int clicked = 0;
        private float clickTime = 0;
        private float clickDelay = 0.275f;
        [SerializeField] private Cosmetics cosmetics;
        [SerializeField] private List<EquipSlot> equipSlots = new List<EquipSlot>(); 
        [SerializeField] private SpriteMerger spriteMerger;

        private void Awake() 
        {
            cam = Camera.main;
        }

        private void Start() 
        {
            canvas = GameObject.FindObjectOfType<CanvasManager>().GetComponent<Canvas>();
            equipSlots = GameObject.FindObjectsOfType<EquipSlot>().ToList();
            image = gameObject.GetComponent<Image>();
            canvasGroup = gameObject.GetComponent<CanvasGroup>();
            cosmetics = GameObject.FindObjectOfType<Cosmetics>();
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
            if (transform.parent == cosmetics.GetComponent<Transform>().transform) 
            {
                transform.SetParent(transform.parent.parent.parent.parent.parent);
            }
            else
            {
                transform.SetParent(transform.parent.parent.parent.parent.parent.parent);
                parentAfterDrag = cosmetics.GetComponent<Transform>().transform;
            }
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
            Transform cosmeticsTransform = cosmetics.GetComponent<Transform>();
            foreach (EquipSlot equipSlot in equipSlots)
            {
                Transform equipSlotTransform = equipSlot.GetComponent<Transform>();
                if (parentAfterDrag == equipSlotTransform.transform || parentAfterDrag == cosmeticsTransform.transform)
                {
                    transform.SetParent(parentAfterDrag);
                }
            }

            image.raycastTarget = true; 
            canvasGroup.blocksRaycasts = true;  
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                clicked++;
            }

            if (clicked == 1) clickTime = Time.time;
            

            if (clicked > 1 && Time.time - clickTime < clickDelay)
            {
                clicked = 0;
                Debug.Log($"Before Parent: {transform.parent}");
                Transform cosmeticsTransform = cosmetics.GetComponent<Transform>();

                foreach (EquipSlot equipSlot in equipSlots)
                {
                    string parent = gameObject.transform.parent.ToString();
                    Transform equipSlotTransform = equipSlot.GetComponent<Transform>();
                    CosmeticType cosmeticType = eventData.pointerClick.GetComponent<Cosmetic>().cosmeticScriptableObject.cosmeticType;

                    if (equipSlotTransform.childCount == 1 && !parent.Contains("EquipSlot") && cosmeticType == equipSlot.slotType)
                    {
                        Debug.Log("Why hello Luigi");
                        transform.SetParent(equipSlotTransform);
                        spriteMerger.AppendToList(gameObject);
                        return;
                    }
                    else if (parent.Contains("EquipSlot"))
                    {
                        Debug.Log("Why hello Mario");
                        transform.SetParent(transform.root);
                        transform.SetAsLastSibling();
                        transform.SetParent(cosmeticsTransform);
                        spriteMerger.RemoveFromList(gameObject);
                        return;
                    }
                }
                Debug.Log($"After Parent: {transform.parent}");
                return;
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
