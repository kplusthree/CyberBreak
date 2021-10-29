using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public int health;
    [HideInInspector]
    public bool gameWin;
    public HealthBarBehavior HealthBar;
    public int MaxHealth = 20;


    // Start is called before the first frame update
    void Start()
    {
        health = MaxHealth;
        HealthBar.SetHealth(health, MaxHealth);
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
            health = health - 5;
            Destroy(collision.gameObject);
        }
    }
}
