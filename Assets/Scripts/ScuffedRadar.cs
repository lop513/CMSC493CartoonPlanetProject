using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScuffedRadar : MonoBehaviour
{
    private PathfinderV2 pf;
    private BaderySpawner spawner;
    private Transform player;

    private GameObject playerRenderObj;
    private GameObject goalRenderObj;
    private GameObject[] enemies = null;

    private GameObject[] obstacles = null;
    
    // Start is called before the first frame update
    void Start()
    {
        pf = GameObject.Find("Level Blocks").GetComponent<PathfinderV2>();

        if(GameObject.Find("Badery Spawner") != null)
        {
            spawner = GameObject.Find("Badery Spawner").GetComponent<BaderySpawner>();
        }
        
        player = GameObject.Find("Player").transform;

        //children
        playerRenderObj = make_child_sphere(Color.cyan);
        goalRenderObj = make_child_sphere(Color.green);

        //children - exit button and exit
        
    }

    GameObject make_child_sphere(Color c)
    {
        GameObject rv = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        rv.transform.SetParent(transform, false);
        rv.transform.localScale = Vector3.one * 0.5f;

        Material sphereMaterial = new Material(Shader.Find("Standard"));
        sphereMaterial.color = c;
        rv.GetComponent<Renderer>().material = sphereMaterial;

        return rv;
    }

    GameObject make_child_cube(Transform cubeTransform)
    { 
        // Create a cube primitive as a child of the current GameObject
        GameObject rv = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rv.transform.SetParent(transform, false);

        // Get the scale and extents of the provided cube
        rv.transform.localScale = new Vector3(
            (cubeTransform.localScale.x / (pf.plane.localScale.x * 1f)) * (transform.localScale.x),
            1f,
            (cubeTransform.localScale.z / (pf.plane.localScale.z * 1f)) * (transform.localScale.z)
        );

        // Assign the same material as the provided cube's material

        Material cubeMaterial;
        if (cubeTransform.gameObject.name.Contains("Barrier")) // set to all black
        {
            cubeMaterial = new Material(Shader.Find("Standard"));
            cubeMaterial.color = Color.black;
        }
        else if(cubeTransform.gameObject.name.Contains("Ceiling") ||
                cubeTransform.gameObject.name.Contains("Exit") ||
                cubeTransform.gameObject.name.Contains("Entrance")
        )
        {
            //Invisible material
            cubeMaterial = new Material(Shader.Find("Transparent/Diffuse"));
            cubeMaterial.color = new Color(0, 0, 0, 0);
        }
        else
        {
            cubeMaterial = cubeTransform.GetComponent<Renderer>().material;
        }
        rv.GetComponent<Renderer>().material = cubeMaterial;

        return rv;
    }

    Vector3 double_transform(Vector3 p)
    {
        //transform from world space to level block space
        Vector3 level_block_space = pf.plane.InverseTransformPoint(p);

        level_block_space.y = 0;

        //transform from level block space to world space about transformed radar
        Vector3 ws = transform.TransformPoint(level_block_space);

        return ws;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawner == null)
        {
            ; //no spawner in level
        }
        else if (enemies == null && spawner.swarm.Length != 0) //wait exactly three ticks
        {
            enemies = new GameObject[spawner.swarm.Length];
            for(int i = 0; i < spawner.swarm.Length; i++)
            {
                enemies[i] = make_child_sphere(Color.red);
            }
            goalRenderObj.transform.position = double_transform(GameObject.Find("Exit").transform.position);
            goalRenderObj.transform.position += new Vector3(0f, 0.3f, 0f);

            obstacles = new GameObject[pf.obstacles.Length];
            for(int i = 0; i < pf.obstacles.Length; i++)
            {
                obstacles[i] = make_child_cube(pf.obstacles[i]);
                obstacles[i].transform.position = double_transform(pf.obstacles[i].position);
            }
        }
        else
        {
            //Get all spawned enemies and draw them, transformed, to the screen
            for (int i = 0; i < spawner.swarm.Length; i++)
            {
                Vector3 p = double_transform(spawner.swarm[i].transform.position);
                enemies[i].transform.position = p;
            }
        }
        
        //Draw Player to screen
        playerRenderObj.transform.position = double_transform(player.position);
        playerRenderObj.transform.position += new Vector3(0f, 0.3f, 0f);
    }
}
