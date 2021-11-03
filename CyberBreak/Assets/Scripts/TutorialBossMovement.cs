using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBossMovement : MonoBehaviour
{
    [HideInInspector]
    GameObject playerObj;
    public GameObject bulletPrefab;
    private static Rigidbody bulletRB;
    private Transform target;
    [HideInInspector]
    public gameController boss;

    [HideInInspector]
    public Quaternion rawDirection;
    [HideInInspector]
    public Vector3 direction;
    [HideInInspector]
    float degreeFacing;
    [HideInInspector]
    bool attack;

    public AudioClip bulletClip;
    public AudioClip deathClip;
    public AudioSource bulletSource;
    public AudioSource deathSource;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.Find("Player");
        attack = false;
        boss = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        bulletSource.clip = bulletClip;
        deathSource.clip = deathClip;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // player location
        target = playerObj.transform;

        if (boss.currentBossHealth <= 0)
        {
            deathSource.Play();
        }

        if (attack == true)
        {
            anim.SetInteger("State", 1);
        }
        else
        {
            anim.SetInteger("State", 0);
            // boss looks at player
            transform.LookAt(target);
            StartCoroutine(SmallAttack());
        }

        // let's the boss know what quadrant the player is in
        rawDirection = transform.rotation;
        direction = rawDirection.eulerAngles;
        degreeFacing = direction.y;
    }

    // Fires 3 shots directly at the player
    IEnumerator SmallAttack()
    {
        attack = true;
        int attackTimes = 1;

        // launch the bullets at the player
        for (int i = 0; i < attackTimes; i++)
        {
            Launch();
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(2.0f);

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
        bulletSource.Play();
    }
}
