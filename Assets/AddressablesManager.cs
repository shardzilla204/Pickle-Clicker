using PickleClicker.Controller;
using PickleClicker.Game.Cosmetic;
using PickleClicker.Data.ScriptableObjects.Cosmetic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace PickleClicker.Manager
{
    public class AddressablesManager : MonoBehaviour
    {
        [SerializeField]
        private AssetLabelReference cosmeticsAssetLabelReference;

        [SerializeField]
        private List<string> keys = new List<string>() { "cosmetics", "icons" };

        [SerializeField]
        private AssetLabelReference cosmeticItemIconsAssetLabelReference;

        [SerializeField]
        private AssetLabelReference cosmeticTypeIconsAssetLabelReference;

        [SerializeField]
        private AssetReferenceGameObject cosmeticGameObjectAssetReference;

        [SerializeField]
        private GameObject cosmeticsContainer;

        public List<GameObject> cosmeticItems = new List<GameObject>();

        public AsyncOperationHandle<IList<CosmeticScriptableObject>> cosmeticOperationHandle;

        public void CheckCanvas() 
        {
            if (CanvasManager.lastActiveCanvas.desiredCanvasType == CanvasType.Inventory)
            {
                cosmeticOperationHandle = Addressables.LoadAssetsAsync<CosmeticScriptableObject>(keys, cosmetic => 
                {
                    CreateCosmeticItem(cosmetic);
                }, Addressables.MergeMode.Union, true);
            }
            else
            {
                foreach(GameObject cosmeticItem in cosmeticItems)
                {
                    cosmeticGameObjectAssetReference.ReleaseInstance(cosmeticItem);
                }
                cosmeticItems.Clear();
                Addressables.Release(cosmeticOperationHandle);
            }
        }

        private void CreateCosmeticItem(CosmeticScriptableObject cosmetic)
        {
            cosmeticGameObjectAssetReference.InstantiateAsync().Completed += (asyncOperation) => 
            {
                GameObject result = asyncOperation.Result;
                CosmeticItem cosmeticItem = result.GetComponent<CosmeticItem>();
                Sprite cosmeticSprite = cosmetic.item;
                SpriteMerger spriteMerger = GameObject.FindObjectOfType<SpriteMerger>();
                Debug.Log(spriteMerger.sprites.Contains(cosmeticSprite));
                if (spriteMerger.sprites.Contains(cosmeticSprite))
                {
                    cosmeticGameObjectAssetReference.ReleaseInstance(result);
                }
                else
                {
                    cosmeticItems.Add(result);
                }
                result.name = cosmetic.name;
                result.GetComponent<CosmeticItem>().cosmeticScriptableObject = cosmetic;
                result.GetComponent<CosmeticItem>().UpdateIcon();
                result.transform.SetParent(cosmeticsContainer.transform);
                result.transform.localScale = new Vector3(1, 1, 1);
                result.GetComponent<DraggableItem>().cosmeticsContainer = cosmeticsContainer;
            };
        }
    }
}