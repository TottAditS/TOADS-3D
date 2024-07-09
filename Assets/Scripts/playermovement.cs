using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f * 2;
    public float jumpheight = 3f;

    public Transform groundcek;
    public float groundist = 0.4f;
    public LayerMask groundmask;

    Vector3 velocity;

    bool isground;
    //bool ismove;

    private Vector3 laspost = new Vector3(0,0,0);
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        isground = Physics.CheckSphere(groundcek.position, groundist, groundmask);
        if (isground&& velocity.y < 0 )
        {
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

        if (laspost != gameObject.transform.position && isground)
        {
            //ismove = true;

        }

        else
        {
            //ismove = false;
        }

        laspost = Vector3.zero;
    }
}
