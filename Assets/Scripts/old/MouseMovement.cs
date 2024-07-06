using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float msensitivity = 400f;
    public Camera kamera;

    public float topclamp = -90f;
    public float bottomclamp = 90f;
    public float rotx = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        rotx += -Input.GetAxis("Mouse Y") * msensitivity * Time.deltaTime;
        rotx = Mathf.Clamp(rotx, topclamp, bottomclamp);
        kamera.transform.localRotation = Quaternion.Euler(rotx, 0f, 0f);
        transform.rotation *= Quaternion.Euler(0f, Input.GetAxis("Mouse X") * msensitivity * Time.deltaTime, 0f);

    }
}
