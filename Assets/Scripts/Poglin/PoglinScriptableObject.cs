using UnityEngine;

namespace PickleClicker.Poglin
{ 
    [CreateAssetMenu(fileName = "Poglin", menuName = "ScriptableObjects/Poglin")]
    public class PoglinScriptableObject : ScriptableObject
    {
        public GameObject prefab;
        public int id;
        public string alias;
        [TextArea(8, 8)]
        public string description;
        public int rarity;
        public float minimumCapacity = 0.05f;
        public float maximumCapacity = 0.075f;
        public float speed = 1f;
        public float interest = 0.33f;
        public AudioClip deathAudio;
        public AudioClip stealAudio;
    }
}
