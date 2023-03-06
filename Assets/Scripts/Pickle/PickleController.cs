using PickleClicker.Data;
using PickleClicker.Upgrade;
using PickleClicker.Controller;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

namespace PickleClicker.Pickle
{
    public class PickleController : MonoBehaviour
    {
        [SerializeField] private Text pickleLevelText;

        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] pickleSounds;
        private int currentSound = 0;
        [SerializeField] private GameObject pickleValue;

        [SerializeField] private GameObject pickleButton;

        private static ulong totalPicklesPicked;
        private static long totalCoinsCollected;
        private static long totalClicks;

        private void Awake() 
        {
            totalPicklesPicked = PlayerData.pickleData.totalPicklesPicked;
            totalCoinsCollected = PlayerData.pickleData.totalCoinsCollected;
            totalClicks = PlayerData.pickleData.totalClicks;
        }

        private void Update() 
        {
            string pickleLevel = PlayerData.pickleData.pickleLevel.ToString("N0");
            pickleLevelText.text = $"{pickleLevel}";

            CheckLevelProgress();
        }

        //Adds pickles every click 
        public void PickleClick()
        {
            UpgradeCategoryData clickCategory = PlayerData.upgradeList.upgradeCategories.Find(category => category.id == 0);
            UpgradeData increaseClick = clickCategory.upgradeBuyables.Find(upgrade => upgrade.id == 0);
            UpgradeData clickExperience = clickCategory.upgradeBuyables.Find(upgrade => upgrade.id == 2);
            
            int pickleLevel = PlayerData.pickleData.pickleLevel;
            int division = (pickleLevel / 5);
            if (increaseClick.amount == 0)
            {
                PlayerData.pickleData.picklesPerClick = (long) division;
            }
            else 
            {
                PlayerData.pickleData.picklesPerClick = (long) (division * (increaseClick.amount + 1));
            }
            Debug.Log($"Pickles Per Click: {PlayerData.pickleData.picklesPerClick}");

            if (PlayerData.pickleData.picklesPerClick == 0) PlayerData.pickleData.picklesPerClick = 1;

            PlayerData.pickleData.picklesPicked += (ulong) PlayerData.pickleData.picklesPerClick;

            if (PlayerData.pickleData.pickleLevel < ProgressController.maxLevel)
            {
                PlayerData.pickleData.currentPickleProgress += (1 + clickExperience.amount);
            }

            if (currentSound < pickleSounds.Length - 1)
            {
                currentSound += 1; 
            }
            else
            {
                currentSound = 0;
            }
            audioSource.PlayOneShot(pickleSounds[currentSound]);
            GetHighestAmountOfClicks();
            GetHighestAmountOfPickles();
        }

        //Gets highest amount of pickles achieved
        public static void GetHighestAmountOfPickles()
        {
            if (PlayerData.pickleData.picklesPicked > totalPicklesPicked)
            {
                totalPicklesPicked = PlayerData.pickleData.picklesPicked;
                PlayerData.pickleData.totalPicklesPicked = totalPicklesPicked;
            }
        }

        //Gets total of clicks on the pickle 
        public static void GetHighestAmountOfClicks()
        {
            totalClicks = PlayerData.pickleData.totalClicks;
            totalClicks += 1;
            PlayerData.pickleData.totalClicks = totalClicks;
        }

        //Gets total pickle coins collected
        public static void GetHighestAmountOfCoins()
        {
            if (PlayerData.buyableData.pickleCoins > totalCoinsCollected)
            {
                totalCoinsCollected = PlayerData.buyableData.pickleCoins;
                PlayerData.pickleData.totalCoinsCollected = totalCoinsCollected;
            }
        }
        
        //Show how many pickles per click
        public void EmitValue()
        {
            string pickles = PlayerData.pickleData.picklesPerClick.ToString("N0");
            GameObject valueClone = Instantiate(pickleValue);
            valueClone.name = "Value";
            valueClone.transform.SetParent(transform);
            valueClone.GetComponent<TextMesh>().text = $"+ {pickles}";
            StartCoroutine(StartPopUp(valueClone));
        }

        IEnumerator StartPopUp(GameObject clone)
        {
            clone.transform.position = new Vector3(Random.Range(-1f, 2f), Random.Range(-0.5f, 1.5f), 0);
            clone.GetComponent<Animator>().SetTrigger("Activate");
            yield return new WaitForSeconds(1f);
            Destroy(clone);
        }

        //Level rewards
        private void CheckLevelProgress()
        {
            int levelRequired = PlayerData.pickleData.pickleLevel + 1;
            if (PlayerData.pickleData.pickleLevel == levelRequired)
            {   
                PlayerData.pickleData.pickleLevel++;
                PlayerData.pickleData.picklesPicked += (ulong) (Random.Range(25,100) * PlayerData.pickleData.pickleLevel);
                
            }
            pickleLevelText.text = $"{PlayerData.pickleData.pickleLevel}";
        }
    }
}
