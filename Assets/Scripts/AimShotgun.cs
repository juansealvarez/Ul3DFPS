using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimShotgun : MonoBehaviour
{
    public float AimingSpeed = 2f;
    public Transform targetAim;
    public Transform targetNoAim;

    public Transform LookCamera;

    private Animator mAnimator;

    private void Start()
    {
        mAnimator = LookCamera.GetComponent<Animator>();
    }

    private void Update()
    {
        float aiming = AimingSpeed * Time.deltaTime;
        if (Input.GetKey("mouse 1"))
        {
            mAnimator.SetBool("IsAiming", true);
           transform.position = Vector3.MoveTowards(transform.position, targetAim.position, aiming);
        }else
        {
            mAnimator.SetBool("IsAiming", false);
           transform.position = Vector3.MoveTowards(transform.position, targetNoAim.position, aiming);
        }
    }
}
