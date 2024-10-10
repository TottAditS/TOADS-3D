using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankBullet : MonoBehaviour
{
    public GameObject explosionn;
    public float duration = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        explosionn.SetActive(true);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        explosionn.GetComponent<ParticleSystem>().Play();
        Invoke("deletebullet", duration);
    }

    void deletebullet()
    {
        Destroy(gameObject);
    }
}
