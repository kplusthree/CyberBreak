using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        currentPlayerLives = player.lives;
        previousLivesAmount = currentPlayerLives;
        currentBossHealth = boss.health;
        previousHealthAmount = currentBossHealth;
        endLevel = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentPlayerLives = player.lives;
        currentBossHealth = boss.health;

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
            if (boss.gameWin == true)
            {
                Debug.Log("you win!");
                endLevel = true;
                SceneManager.LoadScene("GameOver");
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