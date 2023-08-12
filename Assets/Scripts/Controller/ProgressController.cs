using PickleClicker.Data;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PickleClicker.Controller
{
    public class ProgressController : MonoBehaviour
    {
        private double maximumProgress;
        private double currentProgress;
        [SerializeField] private Text pickleProgressBar;
        [SerializeField] private Text pickleLevelText;
        [SerializeField] private Image mask;
        [SerializeField] private AudioSource levelUp;
        public static int maxLevel = 250;

        private int levelRequired = PlayerData.pickleData.pickleLevel + 1;

        private void Update()
        {
            maximumProgress = PlayerData.pickleData.maximumPickleProgress;
            currentProgress = PlayerData.pickleData.currentPickleProgress;
            GetCurrentFill();
            CheckProgress();
        }

        //Displays the progress bar state
        private void GetCurrentFill()
        {
            mask.fillAmount = (float) (currentProgress / maximumProgress);
            float percentage = (float) (currentProgress / maximumProgress);
            string percentageString = (percentage * 100).ToString("N2");
            if (PlayerData.pickleData.pickleLevel < maxLevel)
            {
                pickleProgressBar.text = $"{percentageString}%";
            }
            else
            { 
                if (PlayerData.pickleData.pickleLevel >= maxLevel)
                {

                    mask.fillAmount = 1f;
                    pickleProgressBar.text = "Max";
                    // pickleLevelText.color = Color.yellow;
                }
            }
        }

        //Checks if the progress bar is filled
        private void CheckProgress()
        {
            if (currentProgress >= maximumProgress && PlayerData.pickleData.pickleLevel < maxLevel)
            {
                PlayerData.pickleData.currentPickleProgress -= PlayerData.pickleData.maximumPickleProgress;
                PlayerData.pickleData.pickleLevel++;
                PlayerData.pickleData.maximumPickleProgress = (int) maximumProgress + 5;

                // Adds 1-2.5% of players current Pickles for leveling up.
                PlayerData.pickleData.picklesPicked += (ulong) (Random.Range(PlayerData.pickleData.picklesPicked/100, PlayerData.pickleData.picklesPicked/40));

                levelUp.Play();
            }
        }
    }
}
