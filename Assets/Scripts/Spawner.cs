using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections.Specialized;
using System.Reflection.Emit;



public class Spawner : MonoBehaviour
{
    private Pathfinder pf;

    public GameObject enemyPrefab;
    private GameObject[] swarm;
    public int SWARM_SIZE = 1;
    public float SWARM_TICK = 120;

    public Vector2Int[] spawns;

    // Start is called before the first frame update
    void Start()
    {
        pf = GameObject.Find("Level Blocks").GetComponent<Pathfinder>();
        swarm = null;
    }


    // Update is called once per frame
    void Update()
    {

    }

    //Wait for path vectors to update before step
    void LateUpdate()
    {
        if (swarm == null) //weird synchronization tomfoolery
        {
            swarm = new GameObject[SWARM_SIZE];
            //List<Vector2Int> spawns = pf.spawn_candidates.OrderBy(x => UnityEngine.Random.value).Take(swarm.Length).ToList();

            for (int i = 0; i < Mathf.Min(spawns.Length, swarm.Length); i++)
            {
                swarm[i] = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
                Enemy script = swarm[i].AddComponent<Enemy>();
                script.ConfigureState(pf, spawns[i], spawns[i], 0, SWARM_TICK);
            }
        }
    }
}
