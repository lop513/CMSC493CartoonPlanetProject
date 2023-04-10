using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_V2 : MonoBehaviour
{
    private PathfinderV2 pf;
    private Vector2Int cur_pos, next_pos;
    private float tick, lerp;

    private System.Random rand_state;

    private EnemyHealth enemy_health;
    

    // Start is called before the first frame update
    void Start()
    {
        enemy_health = GetComponent<EnemyHealth>();
        enemy_health.enemy = this;
    }

    // Update is called once per frame
    //TODO: M* (and / or) variable speed
    void Update()
    {
        //Lerp
        Vector3 e = pf.pts[cur_pos.x, cur_pos.y];
        Vector3 d = pf.pts[next_pos.x, next_pos.y] - e;
        float t = lerp / tick;
        transform.position = e + t * d;

        //Update local path
        if (t >= 1)
        {
            Vector2Int? next = pf.prev[next_pos];
            if (next == null || pf.obstructed_pts.Contains(next.Value)) return;

            cur_pos = next_pos;
            next_pos = next.Value;
            lerp = 0;
        }
        else
        {
            lerp++;
        }
    }

    public void reset()
    {
        //start at a random open (hidden) space from the pathfinder
        List<Vector2Int> wrapped = new List<Vector2Int>(pf.hidden_pts);

        cur_pos = wrapped[rand_state.Next(wrapped.Count)];
        transform.position = pf.pts[cur_pos.x, cur_pos.y];

        next_pos = cur_pos; //force update on tick 0
        lerp = tick;
    }

    public void ConfigureState(PathfinderV2 pf, System.Random random_state, float tick)
    {
        this.pf = pf;
        this.rand_state = random_state;
        this.tick = tick;
        this.lerp = 0;
        reset();
    }
}
