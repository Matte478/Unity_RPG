using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 5f;
        
        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;
        
        Vector3 initPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;

        void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            player = GameObject.FindWithTag("Player");
            initPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if(health.IsDead()) return;

            // if player is in the attack range and is not dead, then enemy can attack
            if (inAttackRange() && fighter.CanAttack(player)) {
                timeSinceLastSawPlayer = 0;

                fighter.Attack(player);
            } else if (timeSinceLastSawPlayer < suspicionTime) {
                // suspicion state
                // cancel the movement and stay in the last position where the player was seen
                GetComponent<ActionScheduler>().CancelCurrentAction();
            } else {
                mover.StartMoveAction(initPosition);
            }

            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private bool inAttackRange()
        {
            return calculatePlayerDistance() < chaseDistance;
        }

        private float calculatePlayerDistance()
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }

        // called by Unity
        private void OnDrawGizmosSelected()
        {
            // draw enemy chaseDistance
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}