using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WardenCloneMovement : MonoBehaviour
{
    [HideInInspector]
    public Vector3 spawnLocation;
    [HideInInspector]
    public Vector3 attackLocation;
    [HideInInspector]
    public bool attack;
    [HideInInspector]
    public bool moving;
    [HideInInspector]
    public bool inFirstPosition;
    public bool inSecondPosition;

    [HideInInspector]
    GameObject playerObj;
    public GameObject bulletPrefab;
    private static Rigidbody bulletRB;
    private Transform target;
    [HideInInspector]
    public gameController warden;

    [HideInInspector]
    public Quaternion rawDirection;
    [HideInInspector]
    public Vector3 direction;
    [HideInInspector]
    float degreeFacing;
    [HideInInspector]
    bool tempAttack;
    float speed;

    Animator anim;

    public UnityEngine.AI.NavMeshAgent wardenAgent;

    public AudioClip bulletClip;
    public AudioClip popClip;
    [HideInInspector]
    public AudioSource bulletSource;
    [HideInInspector]
    public AudioSource popSource;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.Find("Player");
        degreeFacing = 0;
        speed = 1f;
        attack = false;
        moving = false;
        tempAttack = false;
        inFirstPosition = false;
        inSecondPosition = false;
        warden = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        bulletSource = GameObject.FindGameObjectWithTag("BulletSource").GetComponent<AudioSource>();
        popSource = GameObject.FindGameObjectWithTag("PopSource").GetComponent<AudioSource>();
        bulletSource.clip = bulletClip;
        popSource.clip = popClip;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // player location. moved to beginning of update to make sure things that use this info have the right info.
        target = playerObj.transform;

        if (attack == false)
        {
            // boss looks at player
            transform.LookAt(target);
        }

        if (moving == true)
        {
            anim.SetInteger("State", 1);
        }
        else if (attack == false)
        {
            anim.SetInteger("State", 0);
        }

        // let's the boss know what quadrant the player is in
        rawDirection = transform.rotation;
        direction = rawDirection.eulerAngles;
        degreeFacing = direction.y;

        // checks if boss is in the right spot
        CheckBossPosition();
    }

    void CheckPlayerLocation()
    {
        transform.LookAt(target);
        rawDirection = transform.rotation;
        direction = rawDirection.eulerAngles;
        degreeFacing = direction.y;

        return;
    }

    void CheckBossPosition()
    {
        if (Vector3.Distance(transform.position, spawnLocation) < 0.5f)
        {
            inFirstPosition = true;
        }
        else
        {
            inFirstPosition = false;
        }

        if (Vector3.Distance(transform.position, attackLocation) < 0.5f)
        {
            inSecondPosition = true;
        }
        else
        {
            inSecondPosition = false;
        }
    }

    public IEnumerator GoToPosition()
    {
        anim.SetInteger("State", 4);
        wardenAgent.SetDestination(spawnLocation);

        yield return null;
    }

    public IEnumerator GoToAttackPosition()
    {
        moving = true;
        wardenAgent.SetDestination(attackLocation);

        yield return null;
    }

    public IEnumerator Shoot()
    {
        attack = true;

        CheckPlayerLocation();
        anim.SetInteger("State", 3);
        StartCoroutine(CreateQuadOfBullets(degreeFacing, 5));

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
        //moved this to before the wait so boss has less downtime
        tempAttack = true;
        yield return new WaitForSeconds(2.0f);

        foreach (GameObject BossBullet in bossBullets)
        {
            Destroy(BossBullet);
        }

        attack = false;

        //tempAttack = true;

        yield return null;
    }
}
