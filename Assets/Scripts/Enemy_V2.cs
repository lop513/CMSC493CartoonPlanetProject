using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy_V2 : MonoBehaviour
{
    private PathfinderV2 pf;
    private Vector2Int cur_pos, next_pos;
    private float base_tick, tick, lerp;

    private System.Random rand_state;

    private EnemyHealth enemy_health;

    private PlayerHealth playerHealthScript;

    private int outdoorKills;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        enemy_health = GetComponent<EnemyHealth>();
        enemy_health.enemy = this;

        player = GameObject.Find("Player");

        playerHealthScript = player.GetComponent<PlayerHealth>();

    }

    // Update is called once per frame
    //TODO: M* (and / or) variable speed to avoid stacking
    void Update()
    {
        if (playerHealthScript != null)
        {
            outdoorKills = playerHealthScript.kills;
        }
        Scene currScene = SceneManager.GetActiveScene();
        string sceneName = currScene.name;

        bool los = pf.has_los_to_player(transform.position, 1f, 5);
        float dist = pf.dist_to_player(transform.position);
        float cut = 5f * pf.cell_dim;

        if(outdoorKills >= 15 && sceneName == "FieldLevel")
        {
            transform.position = transform.position + new Vector3(-9999, -9999, -9999);
            return;
        }

        if(sceneName == "ArenaLevel")
        {
            GameObject satellite = GameObject.Find("SatelliteDish");
            SatelliteScript satScript;
            satScript = satellite.GetComponent<SatelliteScript>();
            bool victCheck = satScript.victory;

            if(victCheck)
            {
                transform.position = transform.position + new Vector3(-9999, -9999, -9999);
                return;
            }
        }

        //Debug.Log(string.Format("{0} {1} {2}", los, dist, cut));

        //LOS and close - direct pathing!
        if (los && dist < cut)
        {
            Vector3 e = transform.position;
            Vector3 d = new Vector3(pf.player.position.x, e.y, pf.player.position.z);
            d = (d - e).normalized;

            transform.position = e + (1f / tick) * d;
        }
        else
        {
            //Lerp between Dijkstra SP
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
    }

    public void reset()
    {
        //start at a random open (hidden) space from the pathfinder
        List<Vector2Int> wrapped = new List<Vector2Int>(pf.hidden_pts);

        cur_pos = wrapped[rand_state.Next(wrapped.Count)];
        transform.position = pf.pts[cur_pos.x, cur_pos.y];

        //set tick to random, between 50% and 150% of base
        this.tick = rand_state.Next((int)this.base_tick) + (this.base_tick * 0.5f);

        next_pos = cur_pos; //force update on tick 0
        lerp = tick;
    }

    public void ConfigureState(PathfinderV2 pf, System.Random random_state, float tick)
    {
        this.pf = pf;
        this.rand_state = random_state;
        this.base_tick = tick;
        this.lerp = 0;
        reset();
    }
}
