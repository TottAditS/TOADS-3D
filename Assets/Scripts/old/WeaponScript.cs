using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public GameObject bulletprefab;
    public Transform bulletspawn;
    public float bulletvel = 40f;
    public float bullettime = 2f;

    public Camera playcam;
    public bool ishoot;
    public bool readyshoot;
    public bool allowreset = true;
    public float shootdelay = 2f;
    public float tempdelay = 2f;

    public int bulletperburst = 3;
    public int currburst;

    public float spreadintent;
    public enum shootmode
    {
        single,
        burst,
        auto
    }

    public shootmode currshootmode;
    void Start()
    {
        readyshoot = true;
        currburst = bulletperburst;
    }

    void Update()
    {
        if (currshootmode == shootmode.auto)
        {
            shootdelay = 0f;
            ishoot = Input.GetKey(KeyCode.Mouse0);
        }

        else if (currshootmode == shootmode.single || currshootmode == shootmode.burst)
        {
            shootdelay = tempdelay;
            ishoot = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (readyshoot && ishoot)
        {
            currburst = bulletperburst;
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            currshootmode = shootmode.burst;
        }
    }

    void Shoot()
    {
        readyshoot = false;

        Vector3 shootdirection = SmartAim().normalized;

        GameObject bullet = Instantiate(bulletprefab, bulletspawn.position, Quaternion.identity); ;

        bullet.transform.forward = shootdirection;

        bullet.GetComponent<Rigidbody>().AddForce(bulletspawn.forward.normalized * bulletvel, ForceMode.Impulse);

        StartCoroutine(DestroyBulletAfterTime(bullet, bullettime));

        if (allowreset == true)
        {
            Invoke("ResetShot", shootdelay);
            allowreset = false;
        }

        if (currshootmode == shootmode.burst && currburst > 1)
        {
            currburst--;
            Invoke("Shoot", 0.1f);
        }
    }

    private void ResetShot()
    {
        readyshoot = true;
        allowreset = true;
    }

    public Vector3 SmartAim() //Hitung Arah Peluru, Spread
    {
        Ray ray = playcam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        Vector3 targetpoint;

        if(Physics.Raycast(ray, out hit))
        {
            targetpoint = hit.point;
        }
        else
        {
            targetpoint = ray.GetPoint(100);
        }
        
        Vector3 direction = targetpoint - bulletspawn.position;

        float x = UnityEngine.Random.Range(-spreadintent, spreadintent);
        float y = UnityEngine.Random.Range(-spreadintent, spreadintent);

        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(bullet);
    }
}
