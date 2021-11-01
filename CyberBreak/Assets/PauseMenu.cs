using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
      public GameObject GameUI;
      public GameObject PauseUI;

      void Start()
      {
          PauseUI = transform.GetChild(0).gameObject;

      }
      void Update()
      {
          if(Input.GetKeyDown(KeyCode.Escape))
          {
            PauseGame();
          }
      }

      public void PauseGame()
      {
          Time.timeScale = 0f;
      }

      public void ResumeGame()
      {
        Time.timeScale = 1;
      }
}
