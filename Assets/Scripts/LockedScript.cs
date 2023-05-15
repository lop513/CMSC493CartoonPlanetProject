using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class LockedScript : MonoBehaviour
{
    private GameObject player;

    public GameObject door;

    public Material openMat;

    public Renderer doorMeshRend;
    public Renderer doorMeshRend2;
    public Renderer doorMeshRend3;
    public Renderer doorMeshRend4;
    public Renderer buttonMeshRend;

    public bool unlocked;

    public GameObject pressE;
    public GameObject doorOpen;

    float timeLeft = 3.0f;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        unlocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(player.transform.position, this.transform.position);

        RaycastHit ray;
        Physics.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out ray);

        if (ray.collider != null)
        {

            if (distance < 40 && unlocked != true && ray.collider.CompareTag("Button"))
            {
                pressE.SetActive(true);
            }

            else
            {
                pressE.SetActive(false);
            }

            if ((distance < 40) && Input.GetKeyDown("e") && ray.collider.CompareTag("Button"))
            {
                doorMeshRend.material = openMat;
                doorMeshRend2.material = openMat;
                doorMeshRend3.material = openMat;
                doorMeshRend4.material = openMat;
                buttonMeshRend.material = openMat;
                unlocked = true;
                pressE.SetActive(false);

                //update radar
                GameObject.Find("ScuffedRadar").GetComponent<ScuffedRadar>().press_button();
            }
        }
        if (unlocked)
        {
            pressE.SetActive(false);
            doorOpen.SetActive(true);

            timeLeft -= Time.deltaTime;

            if(timeLeft <= 0)
            {
                doorOpen.SetActive(false);
            }
        }
    }
}
