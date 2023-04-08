using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyLookAt : MonoBehaviour
{
    public Transform player;

    void Start()
    {
        this.player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {

        if(EnemyHealth.isEnemyDead == false)
        {
            transform.LookAt(player);
        }
        
    }
}
