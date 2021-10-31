using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    [HideInInspector]
    public int health;
    [HideInInspector]
    public int maxHealth = 100;
    public bool gameWin;
    public Slider slider;


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        slider.value = CalculateHealth();
        gameWin = false;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = CalculateHealth();
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

    int CalculateHealth ()
    {
        return health / maxHealth;
    }
}
