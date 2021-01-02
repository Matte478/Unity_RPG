using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{   
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float timeBetweenAttacks = 1f;
        Health target;
        float timeSinceLastAttack = 0;

        void Update()
        {
            // time since last update
            timeSinceLastAttack += Time.deltaTime;

            // check if target is set
            if (target != null && !target.IsDead()) {
                // check if distance between player and target is less than weapon range
                bool isInRange = Vector3.Distance(transform.position, target.transform.position) < weaponRange;

                // if player is not in range, then move to target
                if(!isInRange) {
                    GetComponent<Mover>().MoveTo(target.transform.position);
                } else
                {
                    GetComponent<Mover>().Cancel();
                    AttackBehaviour();
                }
            }
        }

        private void AttackBehaviour()
        {
            // check time between atacks and run attack animation
            if (timeSinceLastAttack >= timeBetweenAttacks) {
                timeSinceLastAttack = 0;
                // this will trigger hit() event
                GetComponent<Animator>().SetTrigger("attack");
            }
        }

        // Animation Event
        private void Hit()
        {
            target.TakeDamage(weaponDamage);
        }

        public void Attack(CombatTarget combatTarget)
        {
            // start attack action
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
        }
    }
}