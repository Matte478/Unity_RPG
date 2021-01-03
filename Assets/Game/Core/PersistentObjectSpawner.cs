using System;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab;

        static bool hasSpawned = false;

        void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistantObject();
        }

        private void SpawnPersistantObject()
        {
            // create new object from prefab
            GameObject persistentObject = Instantiate(persistentObjectPrefab);

            // this object will be persistent between scenes
            DontDestroyOnLoad(persistentObject);

            hasSpawned = true;
        }
    }
}