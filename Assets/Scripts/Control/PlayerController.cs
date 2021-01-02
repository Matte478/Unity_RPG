using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour 
    {
        void Update()
        {
            // if interacted with CombatTarget
            if(InteractWithCombat()) return;

            // if interacted with movement
            if(InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            // list of all hits on mouse click
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits) {
                // try to get component CombatTarget from hit target
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if (!GetComponent<Fighter>().CanAttack(target)) continue;

                // call Attack() on click
                if (Input.GetMouseButton(0)) {
                    GetComponent<Fighter>().Attack(target);
                }

                return true;

            }

            return false;
        }
        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            // show click target
            // Debug.DrawRay(ray.origin, ray.direction * 100);

            // if ray intersects with a collider => move to click target
            if (hasHit) {
                if (Input.GetMouseButton(0)) {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }

                return true;
            }

            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}