using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; set; }

    public Weapon hoveredweapon = null;
    public Ammo hoveredammo = null;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject ObjectHitByRaycast = hit.transform.gameObject;

            if (ObjectHitByRaycast.GetComponent<Weapon>() && ObjectHitByRaycast.GetComponent<Weapon>().isactiveweapon == false )
            {
                hoveredweapon = ObjectHitByRaycast.gameObject.GetComponent<Weapon>();
                hoveredweapon.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.pickupweapon(ObjectHitByRaycast.gameObject);
                }
            }

            else
            {
                if (hoveredweapon) hoveredweapon.GetComponent<Outline>().enabled = false;
            }


            //ammo
            if (ObjectHitByRaycast.GetComponent<Ammo>())
            {
                hoveredammo = ObjectHitByRaycast.gameObject.GetComponent<Ammo>();
                hoveredammo.GetComponent<Outline>().enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    WeaponManager.Instance.pickupammo(hoveredammo);
                    Destroy(ObjectHitByRaycast.gameObject);
                }
            }

            else
            {
                if (hoveredammo) hoveredammo.GetComponent<Outline>().enabled = false;
            }
        }
    }
}
