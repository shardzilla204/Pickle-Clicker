using PickleClicker.Controller;
using PickleClicker.Data.Player;
using PickleClicker.Data.Upgrade;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

namespace PickleClicker.Controller.Pickle
{
    public class PickleController : MonoBehaviour
    {
        [SerializeField] private Text pickleLevelText;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] pickleSounds;
        [SerializeField] private GameObject pickleValue;
        [SerializeField] private GameObject pickleButton;

        private int currentSound = 0;

        public static double totalPicklesPicked;
        public static double totalCoinsCollected;
        public static double totalClicks;

        private void Awake() 
        {
            totalPicklesPicked = PlayerData.pickleData.totalPickles;
            totalCoinsCollected = PlayerData.pickleData.totalCoins;
            totalClicks = PlayerData.pickleData.totalClicks;
        }

        //Adds pickles every click 
        public void PickleClick()
        {
            foreach(UpgradeCategoryData upgradeCategoryData in PlayerData.upgradeCategoryDataList)
            {
                Debug.Log(upgradeCategoryData);
            }
            UpgradeCategoryData clickCategory = PlayerData.upgradeCategoryDataList.Find(category => category.id == 0);
            UpgradeData increaseClick = clickCategory.upgrades.Find(upgrade => upgrade.id == 0);
            UpgradeData clickExperience = clickCategory.upgrades.Find(upgrade => upgrade.id == 2);
            
            double pickleLevel = PlayerData.pickleData.level;
            double division = (pickleLevel / 5);
            if (increaseClick.amount == 0 || division < 1)
            {
                PlayerData.pickleData.gainPerClick = 1;
            }
            else 
            {
                PlayerData.pickleData.gainPerClick = (division * (increaseClick.amount + 1));
            }

            PlayerData.pickleData.pickles += PlayerData.pickleData.gainPerClick;

            if (PlayerData.pickleData.level < ProgressController.maxLevel) PlayerData.pickleData.currentProgress += (1 + clickExperience.amount);

            if (currentSound < pickleSounds.Length - 1)
            {
                currentSound++; 
            }
            else
            {
                currentSound = 0;
            }
            audioSource.PlayOneShot(pickleSounds[currentSound]);
            GetHighestAmountOfClicks();
            GetHighestAmountOfPickles();

            GameObject.FindObjectOfType<ProgressController>().GetCurrentFill();
            GameObject.FindObjectOfType<ProgressController>().CheckProgress();
        }

        //Gets highest amount of pickles achieved
        public static void GetHighestAmountOfPickles()
        {
            totalPicklesPicked = PlayerData.pickleData.pickles;
            PlayerData.pickleData.totalPickles = totalPicklesPicked;
        }

        //Gets total of clicks on the pickleData 
        public static void GetHighestAmountOfClicks()
        {
            totalClicks = PlayerData.pickleData.totalClicks;
            totalClicks++;
            PlayerData.pickleData.totalClicks = totalClicks;
        }

        //Gets total pickleData coins collected
        public static void GetHighestAmountOfCoins()
        {   
            totalCoinsCollected = PlayerData.buyableData.pickleCoins;
            PlayerData.pickleData.totalCoins = totalCoinsCollected;
        }
        
        // Displaying numbers
        public void EmitValue()
        {
            string pickles = PlayerData.pickleData.gainPerClick.ToString("N0");
            GameObject valueClone = Instantiate(pickleValue);
            valueClone.name = "Value";
            valueClone.transform.SetParent(transform);
            valueClone.GetComponent<TextMesh>().text = $"+ {pickles}";
            valueClone.GetComponent<TextMesh>().color = Color.white;

            if (ProgressController.pickleLevelMax) valueClone.GetComponent<TextMesh>().color = Color.yellow;

            valueClone.transform.position = new Vector3(Random.Range(-1f, 2f), Random.Range(-0.5f, 1.5f), 0);
            valueClone.GetComponent<Animator>().SetTrigger("Activate");
        }
    }
}
