using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3.0f;

    Rigidbody rigidbody;
    private float horizontal;
    private float vertical;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontal, 0, vertical);

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void FixedUpdate()
    {
        Vector3 position = rigidbody.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.z = position.z + speed * vertical * Time.deltaTime;

        rigidbody.MovePosition(position);
    }
}
