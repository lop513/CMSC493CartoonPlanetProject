using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteScript : MonoBehaviour
{
    private GameObject player;

    public Material openMat;

    public Renderer satMeshRend;

    public bool activated;
    public bool started;

    float timeLeft = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        activated = false;
        started = false;
    }

    // Update is called once per frame
    void Update()
    {
        //timeLeft -= Time.deltaTime;

        float distance = Vector3.Distance(player.transform.position, this.transform.position);
        if ((distance < 20) && Input.GetKeyDown("e"))
        {
            started = true;
        }

        if (started == true)
        {
            // Start countdown
            timeLeft -= Time.deltaTime;

            if (timeLeft < 0)
            {
                activated = true;
            }
        }


        if (activated == true) 
        {
            // Stop spawns

            satMeshRend.material = openMat;

            // Player wins game
        }
    }
}
