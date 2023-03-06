using PickleClicker.Data;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Poglin
{  
    public class PoglinVariantManager : MonoBehaviour
    {
        public Text nameText;
        public Text killCount;
        public Text descriptionText;
        public Animator animator;

        public void SetPoglin(PoglinScriptableObject poglinScriptableObject)
        {
            PoglinVariantData poglinVariant = PlayerData.poglinList.poglinVariants.Find(variant => variant.id == poglinScriptableObject.id);
            this.nameText.text = $"Type: {poglinScriptableObject.alias}";
            this.killCount.text = $"Killed: {poglinVariant.killed.ToString("N0")}";
            this.descriptionText.text = poglinScriptableObject.description;
            this.animator.runtimeAnimatorController = poglinScriptableObject.prefab.GetComponent<Animator>().runtimeAnimatorController;
        }
    }
}
