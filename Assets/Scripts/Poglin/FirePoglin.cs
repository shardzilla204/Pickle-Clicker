using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace PickleClicker.Poglin
{
    public class FirePoglin : NormalPoglin
    {   
        public int extraLives = 1;
        public bool reviveState = false;

        private void Update() 
        {   
            if (goTowards == null) goTowards = pickleButton.transform;

            float x = this.transform.position.x;
            float y = this.transform.position.y;
            Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
            if (x > screenBounds.x * 2.5f || 
                x < -screenBounds.x * 2.5f || 
                y > screenBounds.y * 2.5f || 
                y < -screenBounds.y * 2.5f) Destroy(this.gameObject);

            dead = animator.GetBool("Dead");

            if (dead || reviveState) return;

            if (stole)
            {
                transform.position = Vector3.MoveTowards(transform.position, goTowards.position, (float) -poglinSpeed * Time.deltaTime);
            }
            else 
            {
                transform.position = Vector3.MoveTowards(transform.position, goTowards.position, (float) poglinSpeed * Time.deltaTime);
            }
        }

        public override void CheckStatus(Image health)
        {
            if (health.fillAmount <= 0 && !dead)
            {
                if (extraLives > 0)
                {
                    extraLives--;
                    FirstDeath();
                }
                else
                {
                    healthBar.gameObject.SetActive(false);
                    animator.SetBool("Dead", true);
                    audioSource.clip = poglinScriptableObject.deathAudio;
                    audioSource.Play();
                    StartCoroutine(AutoCollect());
                }
            }
            else if (dead && animator != null)
            {
                animator.SetTrigger("Disappear");
            }
        }

        public void FirstDeath() 
        {
            animator.SetBool("Burn", true);
            reviveState = animator.GetBool("Burn");
            if (reviveState)
            {
                StartCoroutine(RespawnTimer());
            }
        }

        IEnumerator RespawnTimer()
        {
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Resurrected"))
            {
                yield return new WaitForSeconds(0);
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Resurrected"))
                {
                    Image health = healthBar.GetComponentInChildren<Image>();
                    animator.SetBool("Dead", false);
                    healthBar.gameObject.SetActive(true);
                    health.fillAmount = 1;
                    currentHealth = maxHealth;
                    reviveState = false;
                }
            }
        }
    }
}
