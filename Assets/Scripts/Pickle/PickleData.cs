namespace PickleClicker.Pickle
{
    [System.Serializable]
    public class PickleData
    {
        public ulong picklesPicked = 0;
        public long picklesPerClick = 1;
        public long picklesPerSecond = 0;

        public ulong totalPicklesSpent = 0;
        public long totalAutoPicklesPicked = 0;
        public long totalAutoPicklesUpgraded = 0;
        public long totalUpgradePicklesPicked = 0;

        public ulong totalPicklesPicked = 0;
        public long totalCoinsCollected = 0;
        public long totalClicks = 0;
        public long totalPoglinsSlayed = 0;

        public int pickleLevel = 1;
        public double currentPickleProgress = 0;
        public double maximumPickleProgress = 69;

        public long pickleJarsBroken = 0;
        public long pickleBombsUsed = 0;

        public int currentPickleBombCount = 0;
        public int maxPickleBombCount = 3;
        public long pickleBombCost = 3000;
    }
}
