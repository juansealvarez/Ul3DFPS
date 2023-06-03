using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float WalkingSpeed;

    private float RunnigMultiplier;

    private float speed;
    [SerializeField]
    private float turnSpeed;

    private Rigidbody mRb;
    private Vector2 mDirection;
    private Vector2 mDeltaLook;
    private Transform cameraMain;

    private void Start()
    {
        mRb = GetComponent<Rigidbody>();
        cameraMain = transform.Find("Main Camera");

        Cursor.lockState = CursorLockMode.Locked;

        RunnigMultiplier = 1.5f;
    }

    private void Update()
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
    }

    private void OnMove(InputValue value)
    {
        mDirection = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        mDeltaLook = value.Get<Vector2>();
    }
}
