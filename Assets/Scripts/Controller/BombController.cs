using PickleClicker.Data.Upgrade;
using PickleClicker.Data.Player;
using System.Collections;
using System.Collections.Generic;
using PickleClicker.Game.Pickle;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Controller.Pickle
{

    public class BombController : MonoBehaviour
    {
        [SerializeField] private Text bombCountText;
        [SerializeField] private Text purchaseCostText;

        [SerializeField] private GameObject mainCanvas;
        [SerializeField] private GameObject bombSlotPrefab;
        [SerializeField] private Transform bombSlotsTransform;
        [SerializeField] private GameObject bombPrefab;
        [SerializeField] private float distance = 15f;

        private List<Vector2> angles = new List<Vector2>();
        private List<GameObject> bombSlots = new List<GameObject>();

        private void Update() 
        {
            if (PlayerData.pickleData.currentBombCount < 0) PlayerData.pickleData.currentBombCount = 0;

            UpgradeCategoryData bombCategory = PlayerData.upgradeCategoryDataList.Find(category => category.id == 3);

            if (bombCategory == null) return;
            
            UpgradeData bombCountUpgrade = bombCategory.upgrades.Find(upgrade => upgrade.id == 2);

            purchaseCostText.text = $"{PlayerData.pickleData.bombCost}\nPickles";

            bombCountText.text = $"{PlayerData.pickleData.currentBombCount}/{PlayerData.pickleData.maximumBombCount}";

            if (PlayerData.pickleData.currentBombCount == 0) ClearBombSlot();
            
            if (PlayerData.pickleData.currentBombCount >= PlayerData.pickleData.maximumBombCount) purchaseCostText.text = $"Max";
        }

        private void Start() 
        {   
            GetBombSlot(3);
        }

        public void CreateBomb()
        {
            if (PlayerData.pickleData.currentBombCount >= PlayerData.pickleData.maximumBombCount || PlayerData.pickleData.pickles < (ulong) PlayerData.pickleData.bombCost) return;

            PlayerData.pickleData.pickles -= (ulong) PlayerData.pickleData.bombCost;

            GameObject bombClone = Instantiate(bombPrefab.gameObject);
            foreach (GameObject bombSlot in bombSlots)
            {
                if (bombSlot.transform.childCount != 1)
                {
                    bombClone.transform.position = bombSlot.transform.position;
                    bombClone.transform.SetParent(bombSlot.transform);
                }
            }
            bombClone.name = "Bomb";
            PlayerData.pickleData.currentBombCount++;
            ChangePrice(PlayerData.pickleData.currentBombCount);
        }

        public void AppendBombsOnLoad()
        {
            foreach (GameObject bombSlot in bombSlots)
            {
                if (bombSlot.transform.childCount != 1)
                {
                    GameObject bombClone = Instantiate(bombPrefab.gameObject);
                    bombClone.transform.position = bombSlot.transform.position;
                    bombClone.transform.SetParent(bombSlot.transform);
                    bombClone.name = "Bomb";
                }
            }
        }

        public void GainExtraBombSlot()
        {
            PlayerData.pickleData.maximumBombCount++;
            GetBombSlot(PlayerData.pickleData.maximumBombCount);
        }

        public void CreateBombSlots(int amount)
        {
            Debug.Log("Creating bomb slots");
            Debug.Log($"Amount Creating: {amount}");
            for (int bombCount = 0; bombCount < amount; ++bombCount)
            {
                Debug.Log($"Executing again...");

                float theta = (2 * Mathf.PI / amount) * bombCount;
                angles.Add(new Vector3(Mathf.Cos(theta) * distance, Mathf.Sin(theta) * distance, 0));
            }
        }

        public void AddBombSlots()
        {
            for (int i = 0; i < angles.Count; i++)
            {
                GameObject bombSlotClone = Instantiate(bombSlotPrefab);
                bombSlotClone.transform.SetParent(bombSlotsTransform.transform);
                bombSlotClone.transform.position = angles[i];
                bombSlotClone.name = "BombSpot";
                bombSlotClone.transform.localScale = new Vector2(1f, 1f);
                bombSlots.Add(bombSlotClone);
            }
        }

        public void GetBombSlot(int amount)
        {
            ClearBombSlots();
            CreateBombSlots(amount);
            AddBombSlots();
        }

        public void RemoveBombCount(BombController pickleBombController)
        {
            StartCoroutine(WaitForAnimation(pickleBombController));
        }

        IEnumerator WaitForAnimation(BombController pickleBombController)
        {
            yield return new WaitForSeconds(4);
            Debug.Log("Decreased bomb count");
            PlayerData.pickleData.currentBombCount--;
            pickleBombController.ChangePrice(PlayerData.pickleData.currentBombCount);
        }

        public void ChangePrice(int bombCount)
        {
            long newCost = 3000;

            for (int append = 0; append < bombCount; append++)
            {
                newCost = newCost + (250 * bombCount);
            }

            PlayerData.pickleData.bombCost = newCost;
        }

        public void ClearBombSlots()
        {
            foreach (Transform child in bombSlotsTransform.transform)
            {
                Destroy(child.gameObject);
                bombSlots.Clear();
            }
            angles.Clear();
        }

        public void ClearBombSlot()
        {
            foreach (Transform child in bombSlotsTransform.transform)
            {
                if (child.childCount == 0) return;
                Destroy(child.GetChild(0).gameObject);
            }
        }
    }
}
