using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class buttonmanager : MonoBehaviour
{
    public Animator lanjutmenus;
    public Animator how2play;
    public Animator kamera;
    private bool onklik = false;
    private bool how2panelklik = false;
    public void lanjutmenu()
    {
        if (!onklik)
        {
            lanjutmenus.SetBool("bukalanjut", true);
            onklik = true;
        }

        else 
        {
            lanjutmenus.SetBool("bukalanjut", false);
            onklik = false;
        }
    }

    public void changescene(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

    public void Keluar()
    {
        Application.Quit();
    }

    public void howtoplay()
    {
        if (!how2panelklik) 
        {
            how2play.SetBool("onklik", true);
            kamera.SetBool("kehowtoplay", true);
            how2panelklik = true;
        }

        else 
        {
            how2play.SetBool("onklik", false);
            kamera.SetBool("kehowtoplay", false);
            how2panelklik = false;
        }
    }
}
