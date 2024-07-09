using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public int amount;
    public AmmoType ammoType;

    public enum AmmoType
    {
        ARammo,
        PistolAmmo
    }
}
