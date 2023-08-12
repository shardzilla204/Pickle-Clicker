
using PickleClicker.Manager.Cosmetic;
using UnityEngine;

namespace PickleClicker.Controller.Cosmetic
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
