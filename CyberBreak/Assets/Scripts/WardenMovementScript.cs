using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class WardenMovementScript : MonoBehaviour
{
    [HideInInspector]
    GameObject playerObj;
    public GameObject bulletPrefab;
    public GameObject bulletTrianglePrefab;
    public GameObject clonePrefab;
    GameObject BulletTriangleObject;
    private static Rigidbody bulletRB;
    private Transform target;
    [HideInInspector]
    public gameController warden;

    public Vector3 middleOfRoom;
    public Vector3 cloneSummon;
    public Vector3 pointOne;
    public Vector3 pointTwo;
    public Vector3 pointThree;
    public Vector3 pointFour;
    public Vector3 cloneOnePos;
    public Vector3 cloneTwoPos;
    public Vector3 cloneThreePos;
    public Vector3 cloneFourPos;
    public Vector3 cloneOnePosTwo;
    public Vector3 cloneTwoPosTwo;
    public Vector3 cloneThreePosTwo;
    public Vector3 cloneFourPosTwo;

    [HideInInspector]
    public bool middleCheck;
    [HideInInspector]
    public bool cloneCheck;
    [HideInInspector]
    public bool pointOneCheck;
    [HideInInspector]
    public bool pointTwoCheck;
    [HideInInspector]
    public bool pointThreeCheck;
    [HideInInspector]
    public bool pointFourCheck;

    bool twoThirds;
    bool oneThird;
    bool rotateTriangle;
    float speed;
    float timeBetweenAttacks;
    int randNum;

    [HideInInspector]
    public Quaternion rawDirection;
    [HideInInspector]
    public Vector3 direction;
    [HideInInspector]
    float degreeFacing;
    //[HideInInspector]
    bool attack;
    bool idleWhileMoving;
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

    public NavMeshAgent wardenAgent;

    [HideInInspector]
    GameObject cloneOneRef;
    [HideInInspector]
    public WardenCloneMovement cloneOne;
    [HideInInspector]
    GameObject cloneTwoRef;
    [HideInInspector]
    public WardenCloneMovement cloneTwo;
    [HideInInspector]
    GameObject cloneThreeRef;
    [HideInInspector]
    public WardenCloneMovement cloneThree;
    [HideInInspector]
    GameObject cloneFourRef;
    [HideInInspector]
    public WardenCloneMovement cloneFour;

    // Flag to check if the boss has used their big attack
    bool hasEverUsedBigAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.Find("Player");
        degreeFacing = 0;
        quadrant = 0;
        attack = false;
        idleWhileMoving = false;
        twoThirds = false;
        oneThird = false;
        rotateTriangle = false;
        speed = 1f;
        timeBetweenAttacks = 2f;
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
        // player location. moved to beginning of update to make sure things that use this info have the right info.
        target = playerObj.transform;

        if (attack == false)
        {
            // boss looks at player
            transform.LookAt(target);
        }

        if (idleWhileMoving == true)
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
        int oneThirdsHP = Mathf.CeilToInt(warden.startingBossHealth * 0.33f);
        int twoThirdsHP = Mathf.CeilToInt(warden.startingBossHealth * 0.66f); 
        // check boss health for change in attacks
        if (warden.currentBossHealth <= twoThirdsHP && warden.currentBossHealth > oneThirdsHP)
        {
            twoThirds = true;
        }
        else if (warden.currentBossHealth <= oneThirdsHP && oneThird == false)
        {
            twoThirds = false;
            oneThird = true;
            hasEverUsedBigAttack = false;
        }
        else if (warden.currentBossHealth <= 0)
        {
            deathSource.Play();
        }

        // check for when we need to attack
        if (attack == false)
        {               
            // check if we're under 2/3rds hp   
            if (twoThirds == true)
            {
                //if we've used big attack, then choose another attack.
                if (hasEverUsedBigAttack == true)
                {
                    if (whichAttack == true)
                    { //just makes sure we need to pick attack
                        StartCoroutine(ChooseAttack());
                    }
                }
                else if (hasEverUsedBigAttack == false)
                {
                    StartCoroutine(BigAttack());
                    hasEverUsedBigAttack = true; // check if we've used big attack, and if not, make sure we do, and set that we used it
                }
            }
            else if (oneThird == true)
            {
                //if we've used big attack, then choose another attack.
                if (hasEverUsedBigAttack == true)
                {
                    if (whichAttack == true)
                    { //just makes sure we need to pick attack
                        StartCoroutine(ChooseAttack());
                    }
                }
                else if (hasEverUsedBigAttack == false)
                {
                    StartCoroutine(BigAttack());
                    hasEverUsedBigAttack = true; // check if we've used big attack, and if not, make sure we do, and set that we used it
                }
            }
            else
            { //otherwise, just choose an attack if we need to.
                if (whichAttack == true)
                {
                    StartCoroutine(ChooseAttack());
                }
            }         
        }

        if (rotateTriangle == true && BulletTriangleObject != null)
        {
            BulletTriangleObject.transform.Rotate(0, 100 * Time.deltaTime, 0);
        }
    }

    void CheckBossPosition()
    {
        if (Vector3.Distance(transform.position, middleOfRoom) < 0.5f)
        {
            middleCheck = true;
        }
        else
        {
            middleCheck = false;
        }
        if (Vector3.Distance(transform.position, cloneSummon) < 0.5f)
        {
            cloneCheck = true;
        }
        else
        {
            cloneCheck = false;
        }

        if (Vector3.Distance(transform.position, pointOne) < 0.5f)
        {
            pointOneCheck = true;
        }
        else
        {
            pointOneCheck = false;
        }

        if (Vector3.Distance(transform.position, pointTwo) < 0.4f)
        {
            pointTwoCheck = true;
        }
        else
        {
            pointTwoCheck = false;
        }

        if (Vector3.Distance(transform.position, pointThree) < 0.4f)
        {
            pointThreeCheck = true;
        }
        else
        {
            pointThreeCheck = false;
        }

        if (Vector3.Distance(transform.position, pointFour) < 0.4f)
        {
            pointFourCheck = true;
        }
        else
        {
            pointFourCheck = false;
        }

        return;
    }

    // Cycles between boss's attacks
    IEnumerator ChooseAttack()
    {
        whichAttack = false;

        // create random number between 1 and 3
        randNum = Random.Range(1, 4);

        // don't teleport until time has passed
        yield return new WaitForSeconds(timeBetweenAttacks * speed);

        // teleport to the point generated randomly
        if (randNum == 1)
        {
            StartCoroutine(SmallAttack());
        }
        else if (randNum == 2)
        {
            StartCoroutine(TopAttack());
        }
        else
        {
            StartCoroutine(TriangleAttack());
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
        anim.SetInteger("State", 2);

        StartCoroutine(CreateQuadOfBullets(degreeFacing, 3));

        // wait until attack has finished
        yield return new WaitUntil(() => tempAttack);
        attack = false;

        yield return null;
    }

    void CheckPlayerLocation()
    {
        transform.LookAt(target);
        rawDirection = transform.rotation;
        direction = rawDirection.eulerAngles;
        degreeFacing = direction.y;

        return;
    }

    // Fires rows of bullets from the top of the screen
    IEnumerator TopAttack()
    {
        attack = true;

        idleWhileMoving = true;
        wardenAgent.SetDestination(pointOne);
        yield return new WaitUntil(() => pointOneCheck);
        yield return new WaitForSeconds(0.5f);
        CheckPlayerLocation();
        idleWhileMoving = false;
        anim.SetInteger("State", 3);
        StartCoroutine(CreateQuadOfBullets(degreeFacing, 5));

        // wait until attack has finished
        yield return new WaitUntil(() => tempAttack);

        idleWhileMoving = true;
        wardenAgent.SetDestination(pointTwo);
        yield return new WaitUntil(() => pointTwoCheck);
        yield return new WaitForSeconds(0.5f);
        CheckPlayerLocation();
        idleWhileMoving = false;
        anim.SetInteger("State", 3);
        StartCoroutine(CreateQuadOfBullets(degreeFacing, 5));

        yield return new WaitUntil(() => tempAttack);

        idleWhileMoving = true;
        wardenAgent.SetDestination(pointThree);
        yield return new WaitUntil(() => pointThreeCheck);
        yield return new WaitForSeconds(0.51f);
        CheckPlayerLocation();
        idleWhileMoving = false;
        anim.SetInteger("State", 3);
        StartCoroutine(CreateQuadOfBullets(degreeFacing, 5));

        yield return new WaitUntil(() => tempAttack);

        idleWhileMoving = true;
        wardenAgent.SetDestination(pointFour);
        yield return new WaitUntil(() => pointFourCheck);
        yield return new WaitForSeconds(0.5f);
        CheckPlayerLocation();
        idleWhileMoving = false;
        anim.SetInteger("State", 3);
        StartCoroutine(CreateQuadOfBullets(degreeFacing, 5));

        yield return new WaitUntil(() => tempAttack);

        // wait until attack has finished
        yield return new WaitUntil(() => tempAttack);
        attack = false;

        yield return null;
    }

    // summons bullets in the shape of a triangle and spins it towards the player
    IEnumerator TriangleAttack()
    {
        // move into position
        attack = true;
        idleWhileMoving = true;
        wardenAgent.SetDestination(middleOfRoom); //set nav mesh destination to middle of room, so nav mesh isnt fighting our forced position
        yield return new WaitUntil(() => middleCheck);
        idleWhileMoving = false;
        yield return new WaitForSeconds(1f);
        CheckPlayerLocation();

        // create first attack
        BulletTriangleObject = Instantiate(bulletTrianglePrefab, transform.position, transform.rotation);
        popSource.Play();
        rotateTriangle = true;
        yield return new WaitForSeconds(1f);

        // launch attack
        anim.SetInteger("State", 2);
        bulletRB = BulletTriangleObject.GetComponent<Rigidbody>();
        bulletRB.AddForce(BulletTriangleObject.transform.forward * degreeFacing, ForceMode.Impulse);
        bulletSource.Play();
        yield return new WaitForSeconds(1.0f);
        rotateTriangle = false;
        Destroy(BulletTriangleObject);
        CheckPlayerLocation();

        // create second attack
        BulletTriangleObject = Instantiate(bulletTrianglePrefab, transform.position, transform.rotation);
        popSource.Play();
        rotateTriangle = true;
        yield return new WaitForSeconds(1f);

        // launch attack
        anim.SetInteger("State", 2);
        bulletRB = BulletTriangleObject.GetComponent<Rigidbody>();
        bulletRB.AddForce(BulletTriangleObject.transform.forward * degreeFacing, ForceMode.Impulse);
        bulletSource.Play();
        yield return new WaitForSeconds(1.0f);
        rotateTriangle = false;
        Destroy(BulletTriangleObject);
        CheckPlayerLocation();

        // create third attack
        BulletTriangleObject = Instantiate(bulletTrianglePrefab, transform.position, transform.rotation);
        popSource.Play();
        rotateTriangle = true;
        yield return new WaitForSeconds(1f);

        // launch attack
        anim.SetInteger("State", 2);
        bulletRB = BulletTriangleObject.GetComponent<Rigidbody>();
        bulletRB.AddForce(BulletTriangleObject.transform.forward * degreeFacing, ForceMode.Impulse);
        bulletSource.Play();
        yield return new WaitForSeconds(2.0f);
        rotateTriangle = false;
        Destroy(BulletTriangleObject);

        attack = false;

        yield return null;
    }

    // Creates clones to fire rows of bullets from the top and bottom of the screen
    IEnumerator BigAttack()
    {
        attack = true;
        idleWhileMoving = true;

        wardenAgent.SetDestination(cloneSummon);
        yield return new WaitUntil(() => cloneCheck);
        idleWhileMoving = false;
        yield return new WaitForSeconds(0.5f);

        // summon clones
        anim.SetInteger("State", 4);
        cloneOneRef = Instantiate(clonePrefab, cloneSummon, transform.rotation);
        cloneOne = cloneOneRef.GetComponent<WardenCloneMovement>();
        cloneOne.spawnLocation = cloneOnePos;
        cloneTwoRef = Instantiate(clonePrefab, cloneSummon, transform.rotation);
        cloneTwo = cloneTwoRef.GetComponent<WardenCloneMovement>();
        cloneTwo.spawnLocation = cloneTwoPos;
        cloneThreeRef = Instantiate(clonePrefab, cloneSummon, transform.rotation);
        cloneThree = cloneThreeRef.GetComponent<WardenCloneMovement>();
        cloneThree.spawnLocation = cloneThreePos;
        cloneFourRef = Instantiate(clonePrefab, cloneSummon, transform.rotation);
        cloneFour = cloneFourRef.GetComponent<WardenCloneMovement>();
        cloneFour.spawnLocation = cloneFourPos;

        // move clones to first positions
        yield return new WaitForSeconds(1f);
        idleWhileMoving = true;
        StartCoroutine(cloneOne.GoToPosition());
        StartCoroutine(cloneTwo.GoToPosition());
        StartCoroutine(cloneThree.GoToPosition());
        StartCoroutine(cloneFour.GoToPosition());
        yield return new WaitUntil(() => cloneOne.inFirstPosition);
        yield return new WaitUntil(() => cloneTwo.inFirstPosition);
        yield return new WaitUntil(() => cloneThree.inFirstPosition);
        yield return new WaitUntil(() => cloneFour.inFirstPosition);
        idleWhileMoving = false;
        yield return new WaitForSeconds(1f);

        // move to attack positions
        idleWhileMoving = true;
        cloneOne.attackLocation = cloneOnePosTwo;
        cloneTwo.attackLocation = cloneTwoPosTwo;
        cloneThree.attackLocation = cloneThreePosTwo;
        cloneFour.attackLocation = cloneFourPosTwo;
        StartCoroutine(cloneOne.GoToAttackPosition());
        StartCoroutine(cloneTwo.GoToAttackPosition());
        StartCoroutine(cloneThree.GoToAttackPosition());
        StartCoroutine(cloneFour.GoToAttackPosition());
        yield return new WaitUntil(() => cloneOne.inSecondPosition);
        yield return new WaitUntil(() => cloneTwo.inSecondPosition);
        yield return new WaitUntil(() => cloneThree.inSecondPosition);
        yield return new WaitUntil(() => cloneFour.inSecondPosition);
        idleWhileMoving = false;
        yield return new WaitForSeconds(1f);

        // clones attack
        StartCoroutine(cloneOne.Shoot());
        StartCoroutine(cloneTwo.Shoot());
        StartCoroutine(CreateQuadOfBullets(degreeFacing, 5));
        StartCoroutine(cloneThree.Shoot());
        StartCoroutine(cloneFour.Shoot());
        yield return new WaitUntil(() => cloneOne.attack == false);
        yield return new WaitUntil(() => cloneTwo.attack == false);
        yield return new WaitUntil(() => cloneThree.attack == false);
        yield return new WaitUntil(() => cloneFour.attack == false);

        // destroy clones
        Destroy(cloneOneRef);
        Destroy(cloneTwoRef);
        Destroy(cloneThreeRef);
        Destroy(cloneFourRef);

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
            bulletRB.AddForce(BossBullet.transform.forward * 20, ForceMode.Impulse);
            bulletSource.Play();
        }
        //moved this to before the wait so boss has less downtime
        tempAttack = true;
        yield return new WaitForSeconds(2.0f);

        foreach (GameObject BossBullet in bossBullets)
        {
            Destroy(BossBullet);
        }

        //tempAttack = true;

        yield return null;
    }
}
