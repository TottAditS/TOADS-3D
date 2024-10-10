using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ITankInput))]
public class TankController : MonoBehaviour
{
    private Rigidbody TankRigid;
    private ITankInput TankInput;

    public float rotateSpeed = 90;
    public float speed = 5.0f;
    public float fireInterval = 0.5f;
    public float bulletSpeed = 20;
    public Transform spawnPoint;
    public GameObject bulletObject;
    float nextFire;
    public float nitro = 3f;

    public GameObject turret;
    public GameObject turret_barrel;

    public int ammotank;
    public GameObject canvas;

    public GameObject VCAM;
    public CinemachineVirtualCamera virtualCamera;
    private CinemachineComposer Composer;
    private CinemachineTransposer transposer;
    void Start()
    {
        ammotext.text = ammotank.ToString();
        ammotank = 30;

        nextFire = Time.time + fireInterval;
        TankRigid = GetComponent<Rigidbody>();
        TankInput = GetComponent<ITankInput>();
        virtualCamera = VCAM.GetComponent<CinemachineVirtualCamera>();
        Composer = virtualCamera.GetCinemachineComponent<CinemachineComposer>();
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        virtualCamera.m_Lens.FieldOfView = 60f;
        Composer.m_TrackedObjectOffset.y = 6.9f;
        tankbarrel = turret_barrel.GetComponent<Animator>();

        if (globalVolume.profile.TryGet(out Vignette tempVignette))
        {
            vignette = tempVignette;
        }

        virtualCamera.LookAt = normalTarget;
        virtualCamera.Follow = normalTarget;

        Invoke(nameof(enablecanvas), 15);
    }
    public void enablecanvas()
    {
        canvas.SetActive(true);
    }
    void Update()
    {

        var transAmount = speed * Time.deltaTime;
        var rotateAmount = rotateSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0, transAmount);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, 0, -transAmount);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -rotateAmount, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, rotateAmount, 0);
        }
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + fireInterval;
            fire();
            tankbarrel.Play("shootAnim");
        }
        
        if (isScoped)
        {
            //if (Input.GetKey(KeyCode.LeftArrow))
            //{
            //    aimTarget.transform.Rotate(0, -rotateAmount, 0);
            //}
            //if (Input.GetKey(KeyCode.RightArrow))
            //{
            //    aimTarget.transform.Rotate(0, rotateAmount, 0);
            //}
            //if (Input.GetKey(KeyCode.UpArrow))
            //{
            //    aimTarget.transform.Rotate(rotateAmount, 0, 0);
            //}
            //if (Input.GetKey(KeyCode.DownArrow))
            //{
            //    aimTarget.transform.Rotate(-rotateAmount, 0, 0);
            //}

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                turret.transform.Rotate(0, -rotateAmount/2, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                turret.transform.Rotate(0, rotateAmount/2, 0);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                turret_barrel.transform.Rotate(-rotateAmount/2, 0, 0);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                turret_barrel.transform.Rotate(rotateAmount/2, 0, 0);
            }
        }
        else if (!isScoped)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                turret.transform.Rotate(0, -rotateAmount, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                turret.transform.Rotate(0, rotateAmount, 0);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                turret_barrel.transform.Rotate(-rotateAmount, 0, 0);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                turret_barrel.transform.Rotate(rotateAmount, 0, 0);
            }
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.CapsLock) && Input.GetKey(KeyCode.LeftControl))
        {
            transform.Translate(0, 0, transAmount * nitro);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            aimTarget.gameObject.SetActive(!aimTarget.gameObject.activeSelf);

            isScoped = !isScoped;
            mode();
            // Ubah FOV kamera
            virtualCamera.m_Lens.FieldOfView = isScoped ? scopeFOV : normalFOV;

            //headTurret.localRotation = Quaternion.Euler(0, rotateAmount, 0);
        }
    }

    public Animator tankbarrel;
    public float recoil = 100f;

    public TextMeshProUGUI ammotext;
    void fire()
    {
        ammotank -= 1;
        ammotext.text = ammotank.ToString();
        TankRigid.AddForce(-headTurret.transform.forward * recoil, ForceMode.Impulse);
        var bullet = Instantiate(bulletObject, spawnPoint.position, spawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = spawnPoint.transform.forward * bulletSpeed;
        TankShootEffect.GetComponent<ParticleSystem>().Play();
        tankbarrel.StopPlayback();
    }

    public GameObject TankShootEffect;
    public Transform normalTarget;            // Target normal (misal, player)
    public GameObject TankCrosshair;

    void mode()
    {
        if (isScoped)
        {
            // Masuk ke Scope Mode: Kamera melihat dan mengikuti aimTarget
            virtualCamera.LookAt = aimTarget;
            if (vignette != null)
            {
                TankCrosshair.SetActive(true);
                Composer.m_TrackedObjectOffset.y = 0f;
                transposer.m_FollowOffset.y = 7f;
                transposer.m_FollowOffset.z = 0f;
                vignette.intensity.value = 0.5f;  // Sesuaikan sesuai kebutuhan (misal: lebih gelap)
            }
        }
        else
        {
            // Kembali ke Normal Mode: Kamera kembali melihat dan mengikuti normalTarget
            virtualCamera.LookAt = normalTarget;
            if (vignette != null)
            {
                TankCrosshair.SetActive(false);
                Composer.m_TrackedObjectOffset.y = 6.9f;
                transposer.m_FollowOffset.y = 10f;
                transposer.m_FollowOffset.z = -7.7f;
                vignette.intensity.value = 0.2f;  // Sesuaikan sesuai kebutuhan (misal: lebih gelap)
            }
        }
    }

    [Header("Smart Turret Control")]
    public Transform headTurret;      // Referensi ke objek head turret
    public Transform turretBarrel;    // Referensi ke objek turret barrel
    public Transform aimTarget;       // Target yang akan diikuti (misal aim sight)
    public float rotationSpeed = 5f;  // Kecepatan rotasi turret

    void turret_smart_aim()
    {
        if (isScoped)
        {
            // Menghitung arah ke target untuk rotasi horizontal (yaw) di head turret
            Vector3 directionToTarget = aimTarget.position - headTurret.position;
            Quaternion targetRotationY = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
            headTurret.rotation = Quaternion.Slerp(headTurret.rotation, targetRotationY, Time.deltaTime * rotationSpeed);

            // Menghitung arah ke target untuk rotasi vertikal (pitch) di turret barrel
            Vector3 localTargetPosition = headTurret.InverseTransformPoint(aimTarget.position);
            float anglePitch = Mathf.Atan2(localTargetPosition.y, localTargetPosition.z) * Mathf.Rad2Deg;
            turretBarrel.localRotation = Quaternion.Euler(anglePitch, 0, 0);
        }

        if (isScoped)
        {
            //// Menghitung arah ke target untuk rotasi horizontal (yaw) di head turret
            //Vector3 directionToTarget = aimTarget.position - headTurret.position;

            //// Mengunci rotasi Yaw pada sumbu horizontal untuk head turret (tanpa ikut sumbu X yang miring)
            //Vector3 flatDirection = new Vector3(directionToTarget.x, 0, directionToTarget.z);  // Buat arah baru yang rata pada sumbu XZ
            //Quaternion targetRotationY = Quaternion.LookRotation(flatDirection);  // Menghasilkan rotasi hanya di Y (tanpa kemiringan)

            //// Koreksi: Set Euler X ke 0 untuk mencegah miring
            //Quaternion correctedRotation = Quaternion.Euler(0, targetRotationY.eulerAngles.y, 0);

            //// Lerp ke rotasi yang sudah dikoreksi
            //headTurret.rotation = Quaternion.Slerp(headTurret.rotation, correctedRotation, Time.deltaTime * rotationSpeed);

            //// Menghitung arah ke target untuk rotasi vertikal (pitch) di turret barrel
            //Vector3 localTargetPosition = headTurret.InverseTransformPoint(aimTarget.position);
            //float anglePitch = Mathf.Atan2(localTargetPosition.y, localTargetPosition.z) * Mathf.Rad2Deg;

            //// Set rotasi barrel hanya pada sumbu X (pitch), tanpa mengubah Y/Z
            //turretBarrel.localRotation = Quaternion.Euler(-anglePitch, 0, 0);
        }
    }

    public Volume globalVolume;                      // Referensi ke Global Volume yang mengatur post-processing
    public float normalFOV = 60f;                    // FOV normal
    public float scopeFOV = 30f;                     // FOV saat scope mode (lebih kecil untuk zoom-in)
    private Vignette vignette;                       // Efek vignette
    private bool isScoped = false;                   // Apakah sedang dalam mode scope?
}
