using PickleClicker.CanvasScripts;
using PickleClicker.Data;
using PickleClicker.Upgrade;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PickleClicker.Pickle
{
    public class PickleJarSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject pickleJar;
        private GameObject pickleJarClone;

        private void Start() 
        {
            StartCoroutine(Loop());
        }

        IEnumerator Loop()
        {
            while (true)
            {
                UpgradeCategoryData jarCategory = PlayerData.upgradeList.upgradeCategories.Find(category => category.id == 1);
                UpgradeData jarSpawnTime = jarCategory.upgradeBuyables.Find(upgrade => upgrade.id == 0);

                int spawnValue = Random.Range(129, 189);
                float newSpawnValue = (float) (spawnValue - jarSpawnTime.amount);
                yield return new WaitForSeconds(newSpawnValue);

                Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
                CreatePickleJar(screenBounds);
                yield return new WaitForSeconds(Random.Range(2f, 3f));
                Destroy(pickleJarClone);
            }
        }

        private void CreatePickleJar(Vector2 screenBounds)
        {
            if (CanvasManager.lastActiveCanvas.gameObject.name != "LoginCanvas" ||
                CanvasManager.desiredCanvas.gameObject.name != "LoginCanvas")
            {
                pickleJarClone = Instantiate(pickleJar);
                pickleJarClone.name = "PickleJar";
                pickleJarClone.transform.position = new Vector3(
                    Random.Range(-screenBounds.x + 0.5f, screenBounds.x - 0.5f), 
                    Random.Range(-screenBounds.y + 0.5f, screenBounds.y - 0.5f), 
                    10
                );
            }
        }
    }
}
