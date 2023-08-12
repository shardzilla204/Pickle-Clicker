using PickleClicker.Controller.Cosmetic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PickleClicker.Manager.Cosmetic 
{  
    public class CosmeticManager : MonoBehaviour
    {
        List<CosmeticController> cosmeticControllers;
        public static CosmeticManager cosmeticManager;

        private void Start() 
        {
            cosmeticManager = gameObject.GetComponent<CosmeticManager>();
            cosmeticControllers = cosmeticManager.GetComponentsInChildren<CosmeticController>().ToList();
        }
    }
}
