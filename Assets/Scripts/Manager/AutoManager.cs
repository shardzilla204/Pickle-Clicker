using PickleClicker.Data.ScriptableObjects.Auto;
using PickleClicker.Data.Player;
using PickleClicker.Data.Auto;
using PickleClicker.Controller;
using PickleClicker.Manager;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PickleClicker.Manager.Auto
{
    public class AutoManager : MonoBehaviour
    {
        [SerializeField] 
        private AssetLabelReference autoAssetReference;
        
        private void Update() 
        {
            if (CanvasManager.lastActiveCanvas.desiredCanvasType != CanvasType.AutoShop) return;

            SetAutoBuyables();
        }

        public void SetAutoBuyables()
        {

            Addressables.LoadAssetsAsync<AutoScriptableObject>(autoAssetReference, null).Completed += autos =>
            {
                foreach (AutoScriptableObject auto in autos.Result)
                {
                    int amountRequiredForUpgrade = 5 * (auto.id + 1);
                    PlayerData.autoDataList.Add(new AutoData(
                        auto.id, 
                        auto.alias, 
                        auto.description, 
                        auto.purchaseCost, 
                        auto.recieve, 
                        auto.upgradeCost, 
                        amountRequiredForUpgrade
                    ));
                }
            };
        }
    }
}