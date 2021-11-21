using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody projBody;

    // Start is called before the first frame update
    void Start()
    { 
        
    }

    void Awake()
    {
        projBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] PlayerBullets = GameObject.FindGameObjectsWithTag("PlayerBullet");
        foreach (GameObject PlayerBullet in PlayerBullets)
        {
            Physics.IgnoreCollision(PlayerBullet.GetComponent<Collider>(), GetComponent<Collider>());
        }

        GameObject[] BossBullets = GameObject.FindGameObjectsWithTag("BossBullet");
        foreach (GameObject BossBullet in BossBullets)
        {
            Physics.IgnoreCollision(BossBullet.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}
