using PickleClicker.Pickle;
using PickleClicker.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Poglin
{
    public class EarthPoglin : NormalPoglin
    {
        public int maxArmor;
        public int currentArmor;
        public Image armorBar;
        public Animator barrierAnimator;

        protected override void Start() 
        {
            pickleController = GameObject.FindObjectOfType<PickleController>();
            maxHealth *= PlayerData.pickleData.pickleLevel;
            currentHealth = maxHealth;

            maxArmor *= (int) PlayerData.pickleData.pickleLevel;
            currentArmor = maxArmor;

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

            if (!stole)
            {
                transform.position = Vector3.MoveTowards(transform.position, goTowards.position, (float) poglinSpeed * Time.deltaTime);
            }
            else 
            {
                transform.position = Vector3.MoveTowards(transform.position, goTowards.position, (float) -poglinSpeed * Time.deltaTime);
            }
        }

        private void DecreaseArmorBar()
        {
            UpgradeCategoryData clickCategory = PlayerData.upgradeList.upgradeCategories.Find(category => category.id == 0);
            UpgradeData attackDamage = clickCategory.upgradeBuyables.Find(upgrade => upgrade.id == 1);
            UpgradeData ironSpear = clickCategory.upgradeBuyables.Find(upgrade => upgrade.id == 5);
            if (ironSpear.amount != 0) 
            {
                currentArmor -= attackDamage.amount;
            }
            else
            {
                currentArmor--;
            }
            armorBar.fillAmount = (float) Math.Round((float) currentArmor/ (float) maxArmor, 2);

            if (currentArmor > 0) return;
            
            barrierAnimator.SetTrigger("Break");
        }
        
        public override void CheckStatus(Image healthBar)
        {
            if (healthBar.fillAmount <= 0 && !dead && armorBar.fillAmount <= 0)
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

        public void BreakShield()
        {
            armorBar.fillAmount = 0;
            currentArmor = 0;
            barrierAnimator.SetTrigger("Break");
        }

        new private void OnMouseUp()
        {
            if (currentArmor > 0)
            {
                DecreaseArmorBar();
            }
            else
            {
                DecreaseHealthBar();
            }
        }


    }
}
