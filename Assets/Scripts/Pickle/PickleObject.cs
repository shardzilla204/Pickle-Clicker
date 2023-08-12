using Random = UnityEngine.Random;
using UnityEngine;
using PickleClicker.Manager.Pickle;

namespace PickleClicker.Game.Pickle
{
    public class PickleObject : MonoBehaviour
    {
        private Vector2 screenBounds;
        
        private void Awake() 
        {
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        }

        private void Update()
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y - Random.Range(0.075f, 0.1f));
            this.gameObject.GetComponent<Rigidbody2D>().MovePosition(position);
            this.gameObject.GetComponent<Rigidbody2D>().rotation += 0.5f;

            if (transform.position.y >= -screenBounds.y * 1.5) return;
            
            Destroy(this.gameObject);
        }

        private void OnDestroy() 
        {
            PickleObjectManager.pickleClones--;
        }
    }
}
