using System.Collections.Generic;

public class UpgradeCategory
{
    public string alias { get; set; }
    public string description { get; set; }
    public List<UpgradePickle> upgrades = new List<UpgradePickle>();

}