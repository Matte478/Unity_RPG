using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad = -1;
        private void OnTriggerEnter(Collider other) {
            bool playerTrigger = other.CompareTag("Player");

            if(playerTrigger) {
                print("portal triggered");
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}