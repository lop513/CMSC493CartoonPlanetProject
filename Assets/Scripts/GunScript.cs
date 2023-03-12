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
        Vector3 e = transform.position;
        Vector3 d = Vector3.Normalize(transform.forward);

        Color c = Color.white;
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit r;
            Physics.Raycast(e, d, out r);
            c = Color.red;

            //TODO - HIT CODE GOES HERE
            Transform t = r.transform;
            Debug.Log(string.Format("Gun hit object: {0}", t));
        }

        Debug.DrawLine(e, e + 100 * d, c);
    }
}
