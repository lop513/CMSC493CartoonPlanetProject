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

    public bool isGndGrounded = true;
    public bool isPlaneGrounded = true;
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

        if (Input.GetKeyDown(KeyCode.Space) && (isGndGrounded || isPlaneGrounded))
        {
            playerRgbd.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            isGndGrounded = false;
            isPlaneGrounded = false;
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

        if (isGndGrounded || isPlaneGrounded)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 inputVector = new Vector3(h, 0, v);
            inputVector.Normalize();
            inputVector *= acceleration;
            playerRgbd.AddRelativeForce(inputVector);
        }

        else
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 inputVector = new Vector3(h, 0, v);
            inputVector.Normalize();
            inputVector *= acceleration;
            playerRgbd.AddRelativeForce(inputVector);

            playerRgbd.AddRelativeForce((Input.GetAxis("Horizontal") * acceleration * walkAccelerationRatio) / 10, 0,
            (Input.GetAxis("Vertical") * acceleration * walkAccelerationRatio) / 10);
            

        }
        
        if (isGndGrounded || isPlaneGrounded)
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
                isGndGrounded = true;
                isPlaneGrounded = true;
            }
        }

        if (coll.gameObject.tag.Equals("gnd"))
        {
            isGndGrounded = true;
        }

        if (coll.gameObject.tag.Equals("Plane"))
        {
            isPlaneGrounded = true;
        }

    }

    void OnCollisionExit(Collision coll)
    {

        if (coll.gameObject.tag.Equals("gnd"))
        {
            isGndGrounded = false;
        }

        if (coll.gameObject.tag.Equals("Plane"))
        {
            isPlaneGrounded = false;
        }
    }

}
