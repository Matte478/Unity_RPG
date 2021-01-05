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

        void Start()
        {
            initPosition = transform.position;
            initRotation = transform.rotation;

            // first checkpoint is level init position
            checkpointPosition = transform.position;
            checkpointRotation = transform.rotation;
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
            GetComponent<Health>().Reborn();

            GetComponent<NavMeshAgent>().Warp(position);
            transform.rotation = rotation;
        }

        private void OnTriggerEnter(Collider other) {
            if(other.CompareTag("Checkpoint")) {
                checkpointPosition = other.transform.position;
                checkpointRotation = other.transform.rotation;
            }
        }
    }
}