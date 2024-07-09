using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mousemovenet : MonoBehaviour
{
    public float mousesensitif = 100f;
    float xrotate = 0f;
    float yrotate = 0f;
    public float topclamp = -90f;
    public float bottomclamp = 90f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mousx = Input.GetAxis("Mouse X") * mousesensitif *Time.deltaTime;
        float mousy = Input.GetAxis("Mouse Y") * mousesensitif * Time.deltaTime;

        xrotate -= mousy;
        xrotate = Mathf.Clamp(xrotate, topclamp, bottomclamp);
        yrotate += mousx;

        transform.localRotation = Quaternion.Euler(xrotate, yrotate, 0);
    }
}
