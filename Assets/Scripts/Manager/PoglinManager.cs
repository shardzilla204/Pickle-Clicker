using PickleClicker.Data.ScriptableObjects.Poglin;
using PickleClicker.Data.Player;
using PickleClicker.Data.Upgrade;
using System;
using System.Collections.Generic;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine;

namespace PickleClicker.Manager.Poglin
{  
    public class PoglinManager : MonoBehaviour
    {
        [SerializeField]
        private List<PoglinScriptableObject> poglinScriptableObjects = new List<PoglinScriptableObject>(); 

        private GameObject poglinClone;

        [SerializeField] 
        private GameObject poglinWave;

        [SerializeField] 
        private ulong picklesRequired = 100;

        private double picklesRequiredMultiplier;

        private Vector2 screenBounds;

        private AudioSource spawnAudioSource;

        private void Awake() 
        {        
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            StartCoroutine(RandomizeTime());
        }

        private void Start() 
        {
            spawnAudioSource = gameObject.GetComponent<AudioSource>();
        }

        public void CreateWaves()
        {
            if (picklesRequired > PlayerData.pickleData.pickles) return;

            int poglinCount = (int) (PlayerData.pickleData.pickles / picklesRequired);
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
                
                spawnAudioSource.Play();
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
                poglinWave.GetComponent<TextMesh>().text = $"Wave {currentWave}";
                yield return new WaitForSeconds(10);
            }
        }

        private PoglinScriptableObject GetVariant()
        {
            int total = 0;
            
            PoglinScriptableObject scriptableObject = poglinScriptableObjects.Find(scriptableObject => scriptableObject.id == 2);

            UpgradeCategoryData poglinCategory = PlayerData.upgradeCategoryDataList.Find(category => category.id == 2);
            UpgradeData shinierPoglin = poglinCategory.upgrades.Find(upgrade => upgrade.id == 1);
            
            foreach (PoglinScriptableObject variant in poglinScriptableObjects)
            {
                total += variant.rarity;
            }

            int random = Random.Range(0, total);

            const int PINK_POGLIN_ID = 1;
            const int GOLD_POGLIN_ID = 2;

            foreach (PoglinScriptableObject variant in poglinScriptableObjects)
            {
                int currentRarity = variant.rarity;

                if ((variant.id == PINK_POGLIN_ID || variant.id == GOLD_POGLIN_ID) && shinierPoglin.amount > 0) 
                {
                    currentRarity = (int) Math.Round((variant.rarity + shinierPoglin.amount) * 1.25);
                } 
                else if ((variant.id == PINK_POGLIN_ID || variant.id == GOLD_POGLIN_ID) && shinierPoglin.amount >= 50)
                {
                    currentRarity = (int) Math.Round((variant.rarity + shinierPoglin.amount) * 2.5);
                }
                else if ((variant.id == PINK_POGLIN_ID || variant.id == GOLD_POGLIN_ID) && shinierPoglin.amount == shinierPoglin.maxAmount)
                {
                    currentRarity = (int) Math.Round((variant.rarity + shinierPoglin.amount) * 5.0);
                }

                random -= currentRarity;

                if (random <= 0)
                {
                    return scriptableObject = variant;
                }
            }

            return scriptableObject;
        }

        IEnumerator RandomizeTime()
        {
            const int MINIMUM_TIME = 120;
            const int MAXIMUM_TIME = 180;
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(MINIMUM_TIME, MAXIMUM_TIME));
                CreateWaves();
            }
        }
    }
}
