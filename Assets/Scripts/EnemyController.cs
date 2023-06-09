using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform Player;

    public float Speed = 2f;
    public float AwakeRadio = 2f;

    public float AttackRadio = 0.5f;
    public float Health = 5f;

    private Animator mAnimator;
    private Rigidbody mRb;

    private Vector2 mDirection;  // XZ

    private bool mIsAttacking = false;
    private bool dead = false;

    private AudioSource mAudioSource;
    [SerializeField]
    private List<AudioClip> audioList;
    public GameObject HitboxLeft;
    private CapsuleCollider mCollider;
    private NavMeshAgent navMeshAgent;

    private void Start()
    {
        mRb = GetComponent<Rigidbody>();
        mAnimator = transform
            .GetComponentInChildren<Animator>(false);
        mAudioSource = GetComponent<AudioSource>();
        mCollider = GetComponent<CapsuleCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //mAudioSource.PlayOneShot(audioList[Random.Range(0,2)]);

        var collider1 = IsPlayerInAttackArea();
        if (collider1 != null && !mIsAttacking && !dead)
        {
            mRb.velocity = new Vector3(
                0f,
                0f,
                0f
            );
            //mAudioSource.PlayOneShot(audioList[Random.Range(2,4)]);
            navMeshAgent.isStopped = true;
            mAnimator.SetBool("IsWalking", false);
            mAnimator.SetTrigger("Attacking");
            mAnimator.SetInteger("RandomAttack", Random.Range(0,3));
            return;
        }
        
        
        var collider2 = IsPlayerNearby();

        if (collider2 != null && !mIsAttacking && !dead)
        {
            mAnimator.SetBool("IsWalking", true);
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(collider2.transform.position);
            //Walk(collider2);
            mAnimator.SetFloat("Horizontal", mDirection.x);
            mAnimator.SetFloat("Vertical", mDirection.y);
        }
        else
        {
            // parar
            mRb.velocity = Vector3.zero;
            mAnimator.SetBool("IsWalking", false);
            navMeshAgent.isStopped = true;
        }
    }

    private void Walk(Collider collider2)
    {
        // caminar
        var playerPosition = collider2.transform.position;
        var direction = playerPosition - transform.position;
        mDirection = new Vector2(direction.x, direction.z);

        //transform.LookAt(playerPosition, Vector3.up);
        direction.y = 0f;
        transform.rotation = Quaternion.Lerp(
            transform.rotation, Quaternion.LookRotation(direction, Vector3.up), 0.1f
        );

        mRb.velocity = new Vector3(
            mDirection.x * Speed,
            0f,
            mDirection.y * Speed
        );

        mAnimator.SetBool("IsWalking", true);
        mAnimator.SetFloat("Horizontal", mDirection.x);
        mAnimator.SetFloat("Vertical", mDirection.y);
    }

    private Collider IsPlayerNearby()
    {
        var colliders = Physics.OverlapSphere(
            transform.position,
            AwakeRadio,
            LayerMask.GetMask("Player")
        );
        if (colliders.Length == 1) return colliders[0];
        else return null;
    }

    private Collider IsPlayerInAttackArea()
    {
        var colliders = Physics.OverlapSphere(
            transform.position,
            AttackRadio,
            LayerMask.GetMask("Player")
        );
        if (colliders.Length == 1) return colliders[0];
        else return null;
    }

    public void StartAttack()
    {
        mIsAttacking = true;
    }

    public void EnableHitbox()
    {
        HitboxLeft.SetActive(true);
    }

    public void StopAttack()
    {
        mIsAttacking = false;
        HitboxLeft.SetActive(false);
    }

    public void TakeDamage(float Damage)
    {
        Health -= Damage;
        if (Health == 0f)
        {
            mAnimator.SetTrigger("Die");
            mCollider.enabled = false;
            dead = true;
            Destroy(gameObject, 20f);
        }
    }
}
