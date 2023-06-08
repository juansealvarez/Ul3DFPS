using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimShotgun : MonoBehaviour
{
    public float AimingSpeed = 2f;
    public Transform targetAim;
    public Transform targetNoAim;

    private void Update()
    {
        float aiming = AimingSpeed * Time.deltaTime;
        if (Input.GetKey("mouse 1"))
        {
           transform.position = Vector3.MoveTowards(transform.position, targetAim.position, aiming);
        }else
        {
           transform.position = Vector3.MoveTowards(transform.position, targetNoAim.position, aiming);
        }
    }
}
