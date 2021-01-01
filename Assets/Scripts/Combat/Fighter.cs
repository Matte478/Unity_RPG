using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{   
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        Transform target;

        void Update() {
            // check if target is set
            if (target != null) {
                // check if distance between player and target is less than weaponRange
                bool isInRange = Vector3.Distance(transform.position, target.position) < weaponRange;

                // if player is not in range, then move to target
                if(!isInRange) {
                    GetComponent<Mover>().MoveTo(target.position);
                } else {
                    GetComponent<Mover>().Cancel();
                }
            }
        }
        
        public void Attack(CombatTarget combatTarget)
        {
            // start attack action
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }
    }
}