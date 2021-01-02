using System;
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
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 2f;
        [SerializeField] float waypointDwellTime = 3f;
        
        Fighter fighter;
        Health health;
        Mover mover;
        GameObject player;
        
        Vector3 initPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        int curentWaypointIndex = 0;

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
                PatrolBehaviour();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = initPosition;

            if (patrolPath != null) {
                // if enemy is on the current waypoint (curentWaypointIndex), then he goes to the next waypoint 
                // otherwise he goes to current waypoint
                if(AtWaypoint()) {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            // the enemy will wait a while at the waypoint and then move on to the next one
            if (timeSinceArrivedAtWaypoint >= waypointDwellTime) {
                mover.StartMoveAction(nextPosition);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            // print("distanceToWaypoint: " + distanceToWaypoint);
            // print("waypointTolerance: " + waypointTolerance);
            return distanceToWaypoint <= waypointTolerance;
        }

        private void CycleWaypoint()
        {
            curentWaypointIndex = patrolPath.GetNextIndex(curentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypointPosition(curentWaypointIndex);
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