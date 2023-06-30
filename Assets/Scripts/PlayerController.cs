using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float WalkingSpeed;

    private float RunnigMultiplier;
    [SerializeField]

    private float speed;
    [SerializeField]
    public float turnSpeed;
    public static float PlayerHealth = 20f;

    private Rigidbody mRb;
    private Vector2 mDirection;
    private Vector2 mDeltaLook;
    private Transform cameraMain;
    private GameObject debugImpactSphere;
    private GameObject bloodObjectParticles;
    private GameObject otherObjectParticles;

    private Animator mAnimator;

    private Animator CameraAnimator;

    private AudioSource mAudioSource;
    //[SerializeField]
    //private List<AudioClip> audioList;
    [System.NonSerialized]
    public bool IsDead = false;

    public GameObject PlayerCapsulle;
    public GameObject primary;
    public GameObject secondary;

    public GameObject DeadScreen;
    public GameObject UI;

    public List<AudioClip> mBackgroundAudio;
    private AudioSource BackgroundSource;
    private bool songPlayed = false;
    private AimShotgun aimShotgun;
    private AimShotgun aimPistol;
    private AudioSource pAudioSource;
    private Animator pAnimator;
    [SerializeField]
    private GameManager gameManager;
    private PlayerInput mPlayerInput;

    private void Start()
    {
        mRb = GetComponent<Rigidbody>();
        cameraMain = transform.Find("Main Camera");

        debugImpactSphere = Resources.Load<GameObject>("DebugImpactSphere");
        bloodObjectParticles = Resources.Load<GameObject>("BloodSplat_FX Variant");
        otherObjectParticles = Resources.Load<GameObject>("GunShot_Smoke_FX Variant");
        aimShotgun = transform.Find("Main Camera").Find("SM_Army_Shotgun").GetComponent<AimShotgun>();
        aimPistol = transform.Find("Main Camera").Find("SM_Army_Pistol").GetComponent<AimShotgun>();

        mAnimator = transform.Find("Main Camera")
            .Find("SM_Army_Shotgun")
            .GetComponent<Animator>();

        pAnimator = transform.Find("Main Camera")
            .Find("SM_Army_Pistol")
            .GetComponent<Animator>();

        CameraAnimator = transform.Find("Main Camera").GetComponent<Animator>();
        mPlayerInput = GetComponent<PlayerInput>();

        Cursor.lockState = CursorLockMode.Locked;

        mAudioSource = transform
            .Find("Main Camera")
            .Find("SM_Army_Shotgun").GetComponent<AudioSource>();
        BackgroundSource = transform
            .Find("Main Camera").GetComponent<AudioSource>();
        pAudioSource = transform
            .Find("Main Camera")
            .Find("SM_Army_Pistol").GetComponent<AudioSource>();

        RunnigMultiplier = 1.5f;
        CameraAnimator.enabled = false;
    }

    private void Update()
    {
        if (!IsDead)
        {
            if (Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftControl))
            {
                speed = WalkingSpeed * RunnigMultiplier;
            }else
            {
                speed = WalkingSpeed;
            }

            mRb.velocity = mDirection.y * speed * transform.forward
                + mDirection.x * speed * transform.right;

            transform.Rotate(
                Vector3.up,
                turnSpeed * Time.deltaTime * mDeltaLook.x
            );
            cameraMain.GetComponent<CameraMovement>().RotateUpDown(
                -turnSpeed * Time.deltaTime * mDeltaLook.y
            );
        }else
        {
            PlayerCapsulle.SetActive(false);
            primary.SetActive(false);
            secondary.SetActive(false);
            UI.gameObject.SetActive(false);
            DeadScreen.gameObject.SetActive(true);
            gameManager.enabled = false;
            if(gameManager.CopyrigthSong && !songPlayed)
            {
                BackgroundSource.PlayOneShot(mBackgroundAudio[0]);
                songPlayed = true;
            }else if (!gameManager.CopyrigthSong && !songPlayed)
            {
                BackgroundSource.PlayOneShot(mBackgroundAudio[1]);
                songPlayed = true;
            }
        }
    }
    private void OnMove(InputValue value)
    {
        mDirection = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        mDeltaLook = value.Get<Vector2>();
    }

    private void OnFire(InputValue value)
    {
        if(!IsDead)
        {
            if (value.isPressed && !MenuPausa.isPaused)
            {
                if(aimShotgun.WeaponActive)
                {
                    mAudioSource.PlayOneShot(aimShotgun.Weapon.audioList[0]);
                    mAnimator.SetTrigger("GunShooting");
                    Shoot();
                }else
                {
                    pAudioSource.PlayOneShot(aimPistol.Weapon.audioList[0]);
                    pAnimator.SetTrigger("GunShooting");
                    Shoot();
                }
            }
        }
        
    }

    private void Shoot()
    {
        var scriptGun = aimShotgun;
        if (aimShotgun.WeaponActive)
        {
            scriptGun = aimShotgun;
        }else
        {
            scriptGun = aimPistol;
        }
        scriptGun.shootPS.Play();

        RaycastHit hit;
        if (Physics.Raycast(
            cameraMain.position,
            cameraMain.forward,
            out hit,
            scriptGun.Weapon.shootDistance
        ))
        {
            //var debugSphere = Instantiate(debugImpactSphere, hit.point, Quaternion.identity);
            //Destroy(debugSphere, 3f);
            if(hit.collider.CompareTag("Enemigos"))
            {
                var bloodPS = Instantiate(bloodObjectParticles, hit.point, Quaternion.identity);
                Destroy(bloodPS, 3f);
                var enemyController = hit.collider.GetComponent<EnemyController>();
                enemyController.TakeDamage(scriptGun.Weapon.GunDamage);
            }else
            {
                var otherPS = Instantiate(otherObjectParticles, hit.point, Quaternion.identity);
                otherPS.GetComponent<ParticleSystem>().Play();
                Destroy(otherPS, 3f);
            }
        }
    }

    public void TakeDamage(float Damage)
    {
        PlayerHealth -= Damage;
        if (PlayerHealth <= 0)
        {
            mPlayerInput.SwitchCurrentActionMap("PauseMenu");
            CameraAnimator.enabled = true;
            CameraAnimator.SetBool("IsDead", true);
            IsDead = true;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("EnemyAttack"))
        {
            TakeDamage(EnemyController.damage);
        }
    }
}