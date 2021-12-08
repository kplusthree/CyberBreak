using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]

public class TwinStickMovement : MonoBehaviour
{
    Scene scene;

    public float playerSpeed = 5f;
    public float dashSpeed = 30f;
    public float dashLength = 0.2f;
    private float dashLengthTimer;
    public float dashLimit = 3f;
    private float dashLimitTimer = 0f;
    public float shootRate = 0.5f;
    public float controllerDeadzone = 0.1f;
    public float rotateSmoothing = 1000f;
    int randNum;

    private bool attack = false;
    public bool pause = false;
    public bool paused = false;
    private bool facingRight;
    [HideInInspector]
    public bool isDashing = false;

    private CharacterController controller;
    public GameObject bulletPrefab;
    private static Rigidbody bulletRB;
    Animator anim;

    private Vector2 movement;
    private Vector2 aim;

    private Vector3 playerVelocity;

    private PlayerControls playerControls;
    private PlayerInput playerInput;

    public AudioClip walkClip;
    public AudioClip bulletClip1;
    public AudioClip bulletClip2;
    public AudioClip bulletClip3;
    public AudioSource walkSource;
    public AudioSource bulletSource1;
    public AudioSource bulletSource2;
    public AudioSource bulletSource3;

    void Awake()
    {
        scene = SceneManager.GetActiveScene();
        controller = GetComponent<CharacterController>();
        playerControls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();

        walkSource.clip = walkClip;
        bulletSource1.clip = bulletClip1;
        bulletSource2.clip = bulletClip2;
        bulletSource3.clip = bulletClip3;

        dashLengthTimer = dashLength;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        HandleInput();
        HandleMovement();
        HandleRotation();
        HandleAnimatorState();
    }

    void HandleInput()
    {
        movement = playerControls.Controls.Movement.ReadValue<Vector2>();
        aim = playerControls.Controls.Aim.ReadValue<Vector2>();

        playerControls.Controls.Pause.performed += ctx => HandlePause();

        if (pause == true)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    void HandlePause()
    {
        if (pause == false && paused == false)
        {
            paused = true;
            SceneManager.LoadScene("PauseMenu", LoadSceneMode.Additive);
        }
    }

    void HandleMovement()
    {
        Vector3 move = new Vector3(movement.x, 0, movement.y);

        if (dashLimitTimer <= 0)
        {
            playerControls.Controls.Dash.performed += ctx => isDashing = true;
        }
        else
        {
            isDashing = false;
        }
        
        if (dashLengthTimer <= 0)
        {
            isDashing = false;
            dashLimitTimer = dashLimit;
        }

        if(isDashing == false)
        {
            controller.Move(move * Time.deltaTime * playerSpeed);
            dashLengthTimer = dashLength;
        }
        else
        {
            controller.Move(move * Time.deltaTime * dashSpeed);
            dashLengthTimer -= Time.deltaTime;
        }

        dashLimitTimer -= Time.deltaTime;
    }

    void HandleRotation()
    {
        if(Mathf.Abs(aim.x) > controllerDeadzone || Mathf.Abs(aim.y) > controllerDeadzone)
        {
            Vector3 playerDirection = Vector3.right * aim.x + Vector3.forward * aim.y;
            if (playerDirection.sqrMagnitude > 0.0f)
            {
                Quaternion newrotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, rotateSmoothing * Time.deltaTime);
                // shoots when direction is turned
                if (attack == false && scene.name != "JailCell")
                {
                    // launch the bullets from the player
                    StartCoroutine(Launch());
                }
            }
        }
    }

    void HandleAnimatorState()
    {
        Vector3 move = new Vector3(movement.x, 0, movement.y);

        if (isDashing == true)
        {
            anim.SetInteger("State", 4);
        }
        else if (move.x < 0)
        {
            facingRight = false;
            anim.SetInteger("State", 2);
            walkSource.Play();
        }
        else if (move.x > 0)
        {
            facingRight = true;
            anim.SetInteger("State", 3);
            walkSource.Play();
        }
        else if (move.x == 0 && move.y == 0 && !facingRight)
        {
            anim.SetInteger("State", 0);
            walkSource.Stop();
        }
        else if (move.x == 0 && move.y == 0 && facingRight)
        {
            anim.SetInteger("State", 1);
            walkSource.Stop();
        }
    }

    // Fires Bullet
    IEnumerator Launch()
    {
        attack = true;
        GameObject BossBullet = Instantiate(bulletPrefab, transform.position + transform.forward * 2, transform.rotation);
        bulletRB = BossBullet.GetComponent<Projectile>().GetComponent<Rigidbody>();
        bulletRB.AddForce(transform.forward * 10, ForceMode.Impulse);

        // create random number between 1 and 3
        randNum = Random.Range(1, 4);
        // swap between different bullet sounds
        if (randNum == 1)
        {
            bulletSource1.Play();
        }
        else if (randNum == 2)
        {
            bulletSource2.Play();
        }
        else
        {
            bulletSource3.Play();
        }

        yield return new WaitForSeconds(shootRate);
        attack = false;

        StartCoroutine(Destruction(BossBullet));
    }

    private void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }

    IEnumerator Destruction(GameObject bullet)
    {
        yield return new WaitForSeconds(2);

        Destroy(bullet);
    }
}
