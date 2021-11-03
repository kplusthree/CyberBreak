using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu2 : MonoBehaviour
{
      public GameObject pauseMenuUI;

      public static bool GameIsPaused = false;

      void Update()
      {
          if(Gamepad.current.aButton.wasPressedThisFrame)
          { 
            if (GameIsPaused)
            {
              ResumeGame();
            }
            else
            {
              PauseGame();
            }
          }
      }

      public void PauseGame()
      {
          Time.timeScale = 0f;
          pauseMenuUI.SetActive(true);
          GameIsPaused = true;
      }

      public void ResumeGame()
      {
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
      }

         public void ReturnMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
