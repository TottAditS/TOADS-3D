using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    private void OnCollisionEnter(Collision objecthited)
    {
        //if (objecthited.gameObject.CompareTag("target"))
        //{
        //    createbulleteffect(objecthited);
        //    Destroy(gameObject);
        //}

        //if (objecthited.gameObject.CompareTag("wall"))
        //{
        //    createbulleteffect(objecthited);
        //    Destroy(gameObject);
        //}
    }

    void createbulleteffect(Collision objecthited)
    {
        ContactPoint contact = objecthited.contacts[0];

        GameObject hole = Instantiate(Globalreference.Instance.bulletimpacteffectprefab, contact.point, Quaternion.LookRotation(contact.normal));

        hole.transform.SetParent(objecthited.gameObject.transform);
    }
}
