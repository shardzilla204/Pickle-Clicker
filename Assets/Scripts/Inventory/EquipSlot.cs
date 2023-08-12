using UnityEngine;
using UnityEngine.EventSystems;

namespace PickleClicker.Game.Cosmetic
{  
public class EquipSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] 
    private SpriteMerger spriteMerger;

    public CosmeticType slotType;

    [SerializeField] 
    private GameObject cosmetics;
    
    [SerializeField] 
    private GameObject typeIcon;

    private void Start() 
    {
        spriteMerger = GameObject.FindObjectOfType<SpriteMerger>();
    }

    private void Update() 
    {
        typeIcon.SetActive(true);

        if (transform.childCount == 1) return;

        typeIcon.SetActive(false);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        CosmeticType cosmeticType = dropped.GetComponent<CosmeticItem>().cosmeticScriptableObject.cosmeticType;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();

        if (slotType != cosmeticType)
        {
            draggableItem.transform.SetParent(cosmetics.GetComponent<Transform>().transform);
        }
        else
        {
            if (transform.childCount == 1)
            {
                draggableItem.parentAfterDrag = transform;
                spriteMerger.AppendToList(dropped.GetComponent<CosmeticItem>());
            }
        }
    }

    public void OnClear()
    {
        DraggableItem draggableItem = gameObject.GetComponentInChildren<DraggableItem>();
        draggableItem.transform.SetParent(cosmetics.GetComponent<Transform>().transform);
    }
}

}
