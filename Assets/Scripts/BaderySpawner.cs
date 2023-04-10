using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaderySpawner : MonoBehaviour
{
    private PathfinderV2 pf;

    public GameObject enemyPrefab;
    public GameObject[] swarm = null;
    public int SWARM_SIZE = 1;
    public float SWARM_TICK = 120;

    private System.Random random_state;
    public int random_seed = 777;

    // Start is called before the first frame update
    void Start()
    {
        pf = GameObject.Find("Level Blocks").GetComponent<PathfinderV2>();
        random_state = new System.Random(random_seed);
    }

    // Update is called once per frame
    void Update()
    {
        if(pf.hidden_pts != null && swarm.Length == 0) //wait exactly two ticks
        {
            swarm = new GameObject[SWARM_SIZE];
            for (int i = 0; i < swarm.Length; i++)
            {
                swarm[i] = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
                Enemy_V2 script = swarm[i].AddComponent<Enemy_V2>();

                //sideload in variables
                script.ConfigureState(pf, random_state, SWARM_TICK);
            }
        }
    }
}
