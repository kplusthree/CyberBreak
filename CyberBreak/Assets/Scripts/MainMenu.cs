using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadGame() 
    {
        SceneManager.LoadScene("PreTutorialCutscene");
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame() 
    {
        Application.Quit();
        Debug.Log("Quit!");
    }

    public void LevelRestart()
    {
        SceneManager.LoadScene(KeepData.keepLevelName, LoadSceneMode.Single);
    }
}
