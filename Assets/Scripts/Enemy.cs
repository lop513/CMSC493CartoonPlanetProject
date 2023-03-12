using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Pathfinder pf;
    private Vector2Int prev, cur;
    private float lerp;
    public float ENEMY_TICK = 60;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Lerp
        Vector3 e = pf.pts[prev.x, prev.y];
        Vector3 d = pf.pts[cur.x, cur.y] - e;
        float t = lerp / ENEMY_TICK;
        transform.position = e + t * d;

        return; //TODO - FIXME

        //Update local path
        if (lerp == ENEMY_TICK)
        {
            if (pf.in_terrain.Contains(pf.agent_path[cur])) return;

            prev = cur;
            cur = pf.agent_path[cur];
            lerp = 0;
        }
        else
        {
            lerp++;
        }
    }

    public void ConfigureState(Pathfinder pf, Vector2Int prev, Vector2Int cur, float lerp, float enemy_tick)
    {
        this.pf = pf;
        this.prev = prev;
        this.cur = cur;
        this.lerp = lerp;
        this.ENEMY_TICK = enemy_tick;
    }

    //TODO - detect when colliding with other enemies, work backward along potentially many hops of path (parent) dict
    void OnCollisionEnter(Collision coll)
    {

    }

    void OnCollisionExit(Collision coll)
    {

    }
}
