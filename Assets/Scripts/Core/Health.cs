using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 100f;

        private bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        public void TakeDamage(float damage)
        {
            // if damage > health => set 0
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints == 0) {
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

    }
}