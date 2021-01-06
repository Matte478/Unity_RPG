using UnityEngine;
using UnityEngine.AI;

namespace RPG.Core
{
    public class Spawner : MonoBehaviour
    {
        Vector3 initPosition;
        Quaternion initRotation;
        Vector3 checkpointPosition;
        Quaternion checkpointRotation;
        Health[] characters;

        void Start()
        {
            initPosition = transform.position;
            initRotation = transform.rotation;

            // first checkpoint is level init position
            checkpointPosition = transform.position;
            checkpointRotation = transform.rotation;

            characters = FindObjectsOfType(typeof(Health)) as Health[];
        }

        public void RespawnAtInit()
        {
            Respawn(initPosition, initRotation);

            // if respawn at the init position, then reset the checkpoint
            checkpointPosition = initPosition;
            checkpointRotation = initRotation;
        }

        public void RespawnAtCheckpoint()
        {
            Respawn(checkpointPosition, checkpointRotation);
        }

        private void Respawn(Vector3 position, Quaternion rotation)
        {
            // GetComponent<Health>().Reborn();

            // move and rotate character
            GetComponent<NavMeshAgent>().Warp(position);
            transform.rotation = rotation;

            // reborn all characters (enemies + player) or refill their health
            foreach(Health character in characters) {
                if(character.IsDead())
                    character.Reborn();
                else
                    character.FillHealth();
            }
        }

        private void OnTriggerEnter(Collider other) {
            if(other.CompareTag("Checkpoint")) {
                checkpointPosition = other.transform.position;
                checkpointRotation = other.transform.rotation;
            }
        }
    }
}