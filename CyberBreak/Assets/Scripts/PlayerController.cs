using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int lives;
    public GameObject[] hearts;
    [HideInInspector]

    public bool gameOver;

    // Start is called before the first frame update
    void Start()
    {
        lives = hearts.Length;
        gameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (lives <= 0)
        {
            gameOver = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BossBullet")
        {
            lives = lives - 1;
            Destroy(hearts[lives].gameObject);
            Destroy(collision.gameObject);
        }
    }
}
