using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject cameraObj;
    public float acceleration;
    public float walkAccelerationRatio;

    public float maxWalkSpeed;
    public float deacclerate = 2;
    [HideInInspector]
    public Vector2 horizontalMovement;

    [HideInInspector]
    public float walkDeaccelerateX;
    [HideInInspector]
    public float walkDeaccelerateZ;

    [HideInInspector]
    public bool isGrounded = true;
    Rigidbody playerRgbd;
    public float jumpVelocity = 20;
    float maxSlope = 45;

    void Awake()
    {
        playerRgbd = GetComponent<Rigidbody>();
    }


    void Update()
    {
        Jump();
        Move();
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRgbd.AddForce(0, jumpVelocity, 0);
        }
    }

    void Move()
    {
        horizontalMovement = new Vector2(playerRgbd.velocity.x, playerRgbd.velocity.z);

        if (horizontalMovement.magnitude > maxWalkSpeed)
        {
            horizontalMovement = horizontalMovement.normalized;
            horizontalMovement *= maxWalkSpeed;
        }

        playerRgbd.velocity = new Vector3(horizontalMovement.x, playerRgbd.velocity.y, horizontalMovement.y);

        transform.rotation = Quaternion.Euler(0, cameraObj.GetComponent<CameraScript>().currentY, 0);

        if (isGrounded)
        {
            playerRgbd.AddRelativeForce(Input.GetAxis("Horizontal") * acceleration, 0, Input.GetAxis("Vertical") * acceleration);
        } 

        else
        {
            
           playerRgbd.AddRelativeForce((Input.GetAxis("Horizontal") * acceleration * walkAccelerationRatio)/10, 0,
           (Input.GetAxis("Vertical") * walkAccelerationRatio * acceleration)/10);
        }

        if (isGrounded)
        {
            float xMove = Mathf.SmoothDamp(playerRgbd.velocity.x, 0, ref walkDeaccelerateX, deacclerate);
            float zMove = Mathf.SmoothDamp(playerRgbd.velocity.z, 0, ref walkDeaccelerateZ, deacclerate);
            playerRgbd.velocity = new Vector3(xMove, playerRgbd.velocity.y, zMove);
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        foreach (ContactPoint contact in coll.contacts)
        {
            if (Vector3.Angle(contact.normal, Vector3.up) < maxSlope)
            {
                isGrounded = true;
            }
        }

    }

    void OnCollisionExit(Collision coll)
    {
        
        if (coll.gameObject.name.Equals("Plane"))
        {
            isGrounded = false;
        }
        
    }
}
