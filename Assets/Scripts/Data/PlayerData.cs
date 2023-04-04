using PickleClicker.CanvasScripts;
using PickleClicker.Controller;
using PickleClicker.Pickle;
using PickleClicker.Poglin;
using PickleClicker.Tracker;
using System.Collections;
using System.IO;
using UnityEngine;

namespace PickleClicker.Data
{
    public class PlayerData : MonoBehaviour
    {
        [SerializeField] private MeshRenderer textPopup;
        private string currentMode = "";
        [SerializeField] private CanvasManager canvasManager;

        static readonly string PICKLE_SAVE_FILE = "pickle.dat";
        static readonly string AUTO_SAVE_FILE = "autoPickle.dat";
        static readonly string UPGRADE_SAVE_FILE = "upgradePickle.dat";
        static readonly string POGLIN_SAVE_FILE = "poglin.dat";
        static readonly string BUYABLE_SAVE_FILE = "buyable.dat";

        static readonly string PICKLE_KEY = "ShardLikesToPickPickleSmickles!?";

        public static PickleData pickleData = new PickleData();
        public static AutoList autoList = new AutoList();
        public static UpgradeList upgradeList = new UpgradeList();
        public static PoglinList poglinList = new PoglinList();
        public static BuyableData buyableData = new BuyableData();
        public static UserData userData = new UserData();

        [SerializeField] private AutoBuyableController autoBuyableController;
        [SerializeField] private UpgradeCategoryController upgradeCategoryController;

        [SerializeField] private PickleBombSpawner pickleBombSpawner;

        private void Awake() 
        {
            AppendPoglinVariants();
            textPopup.GetComponent<Renderer>().sortingOrder = 4; 
        }

        private void Start() 
        {
            if (File.Exists(Application.persistentDataPath + "/UserData.json"))
            {
                LoadUserData();
            } 
            else
            {
                SaveUserData();
                userData.transferredToV109 = true;
            }

            if (!userData.transferredToV109)
            {
                Debug.Log("Transferring Data...");
                TransferUserData();
            }
            else
            {
                LoadGame(); 
            }
            StartCoroutine(AutoSave());
        }

        public void AppendPoglinVariants()
        {
            poglinList.poglinVariants.Add(new PoglinVariantData(0, "Normal Poglin"));
            poglinList.poglinVariants.Add(new PoglinVariantData(1, "Pink Poglin"));
            poglinList.poglinVariants.Add(new PoglinVariantData(2, "Gold Poglin"));
            poglinList.poglinVariants.Add(new PoglinVariantData(3, "Fire Poglin"));
            poglinList.poglinVariants.Add(new PoglinVariantData(4, "Earth Poglin"));
            poglinList.poglinVariants.Add(new PoglinVariantData(5, "Water Poglin"));
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
            string autoData = JsonUtility.ToJson(autoList);
            string upgradeData = JsonUtility.ToJson(upgradeList);
            string poglinData = JsonUtility.ToJson(poglinList);
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
            string upgradeJson = crypto.Decrypt(upgradeSoupBackIn, PICKLE_KEY);
            string poglinJson = crypto.Decrypt(poglinSoupBackIn, PICKLE_KEY);
            string buyableJson = crypto.Decrypt(buyableSoupBackIn, PICKLE_KEY);

            pickleData = JsonUtility.FromJson<PickleData>(pickleJson);
            autoList = JsonUtility.FromJson<AutoList>(autoJson);
            upgradeList = JsonUtility.FromJson<UpgradeList>(upgradeJson);
            poglinList = JsonUtility.FromJson<PoglinList>(poglinJson);
            buyableData = JsonUtility.FromJson<BuyableData>(buyableJson);

            UpgradeCategoryData bombCategory = upgradeList.upgradeCategories.Find(category => category.id == 3);
            UpgradeData bombCount = bombCategory.upgradeBuyables.Find(upgrade => upgrade.id == 2);
            
            pickleBombSpawner.GetBombSlot(bombCount.amount + 3);
            pickleBombSpawner.AppendBombsOnLoad();
        }

        public void SaveUserData()
        {
            string settingData = JsonUtility.ToJson(userData, true);
            File.WriteAllText(Application.persistentDataPath + "/UserData.json", settingData);
        }

        //Transfers the old game data to the new game data
        public void TransferUserData()
        {
            userData.transferredToV109 = true;
            string settingData = JsonUtility.ToJson(userData, true);
            File.WriteAllText(Application.persistentDataPath + "/UserData.json", settingData);

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
            string upgradeJson = crypto.Decrypt(upgradeSoupBackIn, PICKLE_KEY);
            string poglinJson = crypto.Decrypt(poglinSoupBackIn, PICKLE_KEY);
            string buyableJson = crypto.Decrypt(buyableSoupBackIn, PICKLE_KEY);

            PickleData pickleData = JsonUtility.FromJson<PickleData>(pickleJson);
            AutoList autoData = JsonUtility.FromJson<AutoList>(autoJson);
            OldUpgradeList upgradeData = JsonUtility.FromJson<OldUpgradeList>(upgradeJson);
            PoglinList poglinData = JsonUtility.FromJson<PoglinList>(poglinJson);
            BuyableData buyableData = JsonUtility.FromJson<BuyableData>(buyableJson);

            PlayerData.pickleData = pickleData;
            autoList.autoBuyables = autoData.autos;
            foreach (AutoData auto in autoList.autoBuyables)
            {
                auto.maxAmount = 100;
            }

            UpgradeCategoryData clickCategory = upgradeList.upgradeCategories.Find(category => category.id == 0);
            UpgradeCategoryData jarCategory = upgradeList.upgradeCategories.Find(category => category.id == 1);
            UpgradeCategoryData poglinCategory = upgradeList.upgradeCategories.Find(category => category.id == 2);

            UpgradeData increaseClick = clickCategory.upgradeBuyables.Find(buyable => buyable.id == 0);
            UpgradeData jarSpawnrate = jarCategory.upgradeBuyables.Find(buyable => buyable.id == 0);
            UpgradeData sharperSpear = clickCategory.upgradeBuyables.Find(buyable => buyable.id == 1);
            UpgradeData tougherPickle = poglinCategory.upgradeBuyables.Find(buyable => buyable.id == 0);
            
            increaseClick.amount = upgradeData.upgrades.Find(upgrade => upgrade.id == 0).amount;
            jarSpawnrate.amount = upgradeData.upgrades.Find(upgrade => upgrade.id == 1).amount;
            sharperSpear.amount = upgradeData.upgrades.Find(upgrade => upgrade.id == 2).amount;
            tougherPickle.amount = upgradeData.upgrades.Find(upgrade => upgrade.id == 3).amount;
            
            PlayerData.poglinList = poglinData;
            PlayerData.buyableData = buyableData;

            SaveGame("Game Transferred!");
        }

        public void LoadUserData()
        {
            string settingData = File.ReadAllText(Application.persistentDataPath + "/UserData.json");
            UserData playerData = JsonUtility.FromJson<UserData>(settingData);

            userData.musicVolume = playerData.musicVolume;
            userData.soundVolume = playerData.soundVolume;
            userData.transferredToV109 = playerData.transferredToV109;
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
            autoList = new AutoList();
            upgradeList = new UpgradeList();
            poglinList = new PoglinList();

            autoBuyableController.SetAutoBuyables();
            upgradeCategoryController.SetUpgradeCategories();

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