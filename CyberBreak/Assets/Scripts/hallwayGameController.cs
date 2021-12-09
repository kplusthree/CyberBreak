using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class hallwayGameController : BaseGameController
{
    private bool endLevel;
    [HideInInspector]
    public PlayerController player;
    private int currentPlayerLives;
    private int previousLivesAmount;

    private bool setMusic;

    public AudioClip hallwayMusicClip;
    public AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        currentPlayerLives = player.lives;
        previousLivesAmount = currentPlayerLives;
        endLevel = false;
        setMusic = false;
    }

    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();

        currentPlayerLives = player.lives;

        if (setMusic == false)
        {

            musicSource.clip = hallwayMusicClip;

            musicSource.Play();

            setMusic = true;
        }

        if (currentPlayerLives != previousLivesAmount)
        {
            Debug.Log(currentPlayerLives);                  // added to player script
            previousLivesAmount = currentPlayerLives;
        }

        /*
        set to actual win conditions
        if (endLevel == false)
        {
            if (boss.gameWin == true && scene.name == "Courtyard")
            {
                Debug.Log("you win!");
                endLevel = true;
                SceneManager.LoadScene("InvestigatorCutscene");
            }
        }

        */
    }
}