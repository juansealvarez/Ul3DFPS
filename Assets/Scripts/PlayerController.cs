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
    private float shootDistance = 4f;
    [SerializeField]
    private ParticleSystem shootPS;

    private float speed;
    [SerializeField]
    public float turnSpeed;
    public float PlayerHealth = 20f;
    public float GunDamage = 1f;

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
    [SerializeField]
    private List<AudioClip> audioList;
    [System.NonSerialized]
    public bool IsDead = false;

    private Transform gun;

    public GameObject PlayerCapsulle;
    public GameObject shotgun;

    public GameObject DeadScreen;
    public GameObject UI;

    public List<AudioClip> mBackgroundAudio;
    private AudioSource BackgroundSource;
    public bool CopyrigthSong;
    private bool songPlayed = false;

    private void Start()
    {
        mRb = GetComponent<Rigidbody>();
        cameraMain = transform.Find("Main Camera");

        debugImpactSphere = Resources.Load<GameObject>("DebugImpactSphere");
        bloodObjectParticles = Resources.Load<GameObject>("BloodSplat_FX Variant");
        otherObjectParticles = Resources.Load<GameObject>("GunShot_Smoke_FX Variant");

        mAnimator = transform.Find("Main Camera")
            .Find("SM_Army_Shotgun")
            .GetComponent<Animator>();

        CameraAnimator = transform.Find("Main Camera").GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;

        mAudioSource = transform
            .Find("Main Camera")
            .Find("SM_Army_Shotgun").GetComponent<AudioSource>();
        BackgroundSource = transform
            .Find("Main Camera").GetComponent<AudioSource>();

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
            shotgun.SetActive(false);
            UI.gameObject.SetActive(false);
            DeadScreen.gameObject.SetActive(true);
            if(CopyrigthSong && !songPlayed)
            {
                BackgroundSource.PlayOneShot(mBackgroundAudio[0]);
                songPlayed = true;
            }else if (!CopyrigthSong && !songPlayed)
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
            if (value.isPressed)
            {
                mAudioSource.PlayOneShot(audioList[0]);
                mAnimator.SetTrigger("GunShooting");
                Shoot();
            } 
        }
        
    }

    private void Shoot()
    {
        shootPS.Play();

        RaycastHit hit;
        if (Physics.Raycast(
            cameraMain.position,
            cameraMain.forward,
            out hit,
            shootDistance
        ))
        {
            //var debugSphere = Instantiate(debugImpactSphere, hit.point, Quaternion.identity);
            //Destroy(debugSphere, 3f);
            if(hit.collider.CompareTag("Enemigos"))
            {
                var bloodPS = Instantiate(bloodObjectParticles, hit.point, Quaternion.identity);
                Destroy(bloodPS, 3f);
                var enemyController = hit.collider.GetComponent<EnemyController>();
                enemyController.TakeDamage(GunDamage);
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
            CameraAnimator.enabled = true;
            CameraAnimator.SetBool("IsDead", true);
            IsDead = true;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("EnemyAttack"))
        {
            TakeDamage(1f);
        }
    }
}