using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timertext;
    [SerializeField] float elapstime;
    public float startime = 300f;
    int waktu;

    //public Slider waterslide;
    public float mengalir = 0f;
    //public Savemenege saveme;

    private void Start()
    {
        elapstime = startime;
        waktu = (int) elapstime;
        //waterslide.maxValue = startime;
    }
    void Update()
    {
        elapstime -= Time.deltaTime;
        mengalir += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapstime / 60);
        int seconds = Mathf.FloorToInt(elapstime % 60);
        float mili = (elapstime % 1) * 100;
        timertext.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, mili);

        //waterslide.value = mengalir;
    }
}
