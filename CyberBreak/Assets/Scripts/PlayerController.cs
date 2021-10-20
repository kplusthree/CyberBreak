using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int health;
    [HideInInspector]
    bool death;

    // Start is called before the first frame update
    void Start()
    {
        health = 20;
        death = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            death = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BossBullet")
        {
            health = health - 5;
            Destroy(collision.gameObject);
        }
    }
}
