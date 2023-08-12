using UnityEngine;

namespace PickleClicker.Game.Cosmetic
{
    public class DisplayItem : MonoBehaviour
    {
        [SerializeField] private GameObject pickleItem;
        [SerializeField] private GameObject pickleObject;
        [SerializeField] private GameObject pickleButton;

        public void ChangePickleItem(Sprite newSkin)
        {
            pickleItem.GetComponent<SpriteRenderer>().sprite = newSkin;
        }

        public void ChangePickleButton(Sprite newSkin)
        {
            pickleButton.GetComponent<SpriteRenderer>().sprite = newSkin; 
            pickleButton.GetComponent<RectTransform>().sizeDelta = new Vector2(newSkin.rect.width/4.115f, newSkin.rect.height/4.115f);
            pickleObject.GetComponent<SpriteRenderer>().sprite = newSkin;
        }
    }
}
