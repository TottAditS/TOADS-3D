using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource shootingchanel;
    public AudioClip pistolshot;
    public AudioClip m4carbineshot;
    public AudioClip reloadpistol9mm;
    public AudioClip reloadm4carbine;
    public AudioClip emptymag;
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

    public void shootsound(weaponmodel weapon)
    {
        switch(weapon)
        {
            case weaponmodel.pistol:
                //pistol9mm.Play();
                shootingchanel.PlayOneShot(pistolshot);
                break;
            case weaponmodel.ARifle:
                //m4carbine.Play();
                shootingchanel.PlayOneShot(m4carbineshot);
                break;
        }
    }

    public void reloadsound(weaponmodel weapon)
    {
        switch (weapon)
        {
            case weaponmodel.pistol:
                shootingchanel.PlayOneShot(reloadpistol9mm);
                break;
            case weaponmodel.ARifle:
                shootingchanel.PlayOneShot(reloadm4carbine);
                break;
        }
    }

    public void emptysound(weaponmodel weapon)
    {
        switch (weapon)
        {
            case weaponmodel.pistol:
                shootingchanel.PlayOneShot(emptymag);
                break;
            case weaponmodel.ARifle:
                shootingchanel.PlayOneShot(emptymag);
                break;
        }
    }
}
