using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    private bool endLevel;

    public PlayerController player;
    public BossController boss;
    // Start is called before the first frame update
    void Start()
    {
        endLevel = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(endLevel == false)
        {
            if (boss.gameWin == true)
            {
                // Add win state here
                Debug.Log("you win!");
                endLevel = true;
            }
            else if (player.gameOver == true)
            {
                //Add loose state here
                Debug.Log("you lose!");
                endLevel = true;
            }
        }
    }
}
