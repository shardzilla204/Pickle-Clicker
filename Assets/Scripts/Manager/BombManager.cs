using PickleClicker.Game.Pickle;
using PickleClicker.Data.Player;
using PickleClicker.Data.Upgrade;
using UnityEngine;

namespace PickleClicker.Manager.Pickle
{
    public class BombManager : MonoBehaviour
    {
        [SerializeField] private Bomb bomb;

        private void Update() 
        {
            bomb = gameObject.GetComponent<Bomb>();    
        }

        public void UpgradeRadius()
        {
            UpgradeCategoryData bombUpgradeCategory = PlayerData.upgradeCategoryDataList.Find(category => category.id == 3);
            UpgradeData bombRadiusUpgrade = bombUpgradeCategory.upgrades.Find(upgrade => upgrade.id == 1);

            Debug.Log(bombRadiusUpgrade.amount);

            // This is here because 
            // Wanted the amount of radius added to the pickle bomb to change
            // After buying 5 upgrades towards the bomb radius, the amount given will have been decreased throughout of upgrading the rest of the bomb radius
            if (bombRadiusUpgrade.amount < 5)
            {
                bomb.gameObject.GetComponent<CircleCollider2D>().radius += (bombRadiusUpgrade.amount/10);
            }
            else 
            {
                bomb.gameObject.GetComponent<CircleCollider2D>().radius += (bombRadiusUpgrade.amount/100);
            }
        }
    }
}

