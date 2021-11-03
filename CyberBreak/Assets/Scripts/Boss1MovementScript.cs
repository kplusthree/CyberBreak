using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Boss1MovementScript : MonoBehaviour
{
    [HideInInspector]
    GameObject playerObj;
    public GameObject bulletPrefab;
    private static Rigidbody bulletRB;
    private Transform target;
    [HideInInspector]
    public gameController boss;

    public Vector3 middleOfRoom;
    public Vector3 pointOne;
    public Vector3 pointTwo;
    public Vector3 pointThree;

    bool twoThirds;
    bool oneThird;
    float speed;
    float timeBetweenTeleports;
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
    bool tempTeleport;
    [HideInInspector]
    bool whichAttack;

    [HideInInspector]
    public int quadrant;

    //public AudioClip bulletClip;
    //[HideInInspector]
    //public AudioSource bulletSource;

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
        timeBetweenTeleports = 7f;
        timeBetweenAttacks = 3.5f;
        boss = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        randNum = 0;
        tempTeleport = true;
        whichAttack = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.currentBossHealth <= 66 && boss.currentBossHealth > 33)
        {
            twoThirds = true;
        }
        else if (boss.currentBossHealth <= 33)
        {
            twoThirds = false;
            oneThird = true;
        }

        if (tempTeleport == true)
        {
            StartCoroutine(Teleport());
        }

        if (attack == false && twoThirds == true)
        {
            timeBetweenTeleports = 4f;
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
            // boss looks at player
            transform.LookAt(target);
        }

        // let's the boss know what quadrant the player is in
        rawDirection = transform.rotation;
        direction = rawDirection.eulerAngles;
        degreeFacing = direction.y;
    }

    // Teleports the boss to one of 3 spots
    IEnumerator Teleport()
    {
        tempTeleport = false;

        // create random number between 1 and 3
        randNum = Random.Range(1, 3);

        // don't teleport until time has passed
        yield return new WaitForSeconds(timeBetweenTeleports * speed);

        // teleport to the point generated randomly
        if (randNum == 1)
        {
            transform.position = pointOne;
        }
        else if (randNum == 2)
        {
            transform.position = pointTwo;
        }
        else
        {
            transform.position = pointThree;
        }

        // wait until any attacks have finished
        yield return new WaitUntil(() => attack == false);

        tempTeleport = true;

        yield return null;
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
            StartCoroutine(SemiCircleAttack(degreeFacing));
        }
        else
        {
            StartCoroutine(FullAttack(degreeFacing));
        }

        // wait until any attacks have finished
        yield return new WaitUntil(() => attack == false);

        whichAttack = true;

        yield return null;
    }

    // Figures out which quadrant the player is currently in
    // 1 being the top and rotating clockwise
    int Quadrant(float degreeFacing)
    {
        int quadrant = 0;

        if (degreeFacing <= 45 || degreeFacing > 315)
        {
            quadrant = 1;
        }
        else if (degreeFacing > 45 && degreeFacing <= 135)
        {
            quadrant = 2;
        }
        else if (degreeFacing > 135 && degreeFacing <= 225)
        {
            quadrant = 3;
        }
        else if (degreeFacing > 225 && degreeFacing <= 315)
        {
            quadrant = 4;
        }

        return quadrant;
    }

    // Fires 3 shots directly at the player
    IEnumerator SmallAttack()
    {
        attack = true;
        int attackTimes = 3;

        // launch the bullets at the player
        for (int i = 0; i < attackTimes; i++)
        {
            Launch();
            yield return new WaitForSeconds(0.2f * speed);
        }

        yield return new WaitForSeconds(1.0f);

        GameObject[] bossBullets = GameObject.FindGameObjectsWithTag("BossBullet");

        // delete the bullets after
        foreach (GameObject BossBullet in bossBullets)
        {
            Destroy(BossBullet);
        }

        attack = false;
    }

    // Fires Bullet
    void Launch()
    {
        GameObject BossBullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        //bulletSource.clip = bulletClip;
        //bulletSource.Play();
        bulletRB = BossBullet.GetComponent<Projectile>().GetComponent<Rigidbody>();
        bulletRB.AddForce(transform.forward * 10, ForceMode.Impulse);
    }

    // Fires a line of bullets in an arc/semicircle towards the player
    IEnumerator SemiCircleAttack(float degreeFacing)
    {
        attack = true;
        StartCoroutine(CreateQuadOfBullets(degreeFacing, 1, 0, false));

        // wait until attack has finished
        yield return new WaitUntil(() => tempAttack);
        attack = false;

        yield return null;
    }

    IEnumerator BigAttack(float degreeFacing)
    {
        transform.position = middleOfRoom;
        attack = true;
        quadrant = Quadrant(degreeFacing);

        for (int i = 0; i < 4; i++)
        {
            StartCoroutine(CreateQuadOfBullets(degreeFacing, 4, quadrant, false));
            if (quadrant == 4)
            {
                quadrant = 1;
            }
            else
            {
                quadrant++;
            }

            yield return new WaitUntil(() => tempAttack);
        }

        if (oneThird == true)
        {
            oneThird = false;
        }
        if (twoThirds == true)
        {
            twoThirds = false;
        }

        attack = false;
        yield return null;
    }

    // Fires a line of bullets in an arc/semicircle towards the player
    IEnumerator FullAttack(float degreeFacing)
    {
        attack = true;
        quadrant = Quadrant(degreeFacing);
        StartCoroutine(CreateQuadOfBullets(degreeFacing, 1, quadrant, true));

        // wait until attack has finished
        yield return new WaitUntil(() => tempAttack);
        attack = false;

        yield return null;
    }

    // Determines where the first bullet should be spwaned in a 90 degree arc
    float Spacing(int amountOfBullets, int quadrants)
    {
        float degreesBetween = (90f * quadrants) / amountOfBullets;
        float startingSpawn;

        // determines if the bullets should be centered off of an even or odd distribution
        if (amountOfBullets % 2 == 1)
        {
            float halfSpace = (amountOfBullets - 1f) / 2f;
            startingSpawn = 45f - (degreesBetween * halfSpace);
        }
        else
        {
            float halfSpace = amountOfBullets / 2f;
            startingSpawn = 90f / halfSpace;
        }

        return startingSpawn;
    }

    float RotationDifference(float degreeFacing, int quadrant)
    {
        float futureAngle = 0;
 
        if (quadrant == 1)
        {
            futureAngle = 0f;
        }
        else if (quadrant == 2)
        {
            futureAngle = 90f;
        }
        else if (quadrant == 3)
        {
            futureAngle = 180f;
        }
        else if (quadrant == 4)
        {
            futureAngle = 270f;
        }

        //futureAngle = futureAngle - degreeFacing;
        return futureAngle;
    }

    // Creates one or multiple layers of bullets for various attacks
    IEnumerator CreateQuadOfBullets(float degreeFacing, int layers, int quadrant, bool fullCirlce)
    {
        tempAttack = false;
        int amountOfBullets;
        int layerBuffer = 2;
        int quadrants;
        float degreesBetween;
        float angleDifference;
        float startingSpawn = 0;
        Vector3 quadRotation;

        // determines whether we'll be working with one quadrant or the whole circle
        if (fullCirlce == false)
        {
            quadrants = 1;
        }
        else
        {
            quadrants = 4;
        }


        // outer for loop for which layer you're in
        for (int i = 0; i < layers; i++)
        {
            // determine amountOfBullets
            if (layers > 1)
            {
                amountOfBullets = (i * 2) + 1;
                layerBuffer = 1;
            }
            else if (fullCirlce == false)
            {
                amountOfBullets = 5;
            }
            else
            {
                amountOfBullets = 20;
            }

            degreesBetween = (90f * quadrants) / amountOfBullets;

            if (quadrant > 0)
            {
                // figure out rotation of quadrant
                angleDifference = RotationDifference(degreeFacing, quadrant);
                quadRotation = new Vector3(0, angleDifference, 0);
                transform.eulerAngles = quadRotation;
            }

            // determine where the first bullet in a layer should spawn
            startingSpawn = Spacing(amountOfBullets, quadrants);

            // rotate the boss to the earliest possible spot the bullet can spawn
            transform.Rotate(0f, -45, 0f, Space.Self);
            transform.Rotate(0f, startingSpawn, 0f, Space.Self);

            // inner for loop for spawning individual bullets
            for (int j = 0; j < amountOfBullets; j++)
            {
                GameObject BulletObject = Instantiate(bulletPrefab, transform.position + transform.forward * (layerBuffer * (i + 1)), transform.rotation);
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

            if (layers == 1 && fullCirlce == false)
            {
                yield return new WaitForSeconds(0.2f * speed);
            }
        }

        if (fullCirlce == true)
        {
            // turn the bullets
            for (int k = 0; k < 5; k++)
            {
                yield return new WaitForSeconds(0.1f);

                foreach (GameObject BossBullet in bossBullets)
                {
                    bulletRB = BossBullet.GetComponent<Projectile>().GetComponent<Rigidbody>();

                    bulletRB.transform.Rotate(0f, 45, 0f, Space.Self);

                    bulletRB.velocity += bulletRB.transform.forward * 10;
                }
            }
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
