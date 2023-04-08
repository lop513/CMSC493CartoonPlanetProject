using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour
{
    private Pathfinder pf;
    private Vector2Int first, prev, cur;
    private float lerp;
    public float ENEMY_TICK_BASE = 60;
    public float ENEMY_TICK_ACTUAL;

    public GameObject player;
    private PlayerHealth phInstance;

    // Start is called before the first frame update
    void Start()
    {
        //phInstance.enemyKnockedBack = false;
        //knockBack = false;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindWithTag("Player");
        phInstance = player.GetComponent<PlayerHealth>();
        //Lerp
        Vector3 e = pf.pts[prev.x, prev.y];
        Vector3 d = pf.pts[cur.x, cur.y] - e;
        float t = lerp / ENEMY_TICK_ACTUAL;
        transform.position = e + t * d;

        //return; //TODO - FIXME
        /*
        if (!phInstance.enemyKnockedBack)
        {
            transform.position = e + t * d;
        }
        */
        //Update local path
        if (lerp >= ENEMY_TICK_ACTUAL)
        {
            if (!pf.agent_path.ContainsKey(cur) || pf.in_terrain.Contains(pf.agent_path[cur])) return;

            prev = cur;
            cur = pf.agent_path[cur];
            lerp = 0;
        }
        else
        {
            lerp++;
        }
    }

    public void reset()
    {
        /*
        this.prev = first;
        this.cur = first;
        */
        int randomIndex = Random.Range(0, pf.spawn_candidates.Count - 1);
        this.prev = pf.spawn_candidates.ToList()[randomIndex];
        this.cur = this.prev;

        this.lerp = 0;

        this.ENEMY_TICK_ACTUAL = (0.5f * (Random.value - 0.5f)) * ENEMY_TICK_BASE + ENEMY_TICK_BASE;
    }

    public void ConfigureState(Pathfinder pf, Vector2Int prev, Vector2Int cur, float lerp, float enemy_tick)
    {
        this.pf = pf;
        this.prev = prev;
        this.cur = cur;
        this.lerp = lerp;
        this.ENEMY_TICK_BASE = enemy_tick;
        this.ENEMY_TICK_ACTUAL = (0.5f * (Random.value - 0.5f)) * ENEMY_TICK_BASE + ENEMY_TICK_BASE;

        this.first = prev;
    }

    //TODO - detect when colliding with other enemies, work backward along potentially many hops of path (parent) dict
    void OnCollisionEnter(Collision coll)
    {


    }

    void OnCollisionExit(Collision coll)
    {

    }
}
