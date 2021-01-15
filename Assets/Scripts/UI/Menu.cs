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
        private bool finished = false;
        private String menuText = "SIMPLE RPG GAME";
        private String pauseText = "PAUSED";
        private String finishText = "FINISHED";
        private String playButton = "PLAY";
        private String resumeButton = "RESUME";
        private String finishButton = "NEW GAME";
        
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

        public bool IsFinished()
        {
            return finished;
        }

        public void FinishGame()
        {
            // stop game
            Time.timeScale = 0;
            
            inGame = false;
            finished = true;
            
            menuTitle.text = finishText;
            playButtonText.text = finishButton;
            
            ShowMenu();
        }

        public void Play()
        {
            print("play");
            print(inGame);
            if(!inGame || finished) {
                // if player is not in the game (or game is finished), then load the first level
                StartCoroutine(LoadGame());
            } else if(paused) {
                Resume();
            }
        }

        public void ShowMenu()
        {
            GetComponent<CanvasGroup>().alpha = 1;
        }

        public void HideMenu()
        {
            GetComponent<CanvasGroup>().alpha = 0;
        }

        public void Pause()
        {
            paused = true;
            // stop game
            Time.timeScale = 0;
            ShowMenu();
        }

        public void Resume()
        {
            paused = false;
            // resume game
            Time.timeScale = 1;
            HideMenu();
        }

        public void Exit()
        {
            Application.Quit();
        }

        private IEnumerator LoadGame()
        {
            int sceneToLoad = 1;

            print(gameObject.name);

            // keep menu game object
            // if(!finished)
                DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();

            // fade out actual scene and then load the first level
            yield return fader.FadeOut(fadeOutTime);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            // update menu
            inGame = true;
            finished = false;
            menuTitle.text = pauseText;
            playButtonText.text = resumeButton;
            HideMenu();

            // wait little bit and then fade in the first level
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeIntTime);
        }
    }
}