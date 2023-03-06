using PickleClicker.Data;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace PickleClicker.Poglin
{
    public class PickleBombController : MonoBehaviour
    {
        [SerializeField] private PickleBomb pickleBomb;

        private void Update() 
        {
            pickleBomb = gameObject.GetComponent<PickleBomb>();    
        }

        public void UpgradeRadius()
        {
            UpgradeCategoryData bombCategory = PlayerData.upgradeList.upgradeCategories.Find(category => category.id == 3);
            UpgradeData bombRadius = bombCategory.upgradeBuyables.Find(upgrade => upgrade.id == 1);

            Debug.Log(bombRadius.amount);

            if (bombRadius.amount < 5)
            {
                pickleBomb.gameObject.GetComponent<CircleCollider2D>().radius += (bombRadius.amount/10);
            }
            else 
            {
                pickleBomb.gameObject.GetComponent<CircleCollider2D>().radius += (bombRadius.amount/100);
            }
        }
    }
}

