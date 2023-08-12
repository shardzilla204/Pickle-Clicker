using System.Collections.Generic;
using UnityEngine;

namespace PickleClicker.Data.ScriptableObjects.Auto
{
    [CreateAssetMenu(fileName = "AutoDatabase", menuName = "ScriptableObjects/AutoDatabase")]
    public class AutoDatabase : ScriptableObject
    {
        public List<AutoScriptableObject> autoScriptableObjects = new List<AutoScriptableObject>();
    }
}