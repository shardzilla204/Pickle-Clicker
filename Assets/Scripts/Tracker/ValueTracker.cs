using PickleClicker.Data;
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
                string picklesPicked = PlayerData.pickleData.picklesPicked.ToString("N0");
                picklesPickedText.GetComponent<Text>().text = $"{picklesPicked}";
            }

            foreach (GameObject picklesPerSecondText in picklesPerSecondTexts)
            {
                string picklesPerSecond = PlayerData.pickleData.picklesPerSecond.ToString("N0");
                picklesPerSecondText.GetComponent<Text>().text = $"{picklesPerSecond}";
            }

            foreach (GameObject picklesCoinText in pickleCoinTexts)
            {
                string pickleCoins = PlayerData.buyableData.pickleCoins.ToString("N0");
                picklesCoinText.GetComponent<Text>().text = $"{pickleCoins}";
            }
        }
    }
}
