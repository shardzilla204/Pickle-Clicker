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
        public bool deleting = false;
        private bool confirm = false;
        [SerializeField] private CanvasManager canvasManager;

        static readonly string PICKLE_SAVE_FILE = "pickle.dat";
        static readonly string AUTO_SAVE_FILE = "autoPickle.dat";
        static readonly string UPGRADE_SAVE_FILE = "upgradePickle.dat";
        static readonly string POGLIN_SAVE_FILE = "poglin.dat";
        static readonly string BUYABLE_SAVE_FILE = "buyable.dat";

        static readonly string PICKLE_SAVE_FILE_EXTRA = "pickleExtra.dat";
        static readonly string AUTO_SAVE_FILE_EXTRA = "autoPickleExtra.dat";
        static readonly string UPGRADE_SAVE_FILE_EXTRA = "upgradePickleExtra.dat";
        static readonly string POGLIN_SAVE_FILE_EXTRA = "poglinExtra.dat";
        static readonly string BUYABLE_SAVE_FILE_EXTRA = "buyableExtra.dat";

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
            //Sets the poglin data
            poglinList.poglinVariants.Add(new PoglinVariantData(0, "Normal Poglin"));
            poglinList.poglinVariants.Add(new PoglinVariantData(1, "Pink Poglin"));
            poglinList.poglinVariants.Add(new PoglinVariantData(2, "Gold Poglin"));
            poglinList.poglinVariants.Add(new PoglinVariantData(3, "Fire Poglin"));
            poglinList.poglinVariants.Add(new PoglinVariantData(4, "Earth Poglin"));
            poglinList.poglinVariants.Add(new PoglinVariantData(5, "Water Poglin"));

            textPopup.GetComponent<Renderer>().sortingOrder = 4; 

            DecryptAndSaveMain();
            DecryptAndSaveSub();
        }

        private void Start() 
        {
            LoadUserData();
            if (!userData.transferredToV109)
            {
                SaveUserData();
                Debug.Log("Transferring Data...");
                TransferUserData();
            }
            LoadGameOnce();
            StartCoroutine(AutoSave());
        }

        //Auto saves every minute
        public IEnumerator AutoSave()
        {
            while (true)
            {
                yield return new WaitForSeconds(60f);
                SaveGameOnce();
                LoadGameOnce();
            }
        }

        //Saves the data to a JSON file
        public void SaveGameFromPlayFab()
        {
            Debug.Log("Saved the game!");
            string gameData = JsonUtility.ToJson(pickleData);
            string autoData = JsonUtility.ToJson(autoList);
            string upgradeData = JsonUtility.ToJson(upgradeList);
            string poglinData = JsonUtility.ToJson(poglinList);
            string itemData = JsonUtility.ToJson(buyableData);

            SaveUserData();
            StartCoroutine(GameState("Game Saved!"));
        }

        //Used for the save button
        public void SaveGameOnce()
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

            // File.WriteAllText(pickleFileName, gameData);
            // File.WriteAllText(autoFileName, autoData);
            // File.WriteAllText(upgradeFileName, upgradeData);
            // File.WriteAllText(poglinFileName, poglinData);
            // File.WriteAllText(buyableFileName, itemData);

            SaveUserData();
            StartCoroutine(GameState("Game Saved!"));
        }

        //Loads the data saved in the PlayFab
        public void LoadGameOnLogin(string gameData, string autoData, string upgradeData, string poglinData, string buyableData)
        {
            Load(gameData, autoData, upgradeData, poglinData, buyableData);
        }

        //Used for the load button
        public void LoadGameOnceFromPlayFab(string gameData, string autoData, string upgradeData, string poglinData, string buyableData)
        {   
            StartCoroutine(GameState("Game Loaded!"));
            Load(gameData, autoData, upgradeData, poglinData, buyableData);
        }

        public void LoadGameOnce()
        {   
            string directoryPath = Application.persistentDataPath + "/PickleSmickle/";

            string pickleFileName = directoryPath + PICKLE_SAVE_FILE;
            string autoFileName = directoryPath + AUTO_SAVE_FILE;
            string upgradeFileName = directoryPath + UPGRADE_SAVE_FILE;
            string poglinFileName = directoryPath + POGLIN_SAVE_FILE;
            string buyableFileName = directoryPath + BUYABLE_SAVE_FILE;

            // string pickleJson = File.ReadAllText(pickleFileName);
            // string autoJson = File.ReadAllText(autoFileName);
            // string upgradeJson = File.ReadAllText(upgradeFileName);
            // string poglinJson = File.ReadAllText(poglinFileName);
            // string buyableJson = File.ReadAllText(buyableFileName);

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

            UpgradeCategoryData bombCategory = PlayerData.upgradeList.upgradeCategories.Find(category => category.id == 3);
            UpgradeData bombCount = bombCategory.upgradeBuyables.Find(upgrade => upgrade.id == 2);
            
            pickleBombSpawner.CreateBombSlots(bombCount.amount + 3);

            StartCoroutine(GameState("Game Loaded!"));
        }

        public void DecryptAndSaveMain()
        {
            string directoryPath = Application.persistentDataPath + "/PickleSmickle/";

            string pickleFileName = directoryPath + PICKLE_SAVE_FILE;
            string autoFileName = directoryPath + AUTO_SAVE_FILE;
            string upgradeFileName = directoryPath + UPGRADE_SAVE_FILE;
            string poglinFileName = directoryPath + POGLIN_SAVE_FILE;
            string buyableFileName = directoryPath + BUYABLE_SAVE_FILE;

            string directoryPathExtra = Application.persistentDataPath + "/PickleSmickleExtra/";

            string pickleFileNameExtra = directoryPath + PICKLE_SAVE_FILE_EXTRA;
            string autoFileNameExtra = directoryPath + AUTO_SAVE_FILE_EXTRA;
            string upgradeFileNameExtra = directoryPath + UPGRADE_SAVE_FILE_EXTRA;
            string poglinFileNameExtra = directoryPath + POGLIN_SAVE_FILE_EXTRA;
            string buyableFileNameExtra = directoryPath + BUYABLE_SAVE_FILE_EXTRA;

            byte[] pickleSoupBackIn = File.ReadAllBytes(pickleFileName);
            byte[] autoSoupBackIn = File.ReadAllBytes(autoFileName);
            byte[] upgradeSoupBackIn = File.ReadAllBytes(upgradeFileName);
            byte[] poglinSoupBackIn = File.ReadAllBytes(poglinFileName);
            byte[] buyableSoupBackIn = File.ReadAllBytes(buyableFileName);

            SteamRijndael crypto = new SteamRijndael();
            string gameData = crypto.Decrypt(pickleSoupBackIn, PICKLE_KEY);
            string autoData = crypto.Decrypt(autoSoupBackIn, PICKLE_KEY);
            string upgradeData = crypto.Decrypt(upgradeSoupBackIn, PICKLE_KEY);
            string poglinData = crypto.Decrypt(poglinSoupBackIn, PICKLE_KEY);
            string itemData = crypto.Decrypt(buyableSoupBackIn, PICKLE_KEY);

            File.WriteAllText(pickleFileNameExtra, gameData);
            File.WriteAllText(autoFileNameExtra, autoData);
            File.WriteAllText(upgradeFileNameExtra, upgradeData);
            File.WriteAllText(poglinFileNameExtra, poglinData);
            File.WriteAllText(buyableFileNameExtra, itemData);
        }

        public void DecryptAndSaveSub()
        {
            string directoryPath = Application.persistentDataPath + "/SmicklePickle/";

            string pickleFileName = directoryPath + PICKLE_SAVE_FILE;
            string autoFileName = directoryPath + AUTO_SAVE_FILE;
            string upgradeFileName = directoryPath + UPGRADE_SAVE_FILE;
            string poglinFileName = directoryPath + POGLIN_SAVE_FILE;
            string buyableFileName = directoryPath + BUYABLE_SAVE_FILE;

            string directoryPathExtra = Application.persistentDataPath + "/SmicklePickleExtra/";

            string pickleFileNameExtra = directoryPath + PICKLE_SAVE_FILE_EXTRA;
            string autoFileNameExtra = directoryPath + AUTO_SAVE_FILE_EXTRA;
            string upgradeFileNameExtra = directoryPath + UPGRADE_SAVE_FILE_EXTRA;
            string poglinFileNameExtra = directoryPath + POGLIN_SAVE_FILE_EXTRA;
            string buyableFileNameExtra = directoryPath + BUYABLE_SAVE_FILE_EXTRA;

            byte[] pickleSoupBackIn = File.ReadAllBytes(pickleFileName);
            byte[] autoSoupBackIn = File.ReadAllBytes(autoFileName);
            byte[] upgradeSoupBackIn = File.ReadAllBytes(upgradeFileName);
            byte[] poglinSoupBackIn = File.ReadAllBytes(poglinFileName);
            byte[] buyableSoupBackIn = File.ReadAllBytes(buyableFileName);

            SteamRijndael crypto = new SteamRijndael();
            string gameData = crypto.Decrypt(pickleSoupBackIn, PICKLE_KEY);
            string autoData = crypto.Decrypt(autoSoupBackIn, PICKLE_KEY);
            string upgradeData = crypto.Decrypt(upgradeSoupBackIn, PICKLE_KEY);
            string poglinData = crypto.Decrypt(poglinSoupBackIn, PICKLE_KEY);
            string itemData = crypto.Decrypt(buyableSoupBackIn, PICKLE_KEY);

            File.WriteAllText(pickleFileNameExtra, gameData);
            File.WriteAllText(autoFileNameExtra, autoData);
            File.WriteAllText(upgradeFileNameExtra, upgradeData);
            File.WriteAllText(poglinFileNameExtra, poglinData);
            File.WriteAllText(buyableFileNameExtra, itemData);
        }

        public void Load(string gameData, string autoData, string upgradeData, string poglinData, string itemData)
        {   
            Debug.Log("Loaded the game!");
            pickleData = JsonUtility.FromJson<PickleData>(gameData);
            autoList = JsonUtility.FromJson<AutoList>(autoData);
            upgradeList = JsonUtility.FromJson<UpgradeList>(upgradeData);
            poglinList = JsonUtility.FromJson<PoglinList>(poglinData);
            buyableData = JsonUtility.FromJson<BuyableData>(itemData);
            SaveGameOnce();
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
            string extraDirectoryPath = Application.persistentDataPath + "/SmicklePickle/";

            string pickleFileName = directoryPath + PICKLE_SAVE_FILE;
            string autoFileName = directoryPath + AUTO_SAVE_FILE;
            string upgradeFileName = directoryPath + UPGRADE_SAVE_FILE;
            string poglinFileName = directoryPath + POGLIN_SAVE_FILE;
            string buyableFileName = directoryPath + BUYABLE_SAVE_FILE;

            string extraPickleFileName = extraDirectoryPath + PICKLE_SAVE_FILE;
            string extraAutoFileName = extraDirectoryPath + AUTO_SAVE_FILE;
            string extraUpgradeFileName = extraDirectoryPath + UPGRADE_SAVE_FILE;
            string extraPoglinFileName = extraDirectoryPath + POGLIN_SAVE_FILE;
            string extraBuyableFileName = extraDirectoryPath + BUYABLE_SAVE_FILE;

            byte[] pickleSoupBackIn = File.ReadAllBytes(pickleFileName);
            byte[] autoSoupBackIn = File.ReadAllBytes(autoFileName);
            byte[] upgradeSoupBackIn = File.ReadAllBytes(upgradeFileName);
            byte[] poglinSoupBackIn = File.ReadAllBytes(poglinFileName);
            byte[] buyableSoupBackIn = File.ReadAllBytes(buyableFileName);

            byte[] extraPickleSoupBackIn = File.ReadAllBytes(extraPickleFileName);
            byte[] extraAutoSoupBackIn = File.ReadAllBytes(extraAutoFileName);
            byte[] extraUpgradeSoupBackIn = File.ReadAllBytes(extraUpgradeFileName);
            byte[] extraPoglinSoupBackIn = File.ReadAllBytes(extraPoglinFileName);
            byte[] extraBuyableSoupBackIn = File.ReadAllBytes(extraBuyableFileName);

            SteamRijndael crypto = new SteamRijndael();
            string pickleJson = crypto.Decrypt(pickleSoupBackIn, PICKLE_KEY);
            string autoJson = crypto.Decrypt(autoSoupBackIn, PICKLE_KEY);
            string upgradeJson = crypto.Decrypt(upgradeSoupBackIn, PICKLE_KEY);
            string poglinJson = crypto.Decrypt(poglinSoupBackIn, PICKLE_KEY);
            string buyableJson = crypto.Decrypt(buyableSoupBackIn, PICKLE_KEY);

            string extraPickleJson = crypto.Decrypt(extraPickleSoupBackIn, PICKLE_KEY);
            string extraAutoJson = crypto.Decrypt(extraAutoSoupBackIn, PICKLE_KEY);
            string extraUpgradeJson = crypto.Decrypt(extraUpgradeSoupBackIn, PICKLE_KEY);
            string extraPoglinJson = crypto.Decrypt(extraPoglinSoupBackIn, PICKLE_KEY);
            string extraBuyableJson = crypto.Decrypt(extraBuyableSoupBackIn, PICKLE_KEY);

            PickleData pickleData = JsonUtility.FromJson<PickleData>(pickleJson);
            AutoList autoList = JsonUtility.FromJson<AutoList>(autoJson);
            UpgradeList upgradeList = JsonUtility.FromJson<UpgradeList>(upgradeJson);
            PoglinList poglinList = JsonUtility.FromJson<PoglinList>(poglinJson);
            BuyableData buyableData = JsonUtility.FromJson<BuyableData>(buyableJson);

            PickleData extraPickleData = JsonUtility.FromJson<PickleData>(extraPickleJson);
            AutoList extraAutoData = JsonUtility.FromJson<AutoList>(extraAutoJson);
            UpgradeData extraUpgradeData = JsonUtility.FromJson<UpgradeData>(extraUpgradeJson);
            PoglinVariantData extraPoglinData = JsonUtility.FromJson<PoglinVariantData>(extraPoglinJson);
            BuyableData extraBuyableData = JsonUtility.FromJson<BuyableData>(extraBuyableJson);

            pickleData.picklesPicked = extraPickleData.picklesPicked;
            autoList.autoBuyables = extraAutoData.autos;
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
        public void Confirm(int number)
        {
            int value = number;
            switch (currentMode)
            {
                case "Delete":
                
                    canvasManager.SwitchCanvas(CanvasType.Confirm);
                    if (value <= 0)
                    {
                        confirm = false;
                    }
                    else if (value == 1)
                    {
                        confirm = true;
                    }
                    DeleteGame();
                break;
                case "Exit":
                    if (CanvasManager.desiredCanvas.gameObject.name == "LoginConfirmCanvas")
                    {
                        canvasManager.SwitchCanvas(CanvasType.LoginConfirm);
                        Debug.Log("Exited Game from Login");
                    } 
                    else 
                    {
                        canvasManager.SwitchCanvas(CanvasType.Confirm);
                        Debug.Log("Exited Game from GameUI");
                    }
                    if (value <= 0)
                    {
                        confirm = false;
                    }
                    else if (value == 1)
                    {
                        confirm = true;
                    }
                    ExitGame();
                break;
            }
        }

        //Deletes the save file
        public void DeleteGame()
        {
            currentMode = "Delete";
            if (!confirm) return;
            deleting = true;

            pickleData = new PickleData();
            autoList = new AutoList();
            upgradeList = new UpgradeList();
            poglinList = new PoglinList();

            autoBuyableController.SetAutoBuyables();
            upgradeCategoryController.SetUpgradeCategories();

            poglinList.poglinVariants.Add(new PoglinVariantData(0, "Normal Poglin"));
            poglinList.poglinVariants.Add(new PoglinVariantData(1, "Pink Poglin"));
            poglinList.poglinVariants.Add(new PoglinVariantData(2, "Gold Poglin"));
            poglinList.poglinVariants.Add(new PoglinVariantData(3, "Fire Poglin"));
            poglinList.poglinVariants.Add(new PoglinVariantData(4, "Earth Poglin"));
            poglinList.poglinVariants.Add(new PoglinVariantData(5, "Water Poglin"));

            string gameData = JsonUtility.ToJson(pickleData);
            string autoData = JsonUtility.ToJson(autoList);
            string upgradeData = JsonUtility.ToJson(upgradeList);
            string poglinData = JsonUtility.ToJson(poglinList);

            SteamRijndael crypto = new SteamRijndael();
            byte[] pickleSoup = crypto.Encrypt(gameData, PICKLE_KEY);
            byte[] autoSoup = crypto.Encrypt(autoData, PICKLE_KEY);
            byte[] upgradeSoup = crypto.Encrypt(upgradeData, PICKLE_KEY);
            byte[] poglinSoup = crypto.Encrypt(poglinData, PICKLE_KEY);

            string directoryPath = Application.persistentDataPath + "/PickleSmickle/";

            string pickleFileName = directoryPath + PICKLE_SAVE_FILE;
            string autoFileName = directoryPath + AUTO_SAVE_FILE;
            string upgradeFileName = directoryPath + UPGRADE_SAVE_FILE;
            string poglinFileName = directoryPath + POGLIN_SAVE_FILE;

            if (File.Exists(pickleFileName)) File.Delete(pickleFileName);
            if (File.Exists(autoFileName)) File.Delete(autoFileName);
            if (File.Exists(upgradeFileName)) File.Delete(upgradeFileName);
            if (File.Exists(poglinFileName)) File.Delete(poglinFileName);

            File.WriteAllBytes(pickleFileName, pickleSoup);
            File.WriteAllBytes(autoFileName, autoSoup);
            File.WriteAllBytes(upgradeFileName, upgradeSoup);
            File.WriteAllBytes(poglinFileName, poglinSoup);

            TimeTracker.seconds = 0; 
            TimeTracker.minutes = 0;
            TimeTracker.hours = 0; 

            pickleBombSpawner.GetBombSlot(0);

            StartCoroutine(GameState("Game Deleted!"));

            confirm = false;
            deleting = false;
            currentMode = "";

            LoadGameOnce();
        }

        //Exits application
        public void ExitGame()
        {
            currentMode = "Exit";
            if (!confirm) return;

            SaveGameOnce();
            currentMode = "";
            Application.Quit();
        }
    }
}