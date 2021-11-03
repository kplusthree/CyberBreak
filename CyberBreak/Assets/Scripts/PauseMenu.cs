using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [HideInInspector]
    public TwinStickMovement pause;

    void Start()
    {
        pause = GameObject.FindGameObjectWithTag("Player").GetComponent<TwinStickMovement>();
        pause.pause = true;
    }

    public void ResumeGame()
    {
      pause.pause = false;
      SceneManager.UnloadSceneAsync("PauseMenu");
    }

    public void ReturnMenu()
    {
      SceneManager.LoadScene("MainMenu");
    }
}
