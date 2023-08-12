using PickleClicker.Data.Player;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Tracker
{  
    public class ValueTracker : MonoBehaviour
    {
        public GameObject[] picklesPickedTexts;
        public GameObject[] picklesPerSecondTexts;
        public GameObject[] pickleCoinTexts;

        private void Update()
        {
            foreach (GameObject picklesPickedText in picklesPickedTexts)
            {
                string pickles = PlayerData.pickleData.pickles.ToString("N0");
                picklesPickedText.GetComponent<Text>().text = $"{pickles}";
            }

            foreach (GameObject picklesPerSecondText in picklesPerSecondTexts)
            {
                string gainPerSecond = PlayerData.pickleData.gainPerSecond.ToString("N0");
                picklesPerSecondText.GetComponent<Text>().text = $"{gainPerSecond}";
            }

            foreach (GameObject picklesCoinText in pickleCoinTexts)
            {
                string pickleCoins = PlayerData.buyableData.pickleCoins.ToString("N0");
                picklesCoinText.GetComponent<Text>().text = $"{pickleCoins}";
            }
        }
    }
}
