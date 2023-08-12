using UnityEngine;
using UnityEngine.EventSystems;

namespace PickleClicker.Game.Cosmetic
{
    public class Inventory : MonoBehaviour, IDropHandler
    {
        [SerializeField] 
        private SpriteMerger spriteMerger;
        
        [SerializeField] 
        private GameObject cosmetics;

        private void Start() 
        {
            spriteMerger = GameObject.FindObjectOfType<SpriteMerger>();
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
