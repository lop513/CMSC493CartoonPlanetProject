using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float lookSensitivity = 2.0f;
    public float lookSmoothDamp = 0.5f;
    [HideInInspector]
    public float xRot, yRot;
    [HideInInspector]
    public float currentX, currentY;
    [HideInInspector]
    public float xRotV, yRotV;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        //Cursor.visible = false;
        
        yRot += Input.GetAxis("Mouse X") * lookSensitivity;
        xRot += Input.GetAxis("Mouse Y") * lookSensitivity;

        currentX = Mathf.SmoothDamp(currentX, xRot, ref xRotV, lookSmoothDamp);
        currentY = Mathf.SmoothDamp(currentY, yRot, ref yRotV, lookSmoothDamp);

        xRot = Mathf.Clamp(xRot, -80, 80);

        transform.rotation = Quaternion.Euler(-currentX, currentY, 0);
        
    }
}

