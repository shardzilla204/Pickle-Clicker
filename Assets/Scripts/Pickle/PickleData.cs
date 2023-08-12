namespace PickleClicker.Data
{
    [System.Serializable]
    public class PickleData
    {
        public double pickles = 0;
        public double gainPerClick = 1;
        public double gainPerSecond = 0;

        public double totalPicklesSpent = 0;
        public double totalAutoPickles = 0;
        public double totalAutoPicklesUpgraded = 0;
        public double totalUpgradePickles = 0;

        public double totalPickles = 0;
        public double totalCoins = 0;
        public double totalClicks = 0;
        public double totalPoglinsSlayed = 0;

        public double level = 1;
        public double currentProgress = 0;
        public double maximumProgress = 100;

        public double jarsBroken = 0;
        public double bombsUsed = 0;

        public int currentBombCount = 0;
        public int maximumBombCount = 3;
        public double bombCost = 3000;
    }
}
