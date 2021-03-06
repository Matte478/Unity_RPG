using System;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.UI;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour 
    {
        Health health;
        Spawner spawner;
        Menu menu;
        
        void Start()
        {
            health = GetComponent<Health>();
            spawner = GetComponent<Spawner>();
            menu = FindObjectOfType<Menu>();
        }

        void Update()
        {
            if(menu.IsPaused() || menu.IsFinished()) return;
            
            if(health.IsDead()) {
                if(Input.GetKeyDown(KeyCode.R)) {
                    // respawn at last checkpoint
                    spawner.RespawnAtCheckpoint();
                } else if(Input.GetKeyDown(KeyCode.T)) {
                    // respawn at level start
                    spawner.RespawnAtInit();
                }

                return;
            }

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
                
                if(target == null) continue;

                GameObject targetGameObject = target.gameObject;
                if (!GetComponent<Fighter>().CanAttack(targetGameObject)) continue;

                // call Attack() on click
                if (Input.GetMouseButton(0)) {
                    GetComponent<Fighter>().Attack(targetGameObject);
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
        
        private void OnTriggerEnter(Collider other) {
            if(other.CompareTag("Finish")) {
                print("ewjiorjeiwjrioew");
                menu.FinishGame();
            }
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}