using PickleClicker.Manager;
using PickleClicker.Controller;
using PickleClicker.Controller.Pickle;
using PickleClicker.Manager.Pickle;
using PickleClicker.Manager.Upgrade;
using PickleClicker.Manager.Auto;
using PickleClicker.Tracker;
using PickleClicker.Data.Auto;
using PickleClicker.Data.Upgrade;
using PickleClicker.Data.Poglin;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEngine;

namespace PickleClicker.Data.Player
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField] 
        private MeshRenderer textPopup;

        private string currentMode = "";
        
        [SerializeField] 
        private CanvasManager canvasManager;

        static readonly string PICKLE_SAVE_FILE = "pickle.dat";
        static readonly string AUTO_SAVE_FILE = "autoPickle.dat";
        static readonly string UPGRADE_SAVE_FILE = "upgradePickle.dat";
        static readonly string POGLIN_SAVE_FILE = "poglin.dat";
        static readonly string BUYABLE_SAVE_FILE = "buyable.dat";

        static readonly string PICKLE_KEY = "ShardLikesToPickPickleSmickles!?";

        public static PickleData pickleData = new PickleData();
        public static List<AutoData> autoDataList = new List<AutoData>();
        public static List<UpgradeCategoryData> upgradeCategoryDataList = new List<UpgradeCategoryData>();
        public static List<PoglinData> poglinDataList = new List<PoglinData>();
        public static BuyableData buyableData = new BuyableData();
        public static LocalData localData = new LocalData();

        [SerializeField] 
        private AutoManager autoManager;

        [SerializeField] 
        private UpgradeCategoryManager upgradeCategoryManager;

        [SerializeField] 
        private BombController bombController;

        [SerializeField] 
        private ProgressController progressController;

        [SerializeField] 
        private PickleObjectManager pickleObjectManager;

        private void Awake() 
        {
            AppendPoglinVariants();
            textPopup.GetComponent<Renderer>().sortingOrder = 4; 
        }

        private void Start() 
        {
            if (File.Exists(Application.persistentDataPath + "/LocalData.json"))
            {
                LoadLocalData();
            } 
            LoadGame(); 

            StartCoroutine(AutoSave());

            canvasManager = GameObject.FindObjectOfType<CanvasManager>();
            autoManager = GameObject.FindObjectOfType<AutoManager>();
            upgradeCategoryManager = GameObject.FindObjectOfType<UpgradeCategoryManager>();
            bombController = GameObject.FindObjectOfType<BombController>();
            progressController = GameObject.FindObjectOfType<ProgressController>();
            pickleObjectManager = GameObject.FindObjectOfType<PickleObjectManager>();
        }

        public void AppendPoglinVariants()
        {
            poglinDataList.Add(new PoglinData(0, "Normal Poglin"));
            poglinDataList.Add(new PoglinData(1, "Pink Poglin"));
            poglinDataList.Add(new PoglinData(2, "Gold Poglin"));
            poglinDataList.Add(new PoglinData(3, "Fire Poglin"));
            poglinDataList.Add(new PoglinData(4, "Earth Poglin"));
            poglinDataList.Add(new PoglinData(5, "Water Poglin"));
        }

        //Auto saves every minute
        public IEnumerator AutoSave()
        {
            while (true)
            {
                yield return new WaitForSeconds(60f);
                SaveGame("Game Saved!");
            }
        }

        //Used for the save button
        public void SaveGame(string gameState)
        {
            Debug.Log("Saved the game!");
            string gameData = JsonUtility.ToJson(pickleData);
            string autoData = JsonUtility.ToJson(autoDataList);
            string upgradeData = JsonUtility.ToJson(upgradeCategoryDataList);
            string poglinData = JsonUtility.ToJson(poglinDataList);
            string itemData = JsonUtility.ToJson(buyableData);

            SteamRijndael crypto = new SteamRijndael();
            byte[] pickleSoup = crypto.Encrypt(gameData, PICKLE_KEY);
            byte[] autoSoup = crypto.Encrypt(autoData, PICKLE_KEY);
            byte[] upgradeSoup = crypto.Encrypt(upgradeData, PICKLE_KEY);
            byte[] poglinSoup = crypto.Encrypt(poglinData, PICKLE_KEY);
            byte[] buyableSoup = crypto.Encrypt(itemData, PICKLE_KEY);

            string directoryPath = Application.persistentDataPath + "/PickleSmickle/";

            string pickleFileName = directoryPath + PICKLE_SAVE_FILE;
            string autoFileName = directoryPath + AUTO_SAVE_FILE;
            string upgradeFileName = directoryPath + UPGRADE_SAVE_FILE;
            string poglinFileName = directoryPath + POGLIN_SAVE_FILE;
            string buyableFileName = directoryPath + BUYABLE_SAVE_FILE;

            if (File.Exists(pickleFileName)) File.Delete(pickleFileName);
            if (File.Exists(autoFileName)) File.Delete(autoFileName);
            if (File.Exists(upgradeFileName)) File.Delete(upgradeFileName);
            if (File.Exists(poglinFileName)) File.Delete(poglinFileName);
            if (File.Exists(buyableFileName)) File.Delete(buyableFileName);

            File.WriteAllBytes(pickleFileName, pickleSoup);
            File.WriteAllBytes(autoFileName, autoSoup);
            File.WriteAllBytes(upgradeFileName, upgradeSoup);
            File.WriteAllBytes(poglinFileName, poglinSoup);
            File.WriteAllBytes(buyableFileName, buyableSoup);

            SaveUserData();
            StartCoroutine(GameState(gameState));
        }

        public void LoadGame()
        {   
            StartCoroutine(GameState("Game Loaded!"));
            Debug.Log("Loading game...");
            string directoryPath = Application.persistentDataPath + "/PickleSmickle/";

            string pickleFileName = directoryPath + PICKLE_SAVE_FILE;
            string autoFileName = directoryPath + AUTO_SAVE_FILE;
            string upgradeFileName = directoryPath + UPGRADE_SAVE_FILE;
            string poglinFileName = directoryPath + POGLIN_SAVE_FILE;
            string buyableFileName = directoryPath + BUYABLE_SAVE_FILE;

            byte[] pickleSoupBackIn = File.ReadAllBytes(pickleFileName);
            byte[] autoSoupBackIn = File.ReadAllBytes(autoFileName);
            byte[] upgradeSoupBackIn = File.ReadAllBytes(upgradeFileName);
            byte[] poglinSoupBackIn = File.ReadAllBytes(poglinFileName);
            byte[] buyableSoupBackIn = File.ReadAllBytes(buyableFileName);

            SteamRijndael crypto = new SteamRijndael();
            string pickleJson = crypto.Decrypt(pickleSoupBackIn, PICKLE_KEY);
            string autoJson = crypto.Decrypt(autoSoupBackIn, PICKLE_KEY);
            string upgradeCategoryJson = crypto.Decrypt(upgradeSoupBackIn, PICKLE_KEY);
            string poglinJson = crypto.Decrypt(poglinSoupBackIn, PICKLE_KEY);
            string buyableJson = crypto.Decrypt(buyableSoupBackIn, PICKLE_KEY);

            pickleData = JsonUtility.FromJson<PickleData>(pickleJson);
            autoDataList = JsonUtility.FromJson<List<AutoData>>(autoJson);
            upgradeCategoryDataList = JsonUtility.FromJson<List<UpgradeCategoryData>>(upgradeCategoryJson);
            poglinDataList = JsonUtility.FromJson<List<PoglinData>>(poglinJson);
            buyableData = JsonUtility.FromJson<BuyableData>(buyableJson);

            UpgradeCategoryData bombCategory = upgradeCategoryDataList.Find(category => category.id == 3);
            if (bombCategory == null) return;
            UpgradeData bombCount = bombCategory.upgrades.Find(upgrade => upgrade.id == 2);
            
            bombController.GetBombSlot(bombCount.amount + 3);
            bombController.AppendBombsOnLoad();

            Debug.Log(GameObject.FindObjectOfType<ProgressController>());

            progressController.GetCurrentFill();
            progressController.CheckProgress();
        }

        public void SaveUserData()
        {
            string localDataJson = JsonUtility.ToJson(localData, true);
            File.WriteAllText(Application.persistentDataPath + "/LocalData.json", localDataJson);
        }

        public void LoadLocalData()
        {
            string localDataFile = File.ReadAllText(Application.persistentDataPath + "/LocalData.json");
            LocalData localDataJson = JsonUtility.FromJson<LocalData>(localDataFile);

            localData.musicVolume = localDataJson.musicVolume;
            localData.soundVolume = localDataJson.soundVolume;
        }

        //Sends out a message on a save, load, delete, and transfer state
        IEnumerator GameState(string text)
        {
            textPopup.GetComponent<TextMesh>().text = text;
            textPopup.enabled = true;
            yield return new WaitForSeconds(3);
            textPopup.enabled = false;
        }

        //Confirms either delete or exit
        public void Confirm(bool value)
        {
            Debug.Log($"Current Mode: {currentMode}");
            Debug.Log($"Current Value: {value}");

            if (!value) return;
            
            if (currentMode == "Delete")
            {
                Debug.Log("Deleting game...");
                DeleteGame();
                return;
            } 
            else if (currentMode == "Exit")
            {
                Debug.Log("Exiting game...");
                ExitGame();
                return;
            }
        }

        public void DeleteMode()
        {
            currentMode = "Delete";
        }

        //Deletes the save file
        public void DeleteGame()
        {
            if (currentMode != "Delete") return;

            string directoryPath = Application.persistentDataPath + "/PickleSmickle/";

            string pickleFileName = directoryPath + PICKLE_SAVE_FILE;
            string autoFileName = directoryPath + AUTO_SAVE_FILE;
            string upgradeFileName = directoryPath + UPGRADE_SAVE_FILE;
            string poglinFileName = directoryPath + POGLIN_SAVE_FILE;

            if (File.Exists(pickleFileName)) File.Delete(pickleFileName);
            if (File.Exists(autoFileName)) File.Delete(autoFileName);
            if (File.Exists(upgradeFileName)) File.Delete(upgradeFileName);
            if (File.Exists(poglinFileName)) File.Delete(poglinFileName);

            pickleData = new PickleData();
            autoDataList = new List<AutoData>();
            upgradeCategoryDataList = new List<UpgradeCategoryData>();
            poglinDataList = new List<PoglinData>();

            autoManager.SetAutoBuyables();
            upgradeCategoryManager.SetUpgradeCategories();

            AppendPoglinVariants();

            TimeTracker.seconds = 0; 
            TimeTracker.minutes = 0;
            TimeTracker.hours = 0; 

            SaveGame("Game Deleted!");

            currentMode = "";
        }

        public void ExitMode()
        {
            currentMode = "Exit";
        }

        //Exits application
        public void ExitGame()
        {
            if (currentMode != "Exit") return;

            Debug.Log("Exiting game...");

            SaveGame("Game Saved!");
            currentMode = "";
            Application.Quit();
        }
    }
}