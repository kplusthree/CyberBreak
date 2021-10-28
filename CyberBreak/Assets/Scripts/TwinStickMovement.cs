using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// [RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]

public class TwinStickMovement : MonoBehaviour
{
    public float playerSpeed = 5f;
    public float dashSpeed = 1000f;
    public float dashDelay = 2f;
    private float dashTimer;
    public float shootRate = 0.5f;
    public float controllerDeadzone = 0.1f;
    public float rotateSmoothing = 1000f;

    private bool isGamepad;
    bool canDash = false;
    private bool attack = false;
    public bool pause = false;

    // private CharacterController controller;
    private Rigidbody playerRB;
    public GameObject bulletPrefab;
    private static Rigidbody bulletRB;

    private Vector2 movement;
    private Vector2 aim;

    private Vector3 playerVelocity;

    private PlayerControls playerControls;
    private PlayerInput playerInput;

    void Awake()
    {
        // controller = GetComponent<CharacterController>();
        playerRB = GetComponent<Rigidbody>();
        playerControls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
        dashTimer = dashDelay;
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
        HandleDash();
        HandlePause();
    }

    void HandleInput()
    {
        movement = playerControls.Controls.Movement.ReadValue<Vector2>();
        aim = playerControls.Controls.Aim.ReadValue<Vector2>();
    }

    void HandleMovement()
    {
        Vector3 move = new Vector3(movement.x, 0, movement.y);
        // controller.Move(move * Time.deltaTime * playerSpeed);
        playerRB.MovePosition(transform.position + move * Time.deltaTime * playerSpeed);
    }

    void HandleDash()
    {
        dashTimer -= Time.deltaTime;

        Vector3 playerDirection = Vector3.right * aim.x + Vector3.forward * aim.y;
        if (playerDirection.sqrMagnitude > 0.0f)
        {
            playerControls.Controls.Dash.performed += ctx => Dash();
        }
    }

    void Dash()
    {
        Vector3 dashDirection = new Vector3(movement.x, 1, movement.y);
        // controller.Move(dashDirection * Time.deltaTime * playerSpeed);
        if (dashTimer <= 0)
        {
            playerRB.MovePosition(dashDirection * Time.deltaTime * dashSpeed);
            dashTimer = dashDelay;
            Debug.Log("Dashed");
        }
    }

    void HandleRotation()
    {
        if (isGamepad)
        {
            if(Mathf.Abs(aim.x) > controllerDeadzone || Mathf.Abs(aim.y) > controllerDeadzone)
            {
                Vector3 playerDirection = Vector3.right * aim.x + Vector3.forward * aim.y;
                if (playerDirection.sqrMagnitude > 0.0f)
                {
                    Quaternion newrotation = Quaternion.LookRotation(playerDirection, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, rotateSmoothing * Time.deltaTime);
                    // shoots when direction is turned
                    if (attack == false)
                    {
                        // launch the bullets from the player
                        StartCoroutine(Launch());
                    }
                }
            }
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(aim);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayDistance;

            if (groundPlane.Raycast(ray, out rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                LookAt(point);
            }
        }
    }

    // Fires Bullet
    IEnumerator Launch()
    {
        attack = true;
        GameObject BossBullet = Instantiate(bulletPrefab, transform.position + transform.forward * 2, transform.rotation);
        //bulletSource.clip = bulletClip;
        //bulletSource.Play();
        bulletRB = BossBullet.GetComponent<Projectile>().GetComponent<Rigidbody>();
        bulletRB.AddForce(transform.forward * 10, ForceMode.Impulse);
        yield return new WaitForSeconds(shootRate);
        attack = false;

        StartCoroutine(Destruction(BossBullet));
    }

    private void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }

    public void OnDeviceCahnge(PlayerInput playerInput)
    {
        isGamepad = playerInput.currentControlScheme.Equals("Gamepad") ? true : false;
    }

    IEnumerator Destruction(GameObject bullet)
    {
        yield return new WaitForSeconds(2);

        Destroy(bullet);
    }

    void HandlePause()
    {
        if (pause == false)
        {
            pause = true;
            playerControls.Controls.Pause.performed += ctx => SceneManager.LoadScene("PauseMenu");
        }
    }
}
