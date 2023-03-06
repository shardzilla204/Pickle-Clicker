using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PickleClicker.CanvasScripts
{  
    public class ChangeInput : MonoBehaviour
    {
        EventSystem system;
        public Selectable firstInput;
        public Button submitButton;
        // Start is called before the first frame update
        void Start()
        {
            system = EventSystem.current;
            firstInput.Select();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift)) {
                Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
                
                if (next == null) return;

                next.Select();
            }
            else if (Input.GetKeyDown(KeyCode.Tab)) {
                Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                
                if (next == null) return;
                
                next.Select();
            }
            else if (Input.GetKeyDown(KeyCode.Return)) 
            {
                submitButton.onClick.Invoke();
            }
        }
    }
}
