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

    private GameObject[] thumbsList;

    public GameObject thumbs1;
    public GameObject thumbs2;
    public GameObject thumbs3;
    public GameObject thumbs4;
    public GameObject thumbs5;
    public GameObject thumbs6;

    float timeLeft = 5.0f;

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
            thumbs1.SetActive(true);
            thumbs2.SetActive(true);
            thumbs3.SetActive(true);
            thumbs4.SetActive(true);
            thumbs5.SetActive(true);
            thumbs6.SetActive(true);

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
