using System;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.UI
{
    public class Menu : MonoBehaviour 
    {   
        private bool paused = false;
        
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                if(paused) {
                    Resume();
                } else {
                    Pause();
                }
            }
        }

        public bool IsPaused()
        {
            return paused;
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
    }
}