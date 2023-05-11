using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject cameraObj;
    [HideInInspector]
    float targetXRot, targetYRot;
    [HideInInspector]
    float targetXRotV, targetYRotV;
    private Color c = Color.white;
    public GameObject shell;
    public Transform shellSpawnPos, bulletSpawnPos;
    public float rotateSpeed = 0.1f, holdHeight = 2, holdSide = .5f;
    private float damage = .5f;
    public float thrust = 20;

    public GameObject animController;

    public GameObject shootAnimController;

    private Transform EnemyTransform;
    private GameObject Enemy;
    public Rigidbody Enemyrgbd;

    private PlayerHealth ph;

    public AudioClip gunshot;
    private AudioSource speaker;

    public float lastShootTime = -1.0f;
    public const float SHOOT_LOCKOUT = 0.66f;

    private Transform cameraTransform;

    private GameObject gunAnimContr;
    private GameObject animContr;

    public GameObject spawnPoint;
    public Camera m_Camera;

    int overheat = 0;
    int minHeat = 0;
    int maxHeat = 1000;
    int decVal = 1;
    int incVal = 1;
    bool heating = false;
    bool notFiring = true;
    float timeLeft = 5.0f;

    float gunAnimTimeLeft = 0.0f;
    float animTimeLeft = 0.0f;

    //private Animation bulletHole;

    void Start()
    {
        ph = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();

        speaker = GetComponent<AudioSource>();

        Cursor.visible = false; 
    }

    // Update is called once per frame
    void Update()
    { 

        Enemy = GameObject.FindWithTag("Enemy");
        Enemyrgbd = Enemy.GetComponent<Rigidbody>();
        Shoot();

        targetXRot = Mathf.SmoothDamp(targetXRot, FindObjectOfType<CameraScript>().xRot, ref targetXRotV, rotateSpeed);
        targetYRot = Mathf.SmoothDamp(targetYRot, FindObjectOfType<CameraScript>().yRot, ref targetYRotV, rotateSpeed);

        transform.position = cameraObj.transform.position + Quaternion.Euler(0, targetYRot, 0) * new Vector3(holdSide, holdHeight, 0);

        float clampedX = Mathf.Clamp(targetXRot, -70, 80);
        transform.rotation = Quaternion.Euler(-clampedX, targetYRot, rotateSpeed);

        if (overheat >= maxHeat)
        {
            UnityEngine.Debug.Log("OVERHEATED");
            heating = false;

            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0)
            {
                overheat = 0;
            }

        }

        else if (overheat <= minHeat)
        {
            overheat = minHeat;
            heating = true;
        }

        if (heating && notFiring)
        {
            overheat -= decVal;// decrease the value from overheat
        }

        if(timeLeft <= 0)
        {
            timeLeft = 5.0f;
        }

    }

    void Shoot()
    {
        Vector3 e = transform.position;
        e.y += .05f;
        Vector3 d = Vector3.Normalize(Camera.main.transform.forward);

        //Color c = Color.white;
        if (Input.GetButton("Fire1") && ph.playerHealth > 0 && overheat < maxHeat) //&& Time.time - SHOOT_LOCKOUT > lastShootTime)
        {
            notFiring = false;

            overheat += incVal; //increment heat
            heating = true; //heat check

            //lastShootTime = Time.time;
            speaker.PlayOneShot(gunshot);

            /*
            gunrgbd = gun.GetComponent<Rigidbody>();
            Vector2 difference = gunrgbd.transform.position - transform.position;
            difference = difference.normalized * thrust;
            gunrgbd.AddForce(difference, ForceMode.Impulse);
            UnityEngine.Debug.Log("Check");
            */

            RaycastHit r;
            //Physics.Raycast(e, d, out r);
            Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out r);
            c = Color.red;

            //Blast from gun Code
            gunAnimTimeLeft -= Time.deltaTime;

            if (gunAnimTimeLeft <= 0)
            {
                Destroy(gunAnimContr);
                gunAnimContr = Instantiate(shootAnimController, new Vector3(transform.position.x, transform.position.y,
                    transform.position.z), Quaternion.identity) as GameObject;
                gunAnimTimeLeft = .1f;

                gunAnimContr.transform.parent = this.transform;
                gunAnimContr.transform.localPosition = spawnPoint.transform.localPosition;
                gunAnimContr.transform.localScale = spawnPoint.transform.localScale;
                gunAnimContr.transform.localRotation = spawnPoint.transform.localRotation;
                //gunAnimContr.transform.rotation = Quaternion.FromToRotation(Vector3.up, r.normal);
                //gunAnimContr.transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
                //  m_Camera.transform.rotation * Vector3.up);
            }
            //Bullet Hole Code

            animTimeLeft -= Time.deltaTime;

            if (animTimeLeft <= 0)
            {
                Destroy(animContr);
                animContr = Instantiate(animController, r.point + (r.normal * .0001f), Quaternion.identity) as GameObject;
                //animContr.transform.position = animContr.transform.position + new Vector3(-.125f, 0, 0);
                animTimeLeft = .1f;


                if (r.collider != null)
                {
                    if (r.collider.CompareTag("Enemy"))
                    {
                        animContr.transform.parent = r.transform;
                    }
                    animContr.transform.rotation = Quaternion.FromToRotation(Vector3.forward, r.normal);
                }
            }

            //TODO - HIT CODE GOES HERE
            Transform t = r.transform;
            if (r.collider != null)
            {
                if (r.collider.CompareTag("Enemy"))
                {
                    //UnityEngine.Debug.Log(string.Format("Gun hit object: {0}" + damage, t));
                    if (r.collider.GetComponent<EnemyHealth>() != null)
                    {
                        r.collider.GetComponent<EnemyHealth>().TakeDamage(damage);
                    }

                    if (r.collider.GetComponent<ThumbsHealth>() != null)
                    {
                        r.collider.GetComponent<ThumbsHealth>().TakeDamage(damage);
                    }

                    // Enemy gets knockbacked by gun hitting it
                    /*
                    Vector2 difference = Enemyrgbd.transform.position - transform.position;
                    difference = difference.normalized * thrust;
                    Enemyrgbd.AddForce(difference, ForceMode.Impulse);
                    UnityEngine.Debug.Log("Check");
                    */
                }
            }
            
            
        } else {
            notFiring = true;

        }



        if (Input.GetButtonUp("Fire1"))
        {
            c = Color.white;
        }


        Debug.DrawLine(e, e + 100 * d, c);
    }

}
