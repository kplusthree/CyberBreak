﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    private bool endLevel;

    public PlayerController player;
    private int currentPlayerLives;
    private int previousLivesAmount;

    public BossController boss;
    private int currentBossHealth;
    private int previousHealthAmount;

    // Start is called before the first frame update
    void Start()
    {
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
            Debug.Log(currentBossHealth);                   // Update boss health ui here
            previousHealthAmount = currentBossHealth;
        }
        if (currentPlayerLives != previousLivesAmount)
        {
            Debug.Log(currentPlayerLives);                  // Update player health ui here
            previousLivesAmount = currentPlayerLives;
        }

        if(endLevel == false)
        {
            if (boss.gameWin == true)
            {
                Debug.Log("you win!");                      // Add win state here
                endLevel = true;
            }
            else if (player.gameOver == true)
            {
                Debug.Log("you lose!");                     //Add loose state here
                endLevel = true;
            }
        }
    }
}
