using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 0.5f;
        [SerializeField] float fadeIntTime = 0.5f;
        [SerializeField] float fadeWaitTime = 0.3f;

        private void OnTriggerEnter(Collider other) {
            bool playerTrigger = other.CompareTag("Player");

            if(playerTrigger) {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad == -1) {
                Debug.LogError("Scene to load not set");
                yield break;
            }

            // keep portal game object
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            // fade out actual scene and then load next scene
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            // after loading the scene
            // get next portal (portal from another scene)
            Portal otherPortal = getOtherPortal();
            // set player position eqauls to portal spawn point position
            updatePlayer(otherPortal);

            // wait little bit and then fade in new scene
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeIntTime);

            // destroy portal game object
            Destroy(gameObject);
        }

        private void updatePlayer(Portal otherPortal)
        {
            // spawn point from other portal
            // place, where the player will be moved
            Transform spawnPoint = otherPortal.spawnPoint;

            if(spawnPoint == null) {
                Debug.LogError("Spawn point not set");
                return;
            }

            GameObject player = GameObject.FindWithTag("Player");

            // sets the position and rotation of the player the same as the position and rotation of the spawn point
            player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }

        private Portal getOtherPortal()
        {
            // list of all portals
            Portal[] portals = FindObjectsOfType<Portal>();

            // find portal with required destination
            foreach (Portal portal in portals) { 
                if (portal != this && portal.destination == destination) {
                    return portal;
                }
            }

            return null;
        }
    }
}