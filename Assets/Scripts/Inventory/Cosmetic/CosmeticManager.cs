using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PickleClicker 
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
