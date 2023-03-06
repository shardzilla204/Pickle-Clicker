using PickleClicker.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Poglin
{

    public class PickleBombSpawner : MonoBehaviour
    {
        [SerializeField] private Text countText;
        [SerializeField] private Text purchaseText;

        [SerializeField] private GameObject mainCanvas;
        [SerializeField] private GameObject pickleBombSlot;
        [SerializeField] private Transform pickleBombSlots;
        public GameObject pickleBomb;
        [SerializeField] private float distance = 15f;
        [SerializeField] private List<float> cosineAngles = new List<float>();
        [SerializeField] private List<float> sineAngles = new List<float>();
        [SerializeField] private List<Vector2> angles = new List<Vector2>();
        public List<GameObject> spots = new List<GameObject>();

        public int extraBombs = 0;

        private void Update() 
        {
            if (PlayerData.pickleData.currentPickleBombCount < 0) PlayerData.pickleData.currentPickleBombCount = 0;

            UpgradeCategoryData bombCategory = PlayerData.upgradeList.upgradeCategories.Find(category => category.id == 3);
            UpgradeData bombCount = bombCategory.upgradeBuyables.Find(upgrade => upgrade.id == 2);
            
            if (bombCount.amount != extraBombs) GainExtraBombSlot();

            countText.text = $"{PlayerData.pickleData.currentPickleBombCount}/{PlayerData.pickleData.maxPickleBombCount}";

            if (PlayerData.pickleData.currentPickleBombCount == 0) ClearBombSlot();

            purchaseText.text = $"{PlayerData.pickleData.pickleBombCost}\nPickles";
            
            if (PlayerData.pickleData.currentPickleBombCount < PlayerData.pickleData.maxPickleBombCount) return;
            
            purchaseText.text = $"Max";
        }

        private void Start() 
        {   
            CreateBombSlots(3);
            AddBombSlots();
            angles.Clear(); 
        }

        public void CreateBomb()
        {
            if (PlayerData.pickleData.currentPickleBombCount >= PlayerData.pickleData.maxPickleBombCount || PlayerData.pickleData.picklesPicked < (ulong) PlayerData.pickleData.pickleBombCost) return;

            PlayerData.pickleData.picklesPicked -= (ulong) PlayerData.pickleData.pickleBombCost;

            GameObject pickleBombClone = Instantiate(pickleBomb);
            pickleBombClone.transform.position = spots[PlayerData.pickleData.currentPickleBombCount].transform.position;
            pickleBombClone.transform.SetParent(spots[PlayerData.pickleData.currentPickleBombCount].transform);
            pickleBombClone.name = "PickleBomb";
            PlayerData.pickleData.currentPickleBombCount++;
            ChangePrice(PlayerData.pickleData.currentPickleBombCount);
        }

        public void GainExtraBombSlot()
        {
            PlayerData.pickleData.maxPickleBombCount++;
            extraBombs++;
            GetBombSlot(PlayerData.pickleData.maxPickleBombCount);
        }

        public void CreateBombSlots(int amount)
        {
            for (int bombCount = 0; bombCount < amount; ++bombCount)
            {
                float theta = (2 * Mathf.PI / amount) * bombCount;
                angles.Add(new Vector3(Mathf.Cos(theta) * distance, Mathf.Sin(theta) * distance, 0));
            }
        }

        public void AddBombSlots()
        {
            for (int i = 0; i < angles.Count; i++)
            {
                GameObject spot = Instantiate(pickleBombSlot);
                spot.transform.SetParent(pickleBombSlots.transform);
                spot.transform.position = angles[i];
                spot.name = "PickleBombSpot";
                spot.transform.localScale = new Vector2(1f, 1f);
                spots.Add(spot);
            }
        }

        public void GetBombSlot(int amount)
        {
            ClearBombSlots();

            CreateBombSlots(amount);

            AddBombSlots();

            angles.Clear(); 

            if (PlayerData.pickleData.currentPickleBombCount == 0) return;

            for (int i = 0; i < PlayerData.pickleData.currentPickleBombCount; i++)
            {
                GameObject pickleBombClone = Instantiate(pickleBomb);
                pickleBombClone.transform.position = spots[i].transform.position;
                pickleBombClone.transform.SetParent(spots[i].transform);
                pickleBombClone.name = "PickleBomb";
            }
        }

        public void RemoveBombCount(PickleBombSpawner pickleBombSpawner)
        {
            StartCoroutine(WaitForAnimation(pickleBombSpawner));
        }

        IEnumerator WaitForAnimation(PickleBombSpawner pickleBombSpawner)
        {
            yield return new WaitForSeconds(5f);
            PlayerData.pickleData.currentPickleBombCount--;
            pickleBombSpawner.ChangePrice(PlayerData.pickleData.currentPickleBombCount);
        }

        public void ChangePrice(int bombCount)
        {
            if (bombCount == 0) ClearBombSlots();

            long newCost = 3000;

            for (int append = 0; append < bombCount; append++)
            {
                newCost = newCost + (250 * bombCount);
            }

            PlayerData.pickleData.pickleBombCost = newCost;
        }

        public void ClearBombSlots()
        {
            foreach (Transform child in pickleBombSlots.transform)
            {
                Destroy(child.gameObject);
                spots.Clear();
            }
        }

        public void ClearBombSlot()
        {
            foreach (Transform child in pickleBombSlots.transform)
            {
                if (child.childCount == 0) return;
                Destroy(child.GetChild(0).gameObject);
            }
        }
    }
}
