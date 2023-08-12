using PickleClicker.Pickle;
using PickleClicker.Data.ScriptableObjects.Poglin;
using PickleClicker.Data.Player;
using PickleClicker.Data.Poglin;
using PickleClicker.Data.Upgrade;
using PickleClicker.Controller.Pickle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace PickleClicker.Game.Poglin.Types
{
    public class NormalPoglin : MonoBehaviour
    {
        public PoglinScriptableObject poglinScriptableObject; 

        public double maxHealth;
        public double currentHealth;
        public Image healthBar;

        public double currentTemporaryHealth;
        public Image temporaryHealthBar;

        public float poglinSpeed;

        public PickleButton pickleButton;
        public AudioSource audioSource;
        public Animator animator;

        public GameObject value;
        public PickleController pickleController;

        public Transform goTowards;

        public bool stole;
        protected bool dead;

        protected double totalSteal;

        protected virtual void Start()
        {
            pickleController = GameObject.FindObjectOfType<PickleController>();
            maxHealth *= PlayerData.pickleData.level;
            currentHealth = maxHealth;

            currentTemporaryHealth = 0;
            temporaryHealthBar.fillAmount = 0;

            pickleButton = GameObject.FindObjectOfType<PickleButton>();
            audioSource.clip = poglinScriptableObject.stealAudio;

            poglinSpeed = poglinScriptableObject.speed;

            goTowards = pickleButton.transform;
        }

        private void Update() 
        {   
            if (goTowards == null) goTowards = pickleButton.transform;
            
            float x = transform.position.x;
            float y = transform.position.y;
            Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            if (x > screenBounds.x * 2.5f || 
                x < -screenBounds.x * 2.5f || 
                y > screenBounds.y * 2.5f || 
                y < -screenBounds.y * 2.5f) Destroy(this.gameObject);

            dead = animator.GetBool("Dead");

            if (dead) return;

            if (stole && goTowards.transform == pickleButton.transform)
            {
                transform.position = Vector3.MoveTowards(transform.position, goTowards.position, (float) -poglinSpeed * Time.deltaTime);
            }
            else 
            {
                transform.position = Vector3.MoveTowards(transform.position, goTowards.position, (float) poglinSpeed * Time.deltaTime);
            }
        }

        protected virtual void DecreaseHealthBar()
        {
            UpgradeCategoryData clickCategory = PlayerData.upgradeCategoryDataList.Find(category => category.id == 0);
            UpgradeData attackDamage = clickCategory.upgrades.Find(upgrade => upgrade.id == 1);
            UpgradeData extraAttackDamage = clickCategory.upgrades.Find(upgrade => upgrade.id == 3);
            UpgradeData extraAttackDamageChance = clickCategory.upgrades.Find(upgrade => upgrade.id == 4);

            int chance = Random.Range(extraAttackDamageChance.amount, 500);

            if (chance > 315 - extraAttackDamageChance.amount) {
                if (attackDamage.amount != 0 && extraAttackDamage.amount != 0) {
                    currentHealth -= (attackDamage.amount + extraAttackDamage.amount);  
                }
                else if (attackDamage.amount != 0) {
                    currentHealth -= attackDamage.amount;
                }
                else {
                    currentHealth--;
                }
            }
            else {
                if (attackDamage.amount != 0) {
                    currentHealth -= attackDamage.amount;  
                }
                else {
                    currentHealth--;
                }
            }

            healthBar.fillAmount = (float) Math.Round((float) currentHealth/(float) maxHealth, 2);
            CheckStatus(healthBar);
        }

        protected void DecreaseTemporaryHealthBar()
        {
            UpgradeCategoryData clickCategory = PlayerData.upgradeCategoryDataList.Find(category => category.id == 0);
            UpgradeData attackDamage = clickCategory.upgrades.Find(upgrade => upgrade.id == 1);
            UpgradeData extraAttackDamage = clickCategory.upgrades.Find(upgrade => upgrade.id == 3);
            UpgradeData extraAttackDamageChance = clickCategory.upgrades.Find(upgrade => upgrade.id == 4);

            if (attackDamage.amount != 0)
            {
                currentTemporaryHealth -= attackDamage.amount;  
            }
            else
            {
                currentTemporaryHealth -= 1;
            }
            temporaryHealthBar.fillAmount = (float) Math.Round((float) currentTemporaryHealth/(float) (maxHealth), 2);
        }

        public virtual void CheckStatus(Image healthBar)
        {
            if (healthBar.fillAmount <= 0 && !dead)
            {
                healthBar.gameObject.SetActive(false);
                animator.SetBool("Dead", true);
                audioSource.clip = poglinScriptableObject.deathAudio;
                audioSource.Play();
                StartCoroutine(AutoCollect());
            }
            else if (dead && animator != null)
            { 
                animator.SetTrigger("Disappear");
            }
        }

        public virtual void CollectPoglin()
        {
            List<PoglinData> poglins = PlayerData.poglinDataList;
            poglins[poglinScriptableObject.id].killed += 1;
            PlayerData.pickleData.totalPoglinsSlayed += 1;

            float minimumRecieve = poglinScriptableObject.minimumCapacity/10;
            float maximumRecieve = poglinScriptableObject.maximumCapacity/5;
            double totalRecieve = PlayerData.pickleData.pickles * Math.Round(Random.Range(minimumRecieve, maximumRecieve), 2);

            double totalInterest = (totalSteal * poglinScriptableObject.interest) + totalRecieve;
            PlayerData.pickleData.pickles += (ulong) totalInterest;
            PlayerData.pickleData.currentProgress += (Random.Range(5, 10) * PlayerData.pickleData.level)/3;

            // value.transform.SetParent(pickleController.transform);
            GameObject valueClone = Instantiate(value);
            valueClone.name = "Value";
            valueClone.transform.SetParent(pickleController.transform);
            valueClone.GetComponent<TextMesh>().text = $"+{totalInterest.ToString("N0")}";
            StartCoroutine(StartPopUp(valueClone));

        }

        protected virtual void OnMouseUp() 
        {
            if (temporaryHealthBar.fillAmount > 0)
            {
                DecreaseTemporaryHealthBar();
                return;
            }

            DecreaseHealthBar();
        }
        
        public void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.name == "PickleBomb" && !stole)
            {
                goTowards = col.gameObject.transform;
            }

            if (col.gameObject.name != "PickleButton" || dead) return;
            
            if (gameObject.transform.rotation == Quaternion.Euler(0,0,0))
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            stole = true;
            audioSource.Play();
            
            UpgradeCategoryData poglinCategory = PlayerData.upgradeCategoryDataList.Find(category => category.id == 2);
            UpgradeData defense = poglinCategory.upgrades.Find(upgrade => upgrade.id == 0);
            
            float minimumCapacity = poglinScriptableObject.minimumCapacity;
            float maximumCapacity = poglinScriptableObject.maximumCapacity;
            double beforeDefense = Math.Round(Random.Range(minimumCapacity, maximumCapacity), 2);
            double afterDefense = 0;
            if (defense.amount != 0)
            {
                afterDefense = (beforeDefense/defense.amount);
            }
            else 
            {
                afterDefense = beforeDefense;
            }

            totalSteal = ((double) PlayerData.pickleData.pickles * afterDefense);
            
            poglinSpeed *= 1.7f;

            GameObject valueClone = Instantiate(value);
            valueClone.transform.SetParent(pickleController.transform);
            valueClone.name = "Value";
            valueClone.GetComponent<TextMesh>().text = $"-{totalSteal.ToString("N0")}";
            StartCoroutine(StartPopUp(valueClone));

            Debug.Log($"The Poglin stole {totalSteal} pickles!");
            PlayerData.pickleData.pickles -= (ulong) totalSteal;
        }

        IEnumerator StartPopUp(GameObject clone)
        {
 
            clone.transform.position = new Vector3(Random.Range(-1f, 2f), Random.Range(-1f, 2f), 0);
            clone.GetComponent<Animator>().SetTrigger("Activate");
            yield return new WaitForSeconds(1f);
            Destroy(clone);
        }

        public IEnumerator DecreaseTemporaryHealthbarOvertime()
        {
            currentTemporaryHealth = maxHealth + 2;
            while(temporaryHealthBar.fillAmount > 0)
            {
                yield return new WaitForSeconds(0.5f);
                currentTemporaryHealth -= 1;
                temporaryHealthBar.fillAmount = (float) Math.Round((float) currentTemporaryHealth/(float) maxHealth, 2);
            }
        }

        public IEnumerator AutoCollect()
        {
            yield return new WaitForSeconds(3f);
            animator.SetTrigger("Disappear");
        }
    }
}
