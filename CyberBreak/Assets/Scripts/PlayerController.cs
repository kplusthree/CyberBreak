using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int lives;
    public GameObject[] hearts;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    public bool gameOver;

    public AudioClip damageClip;
    public AudioClip deathClip;
    public AudioSource damageSource;

    private TwinStickMovement twinStick;

    private CameraController cam;

    // Start is called before the first frame update
    void Start()
    {
        lives = hearts.Length;
        gameOver = false;
        damageSource.clip = damageClip;
        twinStick = GetComponent<TwinStickMovement>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (lives <= 0)
        {
            damageSource.clip = deathClip;
            damageSource.Play();
            gameOver = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "BossBullet")
        {
            if (twinStick.isDashing)
            {
                return;
            }

            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            invincibleTimer = timeInvincible;

            damageSource.Play();
            cam.shakeDuration = 1f;
            lives = lives - 1;
            Destroy(hearts[lives].gameObject);
            Destroy(collision.gameObject);
        }
    }
}
