using UnityEngine;
using UnityEngine.EventSystems;

namespace PickleClicker 
{  
public class EquipSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] SpriteMerger spriteMerger;
    [SerializeField] Cosmetics cosmetics;
    [SerializeField] public CosmeticType slotType;
    [SerializeField] GameObject typeIcon;

    private void Start() 
    {
        cosmetics = GameObject.FindObjectOfType<Cosmetics>();
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
        CosmeticType cosmeticType = dropped.GetComponent<Cosmetic>().cosmeticScriptableObject.cosmeticType;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();

        if (slotType != cosmeticType)
        {
            Debug.Log("True and real");
            draggableItem.transform.SetParent(cosmetics.GetComponent<Transform>().transform);
        }
        else
        {
            if (transform.childCount == 1)
            {
                typeIcon.SetActive(false);
                draggableItem.parentAfterDrag = transform;

                spriteMerger.AppendToList(dropped);
            }
        }
    }

    public void OnClear()
    {
        DraggableItem draggableItem = gameObject.GetComponentInChildren<DraggableItem>();
        draggableItem.transform.SetParent(cosmetics.GetComponent<Transform>().transform);
        Debug.Log(draggableItem.parentAfterDrag);
    }
}

}
