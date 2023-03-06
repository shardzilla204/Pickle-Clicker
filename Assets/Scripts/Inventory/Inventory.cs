using UnityEngine;
using UnityEngine.EventSystems;

namespace PickleClicker 
{
    public class Inventory : MonoBehaviour, IDropHandler
    {
        [SerializeField] SpriteMerger spriteMerger;
        [SerializeField] Cosmetics cosmetics;

        private void Start() 
        {
            spriteMerger = GameObject.FindObjectOfType<SpriteMerger>();
            cosmetics = GameObject.FindObjectOfType<Cosmetics>();
        }

        public void OnDrop(PointerEventData eventData)
        {
            GameObject dropped = eventData.pointerDrag;
            Debug.Log($"Hello my name is: {dropped}");
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.transform.SetParent(cosmetics.GetComponent<Transform>().transform);
            spriteMerger.RemoveFromList(dropped);
        }
    }
}
