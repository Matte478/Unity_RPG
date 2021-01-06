using UnityEngine;
using UnityEngine.UI;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;
        [SerializeField] Canvas healthbarCanvas;

        private float actualHealthPoints;
        private Image healthbarForeground;
        private Vector2 healthbarFullSize;
        private bool isDead = false;

        void Start()
        {
            actualHealthPoints = healthPoints;

            if (healthbarCanvas == null) {
                Debug.LogError("health canvas not set");
            } else {
                Image foreground = healthbarCanvas.transform.Find("Foreground").GetComponent<Image>();
                healthbarFullSize = foreground.rectTransform.sizeDelta;
            }
        }

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            // if damage > health => set 0
            actualHealthPoints = Mathf.Max(actualHealthPoints - damage, 0);

            UpdateHealthbar();

            if (actualHealthPoints == 0) {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;

            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            isDead = true;
        }

        public void Reborn()
        {
            if (!isDead) return;

            GetComponent<Animator>().SetTrigger("reborn");

            FillHealth();

            isDead = false;
        }

        public void FillHealth()
        {
            // fill actual health with the max health points of character
            actualHealthPoints = healthPoints;
            UpdateHealthbar();
        }

        private void UpdateHealthbar()
        {
            if (healthbarCanvas == null) {
                Debug.LogError("health canvas not set");
                return;
            }

            // get health foreground image from canvas
            Image foreground = healthbarCanvas.transform.Find("Foreground").GetComponent<Image>();

            // calc and set new healthbar foreground with
            float width = healthbarFullSize[0] * actualHealthPoints / healthPoints;
            foreground.rectTransform.sizeDelta = new Vector2(width, healthbarFullSize[1]);
        }
    }
}