using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineUi;
    public TextMeshProUGUI totalammo;
    public Image AmmoUi;

    [Header("Weapon")]
    public Image Activeweapon;
    public Image Pasiveweapon;

    [Header("Status")]
    public Slider hpbar;
    public Slider ammobar;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI killcount;

    public Sprite emty;

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
        Weapon activeweapon = WeaponManager.Instance.activeslot.GetComponentInChildren<Weapon>();
        Weapon unactive = getunactive().GetComponentInChildren<Weapon>();

        if (activeweapon)
        {
            magazineUi.text = $"{activeweapon.bulletleft}";
            totalammo.text = $"{WeaponManager.Instance.checkammoleft(activeweapon.currentweapon)}";

            Weapon.weaponmodel model = activeweapon.currentweapon;
            AmmoUi.sprite = getammosprite(model);
            Activeweapon.sprite = getweaponsprite(model);

            if (unactive)
            {
                Pasiveweapon.sprite = getweaponsprite(unactive.currentweapon);
            }
        }
        else
        {
            magazineUi.text = "";
            totalammo.text = "";
            AmmoUi.sprite = emty;
            Activeweapon.sprite = emty;
            Pasiveweapon.sprite = emty;

        }
    }

    private GameObject getunactive()
    {
        foreach (GameObject weaponslot in WeaponManager.Instance.weaponslots)
        {
            if (weaponslot != WeaponManager.Instance.activeslot)
            {
                return weaponslot;
            }
        }

        return null;
    }

    private Sprite getammosprite(Weapon.weaponmodel model)
    {
        switch (model)
        {
            case Weapon.weaponmodel.pistol:
                return Instantiate(Resources.Load<GameObject>("PistolBullet").GetComponent<SpriteRenderer>().sprite);
            case Weapon.weaponmodel.ARifle:
                return Instantiate(Resources.Load<GameObject>("ARBullet").GetComponent<SpriteRenderer>().sprite);
            default:
                return null;
        }
    }

    private Sprite getweaponsprite(Weapon.weaponmodel model)
    {
        switch (model)
        {
            case Weapon.weaponmodel.pistol:
                return Instantiate(Resources.Load<GameObject>("PistolLogo").GetComponent<SpriteRenderer>().sprite);
            case Weapon.weaponmodel.ARifle:
                return Instantiate(Resources.Load<GameObject>("ARLogo").GetComponent<SpriteRenderer>().sprite);
            default:
                return null;
        }
    }
}
