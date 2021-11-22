using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideScrollerEnemyController : MonoBehaviour
{
    [HideInInspector]
    public int health;
    [HideInInspector]
    public int maxHealth = 10;
    public bool gameWin;

    public AudioClip deathClip;
    public AudioSource deathSource;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        gameWin = false;
        deathSource.clip = deathClip;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            gameWin = true;
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            //health = health - 5;
            TakeDamage(10);
            Destroy(collision.gameObject);
            deathSource.Play();
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
    }
}
