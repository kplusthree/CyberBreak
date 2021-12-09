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
    public BaseGameController controller;

    void Start()
    {
        Reconfigure();
        SetMenuActiveStatus(false);
    }

    public void ResumeGame()
    {
      Reconfigure();
      SetMenuActiveStatus(true);
      pause.pause = false;
      pause.paused = false;
      SceneManager.UnloadSceneAsync("PauseMenu");
    }

    public void ReturnMenu()
    {
      SceneManager.LoadScene("MainMenu");
    }

    private void Reconfigure()
    {
        pause = GameObject.FindGameObjectWithTag("Player").GetComponent<TwinStickMovement>();
        pause.pause = true;
        try
        {
            controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<BaseGameController>();
            menu = controller.menu;
        } catch(System.NullReferenceException ex)
        {
            Debug.LogWarning("No Game Controller in scene");
        }
    }

    public void SetMenuActiveStatus(bool status)
    {
        if(menu != null)
        {
            menu.SetActive(status);
        }

        else
        {
            Debug.LogWarning("Menu is null pause can't change menu status");
        }
    }
}
