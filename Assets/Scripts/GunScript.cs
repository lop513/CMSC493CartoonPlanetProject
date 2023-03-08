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

    public GameObject shell;
    public Transform shellSpawnPos, bulletSpawnPos;
    public float rotateSpeed = .3f, holdHeight = -.5f, holdSide = .5f;

    // Update is called once per frame
    void Update()
    {
        Shoot();

        targetXRot = Mathf.SmoothDamp(targetXRot, FindObjectOfType<CameraScript>().xRot, ref targetXRotV, rotateSpeed);
        targetYRot = Mathf.SmoothDamp(targetYRot, FindObjectOfType<CameraScript>().yRot, ref targetYRotV, rotateSpeed);

        transform.position = cameraObj.transform.position + Quaternion.Euler(0, targetYRot, 0) * new Vector3(holdSide, holdHeight, 0);

        float clampedX = Mathf.Clamp(targetXRot, -70, 80);
        transform.rotation = Quaternion.Euler(-clampedX, targetYRot, rotateSpeed);
    }

    void Shoot()
    {
        /*
        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }
        */
        /*
        else if (Input.GetButton("Fire1"))
        {
            Fire();
        }
        */
    }

    void Fire()
    {
        GameObject shellCopy = Instantiate<GameObject>(shell, shellSpawnPos.position, Quaternion.identity) as GameObject;
        RaycastHit variable;
        bool status = Physics.Raycast(bulletSpawnPos.position, bulletSpawnPos.forward, out variable, 100);

        if (status) 
        {
            Debug.Log(variable.collider.gameObject.name);
        }
    }
}
