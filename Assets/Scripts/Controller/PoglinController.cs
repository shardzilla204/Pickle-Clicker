using PickleClicker.Data.Player;
using PickleClicker.Data.Poglin;
using PickleClicker.Data.ScriptableObjects.Poglin;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Controller.Poglin
{  
    public class PoglinController : MonoBehaviour
    {
        public Text nameText;
        public Text killCount;
        public Text descriptionText;
        public Animator animator;

        public void SetPoglin(PoglinScriptableObject poglinScriptableObject)
        {
            PoglinData poglinVariant = PlayerData.poglinDataList.Find(variant => variant.id == poglinScriptableObject.id);
            this.nameText.text = $"Type: {poglinScriptableObject.alias}";
            this.killCount.text = $"Killed: {poglinVariant.killed.ToString("N0")}";
            this.descriptionText.text = poglinScriptableObject.description;
            this.animator.runtimeAnimatorController = poglinScriptableObject.prefab.GetComponent<Animator>().runtimeAnimatorController;
        }
    }
}
