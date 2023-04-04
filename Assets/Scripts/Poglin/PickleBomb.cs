using PickleClicker.Pickle;
using PickleClicker.Data;
using PickleClicker.Upgrade;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PickleClicker.Poglin
{
    public class PickleBomb : MonoBehaviour
    {
        [SerializeField] private CircleCollider2D detectRadius;
        [SerializeField] private RectTransform circleRadius;
        [SerializeField] private int explosionDamage;
        private Vector3 screenPoint;
        private Vector3 offset;
        private List<NormalPoglin> poglins = new List<NormalPoglin>();

        public GameObject timer;
        public  bool startTimer = false;

        private void Start() 
        {
            detectRadius = gameObject.GetComponent<CircleCollider2D>();
            SetRadius();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.GetComponent<NormalPoglin>() == null) return;

            NormalPoglin poglin = col.gameObject.GetComponent<NormalPoglin>();
            
            if (poglin.stole) return;

            poglins.Add(poglin);
            StartCoroutine(Timer());
        }

        IEnumerator Explode()
        {
            Animator pickleBombAnimator = gameObject.GetComponent<Animator>();
            pickleBombAnimator.SetTrigger("Countdown");
            yield return new WaitForSeconds(2f);
            pickleBombAnimator.SetTrigger("Disappear");
            foreach (NormalPoglin poglin in poglins)
            {
                int damage = CalculateDamage(poglin);
                bool hasTemporaryHealth = GetTemporaryHealth(poglin);
                bool isEarthPoglin = GetPoglinType(poglin);
                bool hasArmor = false;

                if (isEarthPoglin) hasArmor = HasArmor(poglin);

                ApplyDamage(poglin, damage, hasTemporaryHealth, hasArmor);
            }
        }

        public void SetUpCountdown()
        {
            GameObject timerClone = Instantiate(timer);
            timerClone.transform.position = gameObject.transform.parent.position;
            timerClone.transform.SetParent(gameObject.transform.parent);
            timerClone.name = "Timer";

            PickleBombTimer pickleBombTimer = timerClone.GetComponent<PickleBombTimer>();
            pickleBombTimer.StartCountdown(timerClone);
        }

        private int CalculateDamage(NormalPoglin poglin)
        {
            UpgradeCategoryData bombCategory = PlayerData.upgradeList.upgradeCategories.Find(category => category.id == 3);
            UpgradeData bombDamage = bombCategory.upgradeBuyables.Find(upgrade => upgrade.id == 1);

            if (bombDamage.amount <= 0) return explosionDamage;

            return explosionDamage * bombDamage.amount;
        }

        private bool GetTemporaryHealth(NormalPoglin poglin)
        {
            if (poglin.currentTemporaryHealth <= 0) return false;

            return true;
        }

        private bool GetPoglinType(NormalPoglin poglin)
        {
            if (poglin.poglinScriptableObject.id != 4) return false;

            Debug.Log("Earth Detected");

            return true;
        }

        private bool HasArmor(NormalPoglin poglin)
        {
            EarthPoglin earthPoglin = poglin.GetComponent<EarthPoglin>();

            if (earthPoglin.currentArmor > 0) return true;

            return false;

        }

        private void ApplyDamage(NormalPoglin poglin, int damage, bool hasTemporaryHealth, bool hasArmor)
        {
            if (hasArmor)
            {
                poglin.GetComponent<EarthPoglin>().BreakShield();
                Debug.Log("Breaking Earth Shield");
                return;
            }

            if (hasTemporaryHealth)
            {
                poglin.currentTemporaryHealth -= damage;
                poglin.temporaryHealthBar.fillAmount = (float) Math.Round((float) poglin.currentTemporaryHealth/(float) (poglin.maxHealth), 2);
                return;
            }

            if (!hasTemporaryHealth || !hasArmor) 
            {
                poglin.currentHealth -= damage;
                poglin.healthBar.fillAmount = (float) Math.Round((float) poglin.currentHealth/(float) poglin.maxHealth, 2);
                poglin.CheckStatus(poglin.healthBar);
                return;
            }
        }

        IEnumerator ApplyDamageOverTime(int amount, NormalPoglin poglin)
        {
            int TIME = 10;
            for (int iteration = 0; iteration < TIME; iteration++)
            {
                yield return new WaitForSeconds(1f);
                poglin.currentHealth -= amount;
            }
        }

        public void ApplyKnockback(int amount, NormalPoglin poglin)
        {
            poglin.transform.position = Vector3.MoveTowards(poglin.transform.position, poglin.transform.position * amount, (float) -poglin.poglinSpeed * Time.deltaTime);
        }

        IEnumerator ApplyStun(int amount, NormalPoglin poglin)
        {
            poglin.transform.position = new Vector3(poglin.transform.position.x, poglin.transform.position.y, poglin.transform.position.z);
            yield return new WaitForSeconds(3f);
        }

        IEnumerator Timer()
        {
            yield return new WaitForSeconds(3f);
            StartCoroutine(Explode());            
        }

        public void SetRadius()
        {
            UpgradeCategoryData bombCategory = PlayerData.upgradeList.upgradeCategories.Find(category => category.id == 3);
            UpgradeData bombRadius = bombCategory.upgradeBuyables.Find(upgrade => upgrade.id == 1);
            Vector3 currentScale = circleRadius.transform.localScale;

            detectRadius.radius = (float) bombRadius.amount/65;
            circleRadius.transform.localScale = new Vector3(detectRadius.radius * 2, detectRadius.radius * 2, detectRadius.radius * 2);
        }
    }
}