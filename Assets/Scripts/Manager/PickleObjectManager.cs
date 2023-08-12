using Random = UnityEngine.Random;
using UnityEngine;

namespace PickleClicker.Manager.Pickle
{
    public class PickleObjectManager : MonoBehaviour
    {
        [SerializeField] private GameObject pickleObject;
        [SerializeField] private GameObject pickleButton;
        public static int pickleClones;

        public void CreatePickle()
        {
            if (pickleClones < 15)
            {
                pickleClones++;
                GameObject pickleClone = Instantiate(pickleObject);
                pickleClone.GetComponent<SpriteRenderer>().sprite = pickleButton.GetComponent<SpriteRenderer>().sprite;
                pickleClone.name = "Pickle";
                pickleClone.GetComponent<SpriteRenderer>().color = new Color32(175, 175, 175, 255);
                pickleClone.transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(5f, 10f), 1);
                pickleClone.transform.rotation = Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));
                float size = Random.Range(0.015f, 0.005f);
                pickleClone.transform.localScale = new Vector2(size, size);
            }
        }
    }
}