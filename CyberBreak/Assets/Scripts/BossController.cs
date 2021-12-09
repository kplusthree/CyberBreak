using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [HideInInspector]
    public int health;
    //[HideInInspector]
    public int maxHealth = 200;
    public bool gameWin;

    public HealthBar healthBar;
    //public GameObject floatingText;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        gameWin = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (health == 50)
        //{
        //    floatingText.SetActive(true);
        //}

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
            TakeDamage(5);
            Destroy(collision.gameObject);
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.SetHealth(health);
    }
}
