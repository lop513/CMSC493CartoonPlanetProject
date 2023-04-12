using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LockedScript : MonoBehaviour
{
    private GameObject player;

    public GameObject door;

    public Material openMat;

    public Renderer doorMeshRend;
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

        if(distance < 3 && unlocked != true)
        {
            pressE.SetActive(true);
        }

        else
        {
            pressE.SetActive(false);
        }

        if ((distance < 3) && Input.GetKeyDown("e"))
        {
            doorMeshRend.material = openMat;
            buttonMeshRend.material = openMat;
            unlocked = true;
            pressE.SetActive(false);
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
