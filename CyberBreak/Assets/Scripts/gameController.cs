using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{
    Scene scene;

    private bool endLevel;
    [HideInInspector]
    public PlayerController player;
    private int currentPlayerLives;
    private int previousLivesAmount;

    [HideInInspector]
    public BossController boss;
    [HideInInspector]
    public int currentBossHealth;
    private int previousHealthAmount;
    private bool setMusic;

    public AudioClip tutorialMusicClip;
    public AudioClip Boss1MusicClip;
    public AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        KeepData.keepLevelName = SceneManager.GetActiveScene().name;
        currentPlayerLives = player.lives;
        previousLivesAmount = currentPlayerLives;
        currentBossHealth = boss.health;
        previousHealthAmount = currentBossHealth;
        endLevel = false;
        setMusic = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerLives = player.lives;
        currentBossHealth = boss.health;

        if (setMusic == false)
        {

            if (scene.name == "Courtyard")
            {
                musicSource.clip = tutorialMusicClip;
            }
            else
            {
                musicSource.clip = Boss1MusicClip;
            }

            musicSource.Play();

            setMusic = true;
        }

        if (currentBossHealth != previousHealthAmount)
        {
            Debug.Log(currentBossHealth);                   // added to boss script
            previousHealthAmount = currentBossHealth;
        }
        if (currentPlayerLives != previousLivesAmount)
        {
            Debug.Log(currentPlayerLives);                  // added to player script
            previousLivesAmount = currentPlayerLives;
        }

        if(endLevel == false)
        {
            if (boss.gameWin == true && scene.name == "Courtyard")
            {
                Debug.Log("you win!");
                endLevel = true;
                SceneManager.LoadScene("HallwayOne");
            }
            else if (boss.gameWin == true && scene.name == "EvidenceLocker")
            {
                Debug.Log("you win!");
                endLevel = true;
                SceneManager.LoadScene("WardenCutScene");
            }
            else if (boss.gameWin == true && scene.name == "WardensOffice")
            {
                Debug.Log("you win!");
                endLevel = true;
                SceneManager.LoadScene("FinalCutscene");
            }
            else if (player.gameOver == true)
            {
                Debug.Log("you lose!");
                endLevel = true;
                SceneManager.LoadScene("GameOver");
            }
        }
    }
}