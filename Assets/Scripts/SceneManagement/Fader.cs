using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator FadeOutIn()
        {
            // wait until FadeOut() is and then call and wait for FadeIn()
            yield return FadeOut(1f);
            yield return FadeIn(1f);
        }

        public IEnumerator FadeOut(float time)
        {
             while (canvasGroup.alpha < 1) {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
             }
        }

        public IEnumerator FadeIn(float time)
        {
             while (canvasGroup.alpha > 0) {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
             }
        }
    }
}