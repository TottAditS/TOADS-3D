using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public Transform groundcheck;
    public LayerMask gmask;
    Vector3 velocity;

    public float speed = 8f;
    public float jumpheight = 3f;
    public float gravity = -9.8f * 2f;
    public float groundist = 0.4f;

    bool isground;
    //bool ismove;

    Vector3 laspost = new Vector3(0f,0f,0f);

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isground = Physics.CheckSphere(groundcheck.position, groundist, gmask);

        if (isground && velocity.y < 0) {

            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isground)
        {
            velocity.y = Mathf.Sqrt(jumpheight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (laspost != gameObject.transform.position && isground == true)
        {
            //ismove = true;
        }

        else
        {
            //ismove = false;
        }
    }
}
