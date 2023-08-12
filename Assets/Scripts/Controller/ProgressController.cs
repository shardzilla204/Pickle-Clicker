using PickleClicker.Data.Player;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Controller
{
    public class ProgressController : MonoBehaviour
    {
        [SerializeField] private Text pickleProgressBar;
        [SerializeField] private Text pickleLevelText;
        [SerializeField] private Image mask;
        [SerializeField] private AudioSource levelUp;
        public static bool pickleLevelMax;
        public static double maxLevel = 250;

        private double levelRequired = PlayerData.pickleData.level + 1;

        //Displays the progress bar state
        public void GetCurrentFill()
        {
            double maximumProgress = PlayerData.pickleData.maximumProgress;
            double currentProgress = PlayerData.pickleData.currentProgress;

            mask.fillAmount = (float) (currentProgress / maximumProgress);
            float percentage = (float) (currentProgress / maximumProgress);
            string percentageString = (percentage * 100).ToString("N2");
            if (PlayerData.pickleData.level < maxLevel)
            {
                pickleProgressBar.text = $"{percentageString}%";
            }
            else
            { 
                pickleLevelText.color = Color.white;
                pickleProgressBar.color = Color.white;

                if (PlayerData.pickleData.level < maxLevel) return;

                pickleLevelMax = true;

                mask.fillAmount = 1f;
                pickleProgressBar.text = "Max";
                pickleProgressBar.color = Color.yellow;
                pickleLevelText.color = Color.yellow;
            }
        }

        //Checks if the progress bar is filled
        public void CheckProgress()
        {
            double maximumProgress = PlayerData.pickleData.maximumProgress;
            double currentProgress = PlayerData.pickleData.currentProgress;

            if (currentProgress >= maximumProgress && PlayerData.pickleData.level < maxLevel)
            {
                PlayerData.pickleData.currentProgress -= PlayerData.pickleData.maximumProgress;
                PlayerData.pickleData.level++;
                PlayerData.pickleData.maximumProgress = (int) maximumProgress + 5;

                // Adds 1-2.5% of players current Pickles for leveling up.
                PlayerData.pickleData.pickles += Random.Range((float) (PlayerData.pickleData.pickles/100), (float) PlayerData.pickleData.pickles/40);

                levelUp.Play();
            }
            
            string level = PlayerData.pickleData.level.ToString("N0");
            pickleLevelText.text = $"{level}";
        }
    }
}
