using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Globalreference : MonoBehaviour
{
    public static Globalreference Instance { get; set; }

    public GameObject bulletimpacteffectprefab;
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
}
