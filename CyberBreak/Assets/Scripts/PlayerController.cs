using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int lives;
    public GameObject[] hearts;
    [HideInInspector]

    public bool gameOver;

    public AudioClip damageClip;
    public AudioClip deathClip;
    public AudioSource damageSource;

    // Start is called before the first frame update
    void Start()
    {
        lives = hearts.Length;
        gameOver = false;
        damageSource.clip = damageClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (lives <= 0)
        {
            damageSource.clip = deathClip;
            damageSource.Play();
            gameOver = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BossBullet")
        {
            damageSource.Play();
            lives = lives - 1;
            Destroy(hearts[lives].gameObject);
            Destroy(collision.gameObject);
        }
    }
}
