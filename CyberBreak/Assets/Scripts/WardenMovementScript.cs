﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WardenMovementScript : MonoBehaviour
{
    [HideInInspector]
    GameObject playerObj;
    public GameObject bulletPrefab;
    private static Rigidbody bulletRB;
    private Transform target;
    [HideInInspector]
    public gameController warden;

    public Vector3 middleOfRoom;
    public Vector3 pointOne;
    public Vector3 pointTwo;
    public Vector3 pointThree;

    bool twoThirds;
    bool oneThird;
    float speed;
    float timeBetweenAttacks;
    int randNum;

    [HideInInspector]
    public Quaternion rawDirection;
    [HideInInspector]
    public Vector3 direction;
    [HideInInspector]
    float degreeFacing;
    [HideInInspector]
    bool attack;
    [HideInInspector]
    bool tempAttack;
    [HideInInspector]
    bool whichAttack;

    [HideInInspector]
    public int quadrant;

    public AudioClip bulletClip;
    public AudioClip deathClip;
    public AudioClip popClip;
    public AudioSource bulletSource;
    public AudioSource deathSource;
    public AudioSource popSource;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.Find("Player");
        degreeFacing = 0;
        quadrant = 0;
        attack = false;
        twoThirds = false;
        oneThird = false;
        speed = 1f;
        timeBetweenAttacks = 3.5f;
        warden = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        randNum = 0;
        whichAttack = true;
        bulletSource.clip = bulletClip;
        deathSource.clip = deathClip;
        popSource.clip = popClip;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (warden.currentBossHealth <= 66 && warden.currentBossHealth > 33)
        {
            twoThirds = true;
        }
        else if (warden.currentBossHealth <= 33)
        {
            twoThirds = false;
            oneThird = true;
        }
        else if (warden.currentBossHealth <= 0)
        {
            deathSource.Play();
        }

        if (attack == false && twoThirds == true)
        {
            StartCoroutine(BigAttack(degreeFacing));
        }
        else if (attack == false && oneThird == true)
        {
            speed = 0.5f;
            StartCoroutine(BigAttack(degreeFacing));
        }
        else if (attack == false && whichAttack == true)
        {
            StartCoroutine(ChooseAttack());
        }

        // player location
        target = playerObj.transform;

        if (attack == false)
        {
            anim.SetInteger("State", 0);
            // boss looks at player
            transform.LookAt(target);
        }

        // let's the boss know what quadrant the player is in
        rawDirection = transform.rotation;
        direction = rawDirection.eulerAngles;
        degreeFacing = direction.y;
    }

    // Teleports the boss to one of 3 spots
    IEnumerator ChooseAttack()
    {
        whichAttack = false;

        // create random number between 1 and 3
        randNum = Random.Range(1, 3);

        // don't teleport until time has passed
        yield return new WaitForSeconds(timeBetweenAttacks * speed);

        // teleport to the point generated randomly
        if (randNum == 1)
        {
            StartCoroutine(SmallAttack());
        }
        else if (randNum == 2)
        {
            StartCoroutine(TopAttack(degreeFacing));
        }
        else
        {
            StartCoroutine(UpDownAttack(degreeFacing));
        }

        // wait until any attacks have finished
        yield return new WaitUntil(() => attack == false);

        whichAttack = true;

        yield return null;
    }

    // Fires 3 shots directly at the player
    IEnumerator SmallAttack()
    {
        attack = true;
        anim.SetInteger("State", 1);

        StartCoroutine(CreateQuadOfBullets(degreeFacing, 3));

        // wait until attack has finished
        yield return new WaitUntil(() => tempAttack);
        attack = false;

        yield return null;
    }

    // Fires rows of bullets from the top of the screen
    IEnumerator TopAttack(float degreeFacing)
    {
        attack = true;
        StartCoroutine(CreateQuadOfBullets(degreeFacing, 5));
        anim.SetInteger("State", 2);

        // wait until attack has finished
        yield return new WaitUntil(() => tempAttack);
        attack = false;

        yield return null;
    }

    // Fires rows of bullets from the top and bottom of the screen
    IEnumerator UpDownAttack(float degreeFacing)
    {
        attack = true;
        StartCoroutine(CreateQuadOfBullets(degreeFacing, 5));
        anim.SetInteger("State", 2);

        // wait until attack has finished
        yield return new WaitUntil(() => tempAttack);
        attack = false;

        yield return null;
    }

    IEnumerator BigAttack(float degreeFacing)
    {
        attack = true;
        transform.position = middleOfRoom;
        anim.SetInteger("State", 3);

        if (oneThird == true)
        {
            oneThird = false;
        }
        if (twoThirds == true)
        {
            twoThirds = false;
        }

        // wait until attack has finished
        yield return new WaitUntil(() => tempAttack);
        attack = false;

        yield return null;
    }

    // Determines where the first bullet should be spwaned in a 90 degree arc
    float Spacing(int amountOfBullets)
    {
        float degreesBetween = 50f / amountOfBullets;
        float startingSpawn;

        // determines if the bullets should be centered off of an even or odd distribution
        if (amountOfBullets % 2 == 1)
        {
            float halfSpace = (amountOfBullets - 1f) / 2f;
            startingSpawn = 25f - (degreesBetween * halfSpace);
        }
        else
        {
            float halfSpace = amountOfBullets / 2f;
            startingSpawn = 50f / halfSpace;
        }

        return startingSpawn;
    }

    // Creates one or multiple layers of bullets for various attacks
    IEnumerator CreateQuadOfBullets(float degreeFacing, int layers)
    {
        tempAttack = false;
        int amountOfBullets;
        float layerBuffer = 0.75f;
        float degreesBetween;
        float startingSpawn = 0;
        Vector3 startQuad = new Vector3(0, degreeFacing, 0);

        // outer for loop for which layer you're in
        for (int i = 0; i < layers; i++)
        {
            if (i > 3)
            {
                layerBuffer = 1f;
            }

            amountOfBullets = (i * 2) + 1;

            degreesBetween = 50f / amountOfBullets;

            transform.eulerAngles = startQuad;

            // determine where the first bullet in a layer should spawn
            startingSpawn = Spacing(amountOfBullets);

            // rotate the boss to the earliest possible spot the bullet can spawn
            transform.Rotate(0f, -25f, 0f, Space.Self);
            transform.Rotate(0f, startingSpawn, 0f, Space.Self);

            // inner for loop for spawning individual bullets
            for (int j = 0; j < amountOfBullets; j++)
            {
                if (i < 4)
                {
                    GameObject BulletObject = Instantiate(bulletPrefab, transform.position + transform.forward * (layerBuffer * (i + 1)), transform.rotation);
                }
                else
                {
                    GameObject BulletObject = Instantiate(bulletPrefab, transform.position + transform.forward * (layerBuffer * i), transform.rotation);
                }
                popSource.Play();
                transform.Rotate(0f, degreesBetween, 0f, Space.Self);

                yield return new WaitForSeconds(0.1f * speed);
            }

            yield return new WaitForSeconds(0.1f * speed);
        }

        GameObject[] bossBullets = GameObject.FindGameObjectsWithTag("BossBullet");

        // fire each bullet
        foreach (GameObject BossBullet in bossBullets)
        {
            bulletRB = BossBullet.GetComponent<Projectile>().GetComponent<Rigidbody>();
            bulletRB.AddForce(BossBullet.transform.forward * 10, ForceMode.Impulse);
            bulletSource.Play();
        }

        yield return new WaitForSeconds(1.0f);

        foreach (GameObject BossBullet in bossBullets)
        {
            Destroy(BossBullet);
        }

        tempAttack = true;

        yield return null;
    }
}
