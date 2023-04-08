using PickleClicker.Data;
using System;
using System.Collections.Generic;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine;

namespace PickleClicker.Poglin
{  
    public class PoglinSpawner : MonoBehaviour
    {
        [SerializeField] private List<PoglinScriptableObject> poglinScriptableObjects = new List<PoglinScriptableObject>(); 

        private GameObject poglinClone;
        [SerializeField] private GameObject poglinWave;
        private ulong picklesRequired = 100;
        private double picklesRequiredMultiplier;

        private Vector2 screenBounds;

        [SerializeField] private AudioSource spawnAudio;

        private void Awake() 
        {        
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            StartCoroutine(RandomizeTime());
        }

        public void CreateWaves()
        {
            if (picklesRequired > PlayerData.pickleData.picklesPicked) return;

            int poglinCount = (int) (PlayerData.pickleData.picklesPicked / picklesRequired);
            int waves = (int) Math.Floor((decimal) (poglinCount));

            if (waves > 2) waves = 2;

            StartCoroutine(CreatePoglins(poglinCount, waves, 5));
            picklesRequired *= (long) 1.042;
        }

        IEnumerator CreatePoglins(int poglinCount, int waves, int poglinsPerWave)
        {
            for (int currentWave = 0; currentWave < waves; currentWave++)
            {   
                poglinWave.GetComponent<TextMesh>().text = $"Wave {currentWave + 1}/{waves}";
                
                spawnAudio.Play();
                for (int poglins = 0; poglins < poglinsPerWave; poglins++)
                {
                    PoglinScriptableObject variant = GetVariant();
                    float distance = Random.Range(screenBounds.x + 1, screenBounds.x + 1.5f);
                    float angle = Random.Range(-Mathf.PI, Mathf.PI);
                    poglinClone = Instantiate(variant.prefab);
                    poglinClone.name = variant.alias;
                    poglinClone.transform.position = new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0);
                    
                    if (poglinClone.transform.position.x > 0)
                    {
                        poglinClone.transform.rotation = Quaternion.Euler(0, 180, 0);
                    }

                    float size = Random.Range(0.47f, 0.32f);
                    poglinClone.transform.localScale = new Vector2(size, size);
                }
                yield return new WaitForSeconds(10);
            }
            // poglinWave.GetComponent<TextMesh>().text = $"Wave {currentWave}";
        }

        private PoglinScriptableObject GetVariant()
        {
            int total = 0;
            
            PoglinScriptableObject scriptableObject = poglinScriptableObjects.Find(scriptableObject => scriptableObject.id == 2);

            UpgradeCategoryData poglinCategory = PlayerData.upgradeList.upgradeCategories.Find(category => category.id == 2);
            UpgradeData shinierPoglin = poglinCategory.upgradeBuyables.Find(upgrade => upgrade.id == 1);
            
            foreach (PoglinScriptableObject variant in poglinScriptableObjects)
            {
                total += variant.rarity;
            }

            // Debug.Log($"Total Value: {total}");

            int random = Random.Range(0, total);

            // Debug.Log($"Random Value: {random}");

            foreach (PoglinScriptableObject variant in poglinScriptableObjects)
            {
                int currentRarity = variant.rarity;

                Debug.Log($"Current rarity: {currentRarity}");

                if ((variant.id == 1 || variant.id == 2) && shinierPoglin.amount > 0) 
                {
                    currentRarity = (int) Math.Round((variant.rarity + shinierPoglin.amount) * 1.25);
                    Debug.Log($"New current rarity: {currentRarity}");
                } 
                else if ((variant.id == 1 || variant.id == 2) && shinierPoglin.amount >= 50)
                {
                    currentRarity = (int) Math.Round((variant.rarity + shinierPoglin.amount) * 2.5);
                    Debug.Log($"New current rarity: {currentRarity}");
                }
                else if ((variant.id == 1 || variant.id == 2) && shinierPoglin.amount == shinierPoglin.maxAmount)
                {
                    currentRarity = (int) Math.Round((variant.rarity + shinierPoglin.amount) * 5.0);
                    Debug.Log($"New current rarity: {currentRarity}");
                }

                random -= currentRarity;

                // Debug.Log($"Random Value is now: {random}");
                if (random <= 0)
                {
                    return scriptableObject = variant;
                }
            }

            return scriptableObject;
        }

        IEnumerator RandomizeTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(120, 180));
                
                CreateWaves();
            }
        }
    }
}
