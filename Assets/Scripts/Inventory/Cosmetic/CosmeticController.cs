using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PickleClicker 
{
    public class CosmeticController : MonoBehaviour
    {
        public static CosmeticManager cosmeticManager;

        private void Start() 
        {
            cosmeticManager = CosmeticManager.cosmeticManager;
        }
    }
}
