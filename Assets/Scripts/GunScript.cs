using System;
using System.Collections;
using System.Collections.Generic;
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
    public float rotateSpeed = .3f, holdHeight = -.5f, holdSide = .5f;
    private int damage = 5;
    public float thrust = 20;

    private Transform EnemyTransform;
    private GameObject Enemy;
    public Rigidbody Enemyrgbd;

    void Start()
    {

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
    }

    void Shoot()
    {
        Vector3 e = transform.position;
        Vector3 d = Vector3.Normalize(transform.forward);

        //Color c = Color.white;
        if (Input.GetButtonDown("Fire1"))
        {
            /*
            gunrgbd = gun.GetComponent<Rigidbody>();
            Vector2 difference = gunrgbd.transform.position - transform.position;
            difference = difference.normalized * thrust;
            gunrgbd.AddForce(difference, ForceMode.Impulse);
            UnityEngine.Debug.Log("Check");
            */

            RaycastHit r;
            Physics.Raycast(e, d, out r);
            c = Color.red;

            //TODO - HIT CODE GOES HERE
            Transform t = r.transform;
            if (r.collider.CompareTag("Enemy"))
            {
                UnityEngine.Debug.Log(string.Format("Gun hit object: {0}" + damage, t));
                r.collider.GetComponent<EnemyHealth>().TakeDamage(damage);
                
                // Enemy gets knockbacked by gun hitting it
                /*
                Vector2 difference = Enemyrgbd.transform.position - transform.position;
                difference = difference.normalized * thrust;
                Enemyrgbd.AddForce(difference, ForceMode.Impulse);
                UnityEngine.Debug.Log("Check");
                */
            }
        }


        if (Input.GetButtonUp("Fire1"))
        {
            c = Color.white;
        }


        Debug.DrawLine(e, e + 100 * d, c);
    }
}
