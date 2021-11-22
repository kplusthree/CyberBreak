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

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        gameWin = false;
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
        }
    }

    void TakeDamage(int damage)
    {
        health -= damage;
    }
}
