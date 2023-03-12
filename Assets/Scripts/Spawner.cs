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

    public int xSpawnLoc = 0;
    public int zSpawnLoc = 0;

    //public List<Vector2Int> spawnLocation = pf.spawn_candidates.OrderBy(x => xSpawnLoc).Take(swarm.Length).ToList();
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
        Vector2Int[] spawns = new Vector2Int[] {
                new Vector2Int(xSpawnLoc, zSpawnLoc)
        };
        if (swarm == null) //weird synchronization tomfoolery
        {
            swarm = new GameObject[SWARM_SIZE];
            //List<Vector2Int> spawns; // = pf.spawn_candidates.OrderBy(x => xSpawnLoc).Take(swarm.Length).ToList();

            spawns = new Vector2Int[] {
                new Vector2Int(xSpawnLoc, zSpawnLoc)
            };

            for (int i = 0; i < swarm.Length; i++) { 
                swarm[i] = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
                Enemy script = swarm[i].AddComponent<Enemy>();
                script.ConfigureState(pf, spawns[i], spawns[i], 0, SWARM_TICK);
            }
        }
    }
}
