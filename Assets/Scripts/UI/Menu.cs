using System;
using System.Collections;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RPG.UI
{
    public class Menu : MonoBehaviour 
    {   
        [SerializeField] float fadeOutTime = 0.5f;
        [SerializeField] float fadeIntTime = 0.5f;
        [SerializeField] float fadeWaitTime = 0.3f;
        [SerializeField] Text menuTitle;
        [SerializeField] Text playButtonText;

        private bool inGame = false;
        private bool paused = false;
        private String menuText = "SIMPLE RPG GAME";
        private String pauseText = "PAUSED";
        private String playButton = "PLAY";
        private String resumeButton = "RESUME";
        
        void Start()
        {
            menuTitle.text = menuText;
            playButtonText.text = playButton;
            GetComponent<CanvasGroup>().alpha = 1;
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                // if player is in the game, then control menu with escape key
                if(inGame) {
                    if(paused) {
                        Resume();
                    } else {
                        Pause();
                    }
                }
            }
        }

        public bool IsPaused()
        {
            return paused;
        }

        public void Play()
        {
            if(!inGame) {
                // if player is not in the game, then load the first level
                StartCoroutine(LoadGame());
            } else {
                Resume();
            }
        }

        public void Pause()
        {
            paused = true;
            Time.timeScale = 0;
            GetComponent<CanvasGroup>().alpha = 1;
        }

        public void Resume()
        {
            paused = false;
            Time.timeScale = 1;
            GetComponent<CanvasGroup>().alpha = 0;
        }

        public void Exit()
        {
            Application.Quit();
        }

        private IEnumerator LoadGame()
        {
            int sceneToLoad = 1;

            // keep menu game object
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            // fade out actual scene and then load the first level
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            // update menu
            inGame = true;
            menuTitle.text = pauseText;
            playButtonText.text = resumeButton;
            GetComponent<CanvasGroup>().alpha = 0;

            // wait little bit and then fade in the first level
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeIntTime);
        }
    }
}