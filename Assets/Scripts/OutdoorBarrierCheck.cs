using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutdoorBarrierCheck : MonoBehaviour
{
    private int outdoorKills;

    public GameObject playerHealthScript;

    private GameObject barrier;
    public Renderer exitMeshRend;
    public Material purpleMat;

    private GameObject[] enemies;

    private GameObject player;
    public GameObject doorOpen;
    public GameObject remainingEnemy;

    // Start is called before the first frame update
    void Start()
    {
        outdoorKills = 0;
        barrier = GameObject.Find("FakeBarrier");
    }

    // Update is called once per frame
    void Update()
    {

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        

        outdoorKills = playerHealthScript.GetComponent<PlayerHealth>().kills;

        // Spawn enemies until then, need to set to how many enemies we want to kill
        if (outdoorKills >= 15)
        {
            // Turn off spawner
            remainingEnemy.SetActive(true);

            if (enemies.Length == 2) // Currently two enemy prefabs that will
                                     // not be killed, need to increase if more prefabs
            {
                remainingEnemy.SetActive(false);
                doorOpen.SetActive(true);
                barrier.SetActive(false);
                exitMeshRend.material = purpleMat;


            }
        }
        
    }
}
