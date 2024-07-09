using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weaponslots;
    public GameObject activeslot;

    [Header("Ammo")]
    public int totalARammo = 0;
    public int totalpistolammo = 0;


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
    private void Start()
    {
        activeslot = weaponslots[0];
    }

    private void Update()
    {
        foreach (GameObject weaponslot in weaponslots)
        {
            if (weaponslot == activeslot)
            {
                weaponslot.SetActive(true);
            }
            else
            {
                weaponslot.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            changeweapontoactive(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            changeweapontoactive(1);
        }
    }

    public void pickupweapon(GameObject pickedweapon)
    {
        addweaponintoactive(pickedweapon);
    }

    internal void pickupammo(Ammo ammo)
    {
        switch (ammo.ammoType)
        {
            case Ammo.AmmoType.PistolAmmo:
                totalpistolammo = totalpistolammo + ammo.amount; break;
            case Ammo.AmmoType.ARammo:
                totalARammo = totalARammo + ammo.amount; break;
        }
    }
    internal void decreasetotalammo(int bulletdecrease, Weapon.weaponmodel currentweapon)
    {
        switch (currentweapon)
        {
            case Weapon.weaponmodel.ARifle:
                totalARammo -= bulletdecrease; break;
            case Weapon.weaponmodel.pistol:
                totalpistolammo -= bulletdecrease; break;
        }
    }

    public int checkammoleft(Weapon.weaponmodel currentmodel)
    {
        switch (currentmodel)
        {
            case Weapon.weaponmodel.ARifle:
                return totalARammo;
            case Weapon.weaponmodel.pistol:
                return totalpistolammo;
            default:
                return 0;
        }
    }

    private void addweaponintoactive(GameObject pickedweapon)
    {
        dropcurrentweapon(pickedweapon);

        pickedweapon.transform.SetParent(activeslot.transform, false);
        Weapon senjata = pickedweapon.GetComponent<Weapon>();

        pickedweapon.transform.localPosition = new Vector3(senjata.spawnpos.x, senjata.spawnpos.y, senjata.spawnpos.z);
        pickedweapon.transform.localRotation = Quaternion.Euler(senjata.rotpos.x, senjata.rotpos.y, senjata.rotpos.z);

        senjata.isactiveweapon = true;
        senjata.GetComponent<Animator>().enabled = true;
    }

    private void dropcurrentweapon(GameObject pickedweapon)
    {
        if (activeslot.transform.childCount > 0)
        {
            var weapondrop = activeslot.transform.GetChild(0).gameObject;

            weapondrop.GetComponent<Weapon>().isactiveweapon = false;
            weapondrop.GetComponent<Weapon>().animator.enabled = false;

            weapondrop.transform.SetParent(pickedweapon.transform.parent);
            weapondrop.transform.localPosition = pickedweapon.transform.localPosition;
            weapondrop.transform.localRotation = pickedweapon.transform.localRotation;
        }
    }

    private void changeweapontoactive(int slotnumber)
    {
        if (activeslot.transform.childCount > 0)
        {
            Weapon currweapon = activeslot.transform.GetChild(0).GetComponent<Weapon>();
            currweapon.isactiveweapon = false;
        }

        activeslot = weaponslots[slotnumber];

        if (activeslot.transform.childCount > 0)
        {
            Weapon newweapon = activeslot.transform.GetChild(0).GetComponent<Weapon>();
            newweapon.isactiveweapon = true;
        }
    }
}
