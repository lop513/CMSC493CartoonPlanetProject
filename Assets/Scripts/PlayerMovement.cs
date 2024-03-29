using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Mime;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    //public bool isGndGrounded = true;
    //public bool isPlaneGrounded = true;
    public bool grounded;
    private PathfinderV2 pf;

    Rigidbody playerRgbd;
    public float jumpVelocity = 20;
    //float maxSlope = 45;
    private Scene currentScene;
    private string sceneName;

    private float stopCheckTimeLeft = .2f;
    private float stopTimeLeft = .2f;

    void Start()
    {
        acceleration = 5;
        maxWalkSpeed = 5;
        stopTimeLeft = .2f;
        stopCheckTimeLeft = .2f;
    }

    void Awake()
    {
        playerRgbd = GetComponent<Rigidbody>();
        pf = GameObject.Find("Level Blocks").GetComponent<PathfinderV2>();
        grounded = false;
        currentScene = SceneManager.GetActiveScene();

    }


    void Update()
    {

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;


        if ((Input.GetAxisRaw("Horizontal") == 0 && grounded) && (Input.GetAxisRaw("Vertical") == 0 && grounded))
        {
            stopCheckTimeLeft -= Time.deltaTime;
            if (stopCheckTimeLeft <= 0)
            {
                stopTimeLeft -= Time.deltaTime;
                acceleration = 0;
                maxWalkSpeed = 0;
                
                if (stopTimeLeft <= 0)
                {
                    acceleration = 5;
                    maxWalkSpeed = 5;

                }
                
            }

        }

        else
        {
            stopTimeLeft = .2f;
            stopCheckTimeLeft = .2f;



            
            if (Input.GetButtonUp("Shift")) 
            {
                acceleration = 5;
                maxWalkSpeed = 5;
            }
            

            if ((sceneName == "StarshipCockpit") && Input.GetButtonDown("Shift"))
            {
                acceleration = 50;
                maxWalkSpeed = 8;
            }

            if ((sceneName == "StarshipCargoBay") && Input.GetButtonDown("Shift"))
            {
                acceleration = 60;
                maxWalkSpeed = 8;
            }


            if ((sceneName == "FieldLevel") && Input.GetButtonDown("Shift"))
            {
                acceleration = 75;
                maxWalkSpeed = 8;
            }


            if ((sceneName == "ArenaLevel") && Input.GetButtonDown("Shift"))
            {
                acceleration = 75;
                maxWalkSpeed = 8;
            }
        }

        Jump();
        Move();
        Cursor.lockState = CursorLockMode.Confined;
        sceneName = currentScene.name;
    }

    void Jump()
    {

        if (Input.GetKeyDown(KeyCode.Space) && (grounded))
        {
            playerRgbd.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            //isGndGrounded = false;
            //isPlaneGrounded = false;
        }
    }

    void FixedUpdate()
    {
        Vector3 e = transform.position;
        Vector3 d = new Vector3(0, -1, 0);
        grounded = Physics.Raycast(e, d, 2);

        foreach (Transform plane in pf.obstacles)
        { 
            BoxCollider coll = plane.gameObject.GetComponent<BoxCollider>();
            Vector3 pt = e + 2 * d;
            if (coll.ClosestPoint(pt) == pt) //down vector within collider
            {
                coll.sharedMaterial.staticFriction = 2;
                coll.sharedMaterial.dynamicFriction = 30;
            }
            else
            {
                coll.sharedMaterial.staticFriction = 0;
                coll.sharedMaterial.dynamicFriction = 0;
            }

            //Debug.Log(string.Format("{0},{1},{2}", plane, coll.sharedMaterial.dynamicFriction, pt));
        }

        if (Mathf.Abs(playerRgbd.velocity.y) > 15)
        {
            playerRgbd.velocity = new Vector3(
                playerRgbd.velocity.x,
                Mathf.Sign(playerRgbd.velocity.y) * 15,
                playerRgbd.velocity.z
            );
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

        if (grounded)
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


        if (grounded)
        {
            float xMove = Mathf.SmoothDamp(playerRgbd.velocity.x, 0, ref walkDeaccelerateX, deacclerate);
            float zMove = Mathf.SmoothDamp(playerRgbd.velocity.z, 0, ref walkDeaccelerateZ, deacclerate);
            playerRgbd.velocity = new Vector3(xMove, playerRgbd.velocity.y, zMove);
        }

    }

}
