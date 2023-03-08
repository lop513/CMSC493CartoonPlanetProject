using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spawner : MonoBehaviour
{
    private Pathfinder pf;
    private ValueTuple<Vector2Int, Vector2Int, float>[]? swarm; //TODO - this needs to be a prefab

    public int SWARM_SIZE   = 10;
    public float SWARM_TICK = 120;

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
        if (swarm == null) //really not good
        {
            swarm = new ValueTuple<Vector2Int, Vector2Int, float>[SWARM_SIZE];
            List<Vector2Int> spawns = pf.spawn_candidates.OrderBy(x => UnityEngine.Random.value).Take(swarm.Length).ToList();
            for (int i = 0; i < swarm.Length; i++)
            {
                swarm[i] = new ValueTuple<Vector2Int, Vector2Int, float>(
                    spawns[i], //local start
                    spawns[i], //local end
                    0          //lerp
                );
            }
        }

        for (int i = 0; i < swarm.Length; i++)
        {
            if (swarm[i].Item3 == SWARM_TICK)
            {
                swarm[i].Item1 = swarm[i].Item2;
                swarm[i].Item2 = pf.agent_path[swarm[i].Item2];
                swarm[i].Item3 = 0;
            }
            else
            {
                swarm[i].Item3 += 1;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (swarm == null) return;

        for (int i = 0; i < swarm.Length; i++)
        { 
            Vector3 pos = pf.pts[swarm[i].Item1.x, swarm[i].Item1.y] + (swarm[i].Item3 / SWARM_TICK) * (pf.pts[swarm[i].Item2.x, swarm[i].Item2.y] - pf.pts[swarm[i].Item1.x, swarm[i].Item1.y]);
            Gizmos.DrawSphere(pos, 2.0f);
        }
    }
}
