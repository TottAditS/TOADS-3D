using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Tiger : MonoBehaviour
{
    public GameObject explosionn;
    public float duration = 0.8f;
    public Animator animator;
    public bool isalive;
    private void Start()
    {
        animator = GetComponent<Animator>();
        isalive = true;
    }
    private void OnCollisionEnter(Collision objectcollided)
    {
        if (objectcollided.gameObject.CompareTag("TankBullet") && isalive)
        {
            animator.SetBool("isalive", false);
            explosionn.GetComponent<ParticleSystem>().Play();
            Invoke("deactivebullet", duration);
            isalive = false;
        }
    }
    void deactivebullet()
    {
        explosionn.SetActive(false);
    }
}
