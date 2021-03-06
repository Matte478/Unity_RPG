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
        private Health target;
        private AudioSource[] sounds;
        private float timeSinceLastAttack = Mathf.Infinity;

        void Start()
        {
            sounds = GetComponents<AudioSource>();
        }

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
            // rotate the player's transform to make the player look at the enemy
            transform.LookAt(target.transform);

            // check time between atacks and run attack animation
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                timeSinceLastAttack = 0;
                TriggerAttack();
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");

            PlayHitSound();
            // this will trigger hit() event
            GetComponent<Animator>().SetTrigger("attack");
        }

        // Animation Event
        private void Hit()
        {
            if (target == null) {
                StopHitSound();
                return;
            }

            target.TakeDamage(weaponDamage);
        }

        public bool CanAttack(GameObject combatTarget)
        {
            bool canAttack = false;

            if (combatTarget != null) {
                Health targetHealth = combatTarget.GetComponent<Health>();
                canAttack = targetHealth != null && !targetHealth.IsDead();
            }

            return canAttack;
        }

        public void Attack(GameObject combatTarget)
        {
            // start attack action
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        private void PlayHitSound()
        {
            sounds[0].Play();
        }

        private void StopHitSound()
        {
            sounds[0].Stop();
        }
    }
}