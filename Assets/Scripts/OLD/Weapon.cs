using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class Weapon : MonoBehaviour
{
    public GameObject bulletprefab;
    public Transform bulletspawn;
    public float bullvel = 30;
    public float bulletprefablife = 3f;

    public bool isshoot, readyshoot;
    bool allowreset = true;
    public float shootdelay = 2f;
    public int bulletperburst = 3;
    public int currentburst;

    public float spreadintensity;
    public GameObject muzzleeffect;

    public float reloadtime;
    public int magsize;
    public int bulletleft;
    public bool isrelod;
    public enum shootingmode
    {
        auto,
        single,
        burst
    }
    public enum weaponmodel
    {
        pistol,
        ARifle,
        Rifle
    }

    public shootingmode currentmode;
    public weaponmodel currentweapon;
    internal Animator animator;

    public Vector3 spawnpos;
    public Vector3 rotpos;
    private void Awake()
    {
        readyshoot = true;
        currentburst = bulletperburst;
        animator = GetComponent<Animator>();
        bulletleft = magsize;
    }

    public bool isactiveweapon;
    void Update()
    {
        if (isactiveweapon){

            GetComponent<Outline>().enabled = false;

            if (currentmode == shootingmode.auto)
            {
                isshoot = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentmode == shootingmode.single)
            {
                isshoot = Input.GetKeyDown(KeyCode.Mouse0);
            }
            else if (currentmode == shootingmode.burst)
            {
                isshoot = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (bulletleft == 0 && isshoot)
            {
                SoundManager.Instance.emptysound(currentweapon);
            }

            if (readyshoot && isshoot && bulletleft > 0)
            {
                currentburst = bulletperburst;
                fireweapon();
            }

            if (AmmoManager.Instance.ammodisplay != null)
            {
                AmmoManager.Instance.ammodisplay.text = $"{bulletleft}";
            }

            if (Input.GetKeyDown(KeyCode.R) && bulletleft < magsize && isrelod == false && WeaponManager.Instance.checkammoleft(currentweapon) > 0)
            {
                reload();
            }

            //if (readyshoot && isrelod == false && isrelod == false && bulletleft <= 0 && WeaponManager.Instance.checkammoleft(currentweapon) > 0)
            //{
            //    reload();
            //} 
        }
    }

    private void fireweapon()
    {

        muzzleeffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");
        SoundManager.Instance.shootsound(currentweapon);

        bulletleft--;
        readyshoot = false;

        Vector3 direction = calculationrecoil().normalized;

        GameObject bullet = Instantiate(bulletprefab, bulletspawn.position, Quaternion.identity);

        bullet.GetComponent<Rigidbody>().AddForce(direction * bullvel, ForceMode.Impulse);

        StartCoroutine(destroybulletaftertime(bullet, bulletprefablife));

        if (allowreset)
        {
            Invoke("resetshot", shootdelay);
            allowreset = false;
        }

        if (currentmode == shootingmode.burst && currentburst > 1)
        {
            currentburst -= 1;
            Invoke("fireweapon", shootdelay);
        }
    }

    private void reload()
    {
        SoundManager.Instance.reloadsound(currentweapon);
        animator.SetTrigger("RELOAD");
        isrelod = true;
        Invoke("reloadcomplete", reloadtime);
    }

    private void cekammolagi (int diweapon, int sisa)
    {
        //Debug.Log("diweapon" + diweapon);
        //Debug.Log("sisa" + sisa);

        //if (WeaponManager.Instance.checkammoleft(currentweapon) == 0)
        //{
        //    return;
        //}

        if (diweapon + sisa > magsize)
        {
            WeaponManager.Instance.decreasetotalammo(magsize-diweapon, currentweapon);
            bulletleft = magsize;
            
            //Debug.Log("diweapon + sisa > 30");
            //Debug.Log("diweapon" + bulletleft);
            //Debug.Log("sisa" + WeaponManager.Instance.checkammoleft(currentweapon));
        }
        else if (diweapon + sisa <= magsize)
        {
            bulletleft = diweapon + sisa;
            WeaponManager.Instance.decreasetotalammo(sisa, currentweapon);

            //Debug.Log("diweapon + sisa < 30");
            //Debug.Log("diweapon" + diweapon);
            //Debug.Log("sisa" + sisa);
        }
    }

    private void reloadcomplete()
    {

        if (WeaponManager.Instance.checkammoleft(currentweapon) > magsize)
        {
            WeaponManager.Instance.decreasetotalammo((magsize-bulletleft), currentweapon);
            bulletleft = magsize;
            
        }
        else
        {
            cekammolagi(bulletleft, WeaponManager.Instance.checkammoleft(currentweapon));
            //WeaponManager.Instance.decreasetotalammo(bulletleft, currentweapon);
            //bulletleft = WeaponManager.Instance.checkammoleft(currentweapon);
        }

        isrelod = false;
    }

    private void resetshot()
    {
        readyshoot = true;
        allowreset = true;
    }

    public Vector3 calculationrecoil()
    {
        //Ray ray = playercamera.ViewportPointToRay(new Vector3(0.3f, 0.3f, 0));

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        RaycastHit hit;
        Vector3 targetpoint;

        //Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

        if (Physics.Raycast(ray, out hit))
        {
            targetpoint = hit.point;
        }else
        {
            targetpoint = ray.GetPoint(100);
        }
        Vector3 direction = targetpoint - bulletspawn.position;

        float x = UnityEngine.Random.Range(-spreadintensity, spreadintensity);
        float y = UnityEngine.Random.Range(-spreadintensity, spreadintensity);

        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator destroybulletaftertime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
