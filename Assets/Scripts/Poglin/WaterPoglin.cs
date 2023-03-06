using PickleClicker.Pickle;
using PickleClicker.Data;
using PickleClicker.Upgrade;
using System;
using System.Collections;
using UnityEngine;

namespace PickleClicker.Poglin
{  
    public class WaterPoglin : NormalPoglin
    {
        private NormalPoglin[] poglins;
        private bool appendTemporaryHealthBar;

        protected override void Start() 
        {
            pickleController = GameObject.FindObjectOfType<PickleController>();
            maxHealth *= PlayerData.pickleData.pickleLevel;
            currentHealth = maxHealth;

            currentTemporaryHealth = 0;
            temporaryHealthBar.fillAmount = 0;

            pickleButton = GameObject.FindObjectOfType<PickleButton>();
            audioSource.clip = poglinScriptableObject.stealAudio;

            poglinSpeed = poglinScriptableObject.speed;

            goTowards = pickleButton.transform;

            StartCoroutine(AutoHeal());
        }

        private void Update() 
        {
            if (goTowards == null) goTowards = pickleButton.transform;

            poglins = GameObject.FindObjectsOfType<NormalPoglin>();
            float x = transform.position.x;
            float y = transform.position.y;
            Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            if (x > screenBounds.x * 2.5f || 
                x < -screenBounds.x * 2.5f || 
                y > screenBounds.y * 2.5f || 
                y < -screenBounds.y * 2.5f) Destroy(this.gameObject);

            dead = animator.GetBool("Dead");

            if (!dead) 
            {
                if (stole) 
                {
                    transform.position = Vector3.MoveTowards(transform.position, goTowards.position, (float) -poglinSpeed * Time.deltaTime);
                }
                else 
                {
                    transform.position = Vector3.MoveTowards(transform.position, goTowards.position, (float) poglinSpeed * Time.deltaTime);
                }
            }
            else 
            {
                if (appendTemporaryHealthBar) return;

                foreach (NormalPoglin poglin in poglins) 
                {   
                    EarthPoglin earthPoglin = poglin.GetComponent<EarthPoglin>();
                    if (earthPoglin != null && earthPoglin.currentArmor <= 0) {
                        ApplyTemporaryHealth(poglin);
                    }
                    
                    if (poglin.poglinScriptableObject.id != 4) {
                        ApplyTemporaryHealth(poglin);
                    }
                }
                appendTemporaryHealthBar = true;
            }  
        }

        private void ApplyTemporaryHealth(NormalPoglin poglin) {
            if (!poglin.animator.GetBool("Dead")) 
            {
                poglin.currentTemporaryHealth = poglin.currentHealth;
                poglin.temporaryHealthBar.fillAmount = (float) Math.Round((float) poglin.currentHealth/(float) poglin.maxHealth, 2);
                StartCoroutine(poglin.DecreaseTemporaryHealthbarOvertime());
            }
        }

        IEnumerator AutoHeal() {
            while (!animator.GetBool("Dead")) 
            {
                yield return new WaitForSeconds(0.5f);

                foreach (NormalPoglin poglin in poglins) 
                {
                    EarthPoglin earthPoglin = poglin.GetComponent<EarthPoglin>();
                    
                    UpgradeCategoryData clickCategory = PlayerData.upgradeList.upgradeCategories.Find(category => category.id == 0);
                    UpgradeData attackDamage = clickCategory.upgradeBuyables.Find(upgrade => upgrade.id == 1);

                    if (earthPoglin != null && earthPoglin.currentArmor <= 0) 
                    {
                        if (poglin.animator.GetBool("Dead")) yield return "Not Dead";

                        for (int i = 0; i < (poglin.maxHealth - poglin.currentHealth)/poglin.currentHealth; i++) {

                            if (earthPoglin.currentHealth >= earthPoglin.maxHealth) yield return "More Than Max Health";

                            earthPoglin.currentHealth += (int) Math.Floor(attackDamage.amount/3f);

                            if (earthPoglin.currentHealth >= earthPoglin.maxHealth) earthPoglin.currentHealth = earthPoglin.maxHealth;

                            earthPoglin.healthBar.fillAmount = (float) Math.Round((float) earthPoglin.currentHealth/(float) earthPoglin.maxHealth, 2);
                        }
                    }
                    
                    if (poglin.poglinScriptableObject.id != 4) 
                    {
                        if (poglin.animator.GetBool("Dead")) yield return "Not Dead";

                        if (poglin.currentHealth >= poglin.maxHealth) yield return "More Than Max Health";

                        poglin.currentHealth += (int) Math.Floor(attackDamage.amount/3f);

                        if (poglin.currentHealth >= poglin.maxHealth) poglin.currentHealth = poglin.maxHealth;

                        poglin.healthBar.fillAmount = (float) Math.Round((float) poglin.currentHealth/(float) poglin.maxHealth, 2);
                    }
                }
            }
        }
    }
}
