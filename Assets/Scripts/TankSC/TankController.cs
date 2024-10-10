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

    protected float rotateSpeed = 90;
    protected float speed = 5.0f;
    protected float fireInterval = 0.5f;
    protected float bulletSpeed = 20;
    protected Transform spawnPoint;
    protected GameObject bulletObject;
    protected float nextFire;
    protected float nitro = 3f;

    protected GameObject turret;
    protected GameObject turret_barrel;

    protected int ammotank;
    protected GameObject canvas;

    protected GameObject VCAM;
    protected CinemachineVirtualCamera virtualCamera;
    private CinemachineComposer Composer;
    private CinemachineTransposer transposer;

    public TextMeshProUGUI ammotext;
    public GameObject TankShootEffect;
    public Transform normalTarget; // Target normal (misal, player)
    public GameObject TankCrosshair;

    public Volume globalVolume;                      // Referensi ke Global Volume yang mengatur post-processing
    public float normalFOV = 60f;                    // FOV normal
    public float scopeFOV = 30f;                     // FOV saat scope mode (lebih kecil untuk zoom-in)
    private Vignette vignette;                       // Efek vignette
    private bool isScoped = false;                   // Apakah sedang dalam mode scope?

    public Animator tankbarrel;
    public float recoil = 100f;

    void Start()
    {
        ammotank = 30;
        ammotext.text = ammotank.ToString();

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

        HandleTurretControl(rotateAmount);

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.CapsLock) && Input.GetKey(KeyCode.LeftControl))
        {
            transform.Translate(0, 0, transAmount * nitro);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            aimTarget.gameObject.SetActive(!aimTarget.gameObject.activeSelf);
            isScoped = !isScoped;
            mode();
            virtualCamera.m_Lens.FieldOfView = isScoped ? scopeFOV : normalFOV;
        }
    }

    void HandleTurretControl(float rotateAmount)
    {
        if (isScoped)
        {
            HandleScopedTurretControl(rotateAmount);
        }
        else
        {
            HandleNormalTurretControl(rotateAmount);
        }
    }

    void HandleScopedTurretControl(float rotateAmount)
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            turret.transform.Rotate(0, -rotateAmount / 2, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            turret.transform.Rotate(0, rotateAmount / 2, 0);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            turret_barrel.transform.Rotate(-rotateAmount / 2, 0, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            turret_barrel.transform.Rotate(rotateAmount / 2, 0, 0);
        }
    }

    void HandleNormalTurretControl(float rotateAmount)
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

    void mode()
    {
        if (isScoped)
        {
            EnterScopedMode();
        }
        else
        {
            ExitScopedMode();
        }
    }

    void EnterScopedMode()
    {
        virtualCamera.LookAt = aimTarget;
        if (vignette != null)
        {
            TankCrosshair.SetActive(true);
            Composer.m_TrackedObjectOffset.y = 0f;
            transposer.m_FollowOffset.y = 7f;
            transposer.m_FollowOffset.z = 0f;
            vignette.intensity.value = 0.5f; // Sesuaikan sesuai kebutuhan
        }
    }

    void ExitScopedMode()
    {
        virtualCamera.LookAt = normalTarget;
        if (vignette != null)
        {
            TankCrosshair.SetActive(false);
            Composer.m_TrackedObjectOffset.y = 6.9f;
            transposer.m_FollowOffset.y = 10f;
            transposer.m_FollowOffset.z = -7.7f;
            vignette.intensity.value = 0.2f; // Sesuaikan sesuai kebutuhan
        }
    }

    void turret_smart_aim()
    {
        if (isScoped)
        {
            Vector3 directionToTarget = aimTarget.position - headTurret.position;
            Quaternion targetRotationY = Quaternion.LookRotation(new Vector3(directionToTarget.x, 0, directionToTarget.z));
            headTurret.rotation = Quaternion.Slerp(headTurret.rotation, targetRotationY, Time.deltaTime * rotationSpeed);

            Vector3 localTargetPosition = headTurret.InverseTransformPoint(aimTarget.position);
            float anglePitch = Mathf.Atan2(localTargetPosition.y, localTargetPosition.z) * Mathf.Rad2Deg;
            turretBarrel.localRotation = Quaternion.Euler(anglePitch, 0, 0);
        }
    }
}
