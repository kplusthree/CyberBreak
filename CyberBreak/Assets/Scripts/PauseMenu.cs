using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [HideInInspector]
    public TwinStickMovement pause;
    public GameObject menu;
    public gameController controller;

    void Start()
    {
        pause = GameObject.FindGameObjectWithTag("Player").GetComponent<TwinStickMovement>();
        pause.pause = true;
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        menu = controller.menu;
        menu.SetActive(false);
    }

    public void ResumeGame()
    {
      menu.SetActive(true);
      pause.pause = false;
      pause.paused = false;
      SceneManager.UnloadSceneAsync("PauseMenu");
    }

    public void ReturnMenu()
    {
      SceneManager.LoadScene("MainMenu");
    }
}
