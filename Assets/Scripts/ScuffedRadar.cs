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

    private List<GameObject> thumbs_s = null;
    private List<GameObject> child_thumbs_s = new List<GameObject>();

    private bool hasPressedButton = false;
    private GameObject doorButton = null;
    private GameObject door = null;

    private GameObject[] obstacles = null;

    private LineRenderer lineRenderer;

    public bool drawObstacles = true;

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
        doorButton = GameObject.Find("DoorButton");
        door = GameObject.Find("Exit");
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    public void press_button()
    {
        hasPressedButton = true;

        Invoke("update_thumbs_s", 1f);
    }

    void update_thumbs_s()
    {
        if (thumbs_s.Count != 0) return;

        //thumbsbs, if any
        thumbs_s = new List<GameObject>();
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> gameObjectsWithMatchingName = new List<GameObject>();
        foreach (GameObject obj in allGameObjects)
        {
            if (obj.name.Contains("ThumbsV2"))
            {
                thumbs_s.Add(obj);
            }
        }

        foreach (GameObject obj in thumbs_s)
        {
            GameObject o = make_child_sphere(Color.magenta);
            child_thumbs_s.Add(o);
            o.transform.position = double_transform(obj.transform.position);
        }
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
        if (pf.prev == null) return; //TODO - fix this garbage by explicitly setting update order

        if(thumbs_s == null)
        {
            //thumbsbs, if any
            thumbs_s = new List<GameObject>();
            GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
            List<GameObject> gameObjectsWithMatchingName = new List<GameObject>();
            foreach (GameObject obj in allGameObjects)
            {
                if (obj.name.Contains("ThumbsV2"))
                {
                    thumbs_s.Add(obj);
                }
            }

            foreach (GameObject obj in thumbs_s)
            {
                GameObject o = make_child_sphere(Color.magenta);
                child_thumbs_s.Add(o);
                o.transform.position = double_transform(obj.transform.position);
            }
        }

        if(spawner == null)
        {
            ; //no spawner in level
        }
        else if (!spawner.stall && enemies == null && spawner.swarm.Length != 0) //wait exactly three ticks
        {
            enemies = new GameObject[spawner.swarm.Length];
            for(int i = 0; i < spawner.swarm.Length; i++)
            {
                enemies[i] = make_child_sphere(Color.red);
            }

            obstacles = new GameObject[pf.obstacles.Length];
            for(int i = 0; i < pf.obstacles.Length; i++)
            {
                obstacles[i] = make_child_cube(pf.obstacles[i]);

                if(drawObstacles) obstacles[i].transform.position = double_transform(pf.obstacles[i].position);
                else
                {
                    obstacles[i].transform.position = new Vector3(-999, -999, -999);
                }
            }
        }
        else if(!spawner.stall)
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

        //Draw path to exit
        if(door != null || doorButton != null)
        {
            if (!hasPressedButton && doorButton == null) return;
            if (hasPressedButton && door == null) return;

            //Draw player 'path'
            Vector3 start;
            Vector2Int startPike, targetPike;

            startPike = pf.getClosestPike(pf.player.transform.position, true).Value;
            start = double_transform(
                pf.pikeToWs(
                    startPike
                )
            );

            if (!hasPressedButton)
            {
                targetPike = pf.getClosestPike(doorButton.transform.position, true).Value;
            }
            else
            {
                targetPike = pf.getClosestPike(door.transform.position, true).Value;
            }

            //TODO - create an array of intermediate transformed points (Vector3), starting from targetPike to startPike
            //Use Dijkstra dictionary, defined as: public Dictionary<Vector2Int, Vector2Int?> prev
            //Because we used getClosestPike with arg2=true, don't need to worry about null value, always a valid pike!
            List<Vector3> pathPoints = new List<Vector3>();
            Vector2Int currentPike = targetPike;

            //Debug.Log(string.Format("{0}, {1}", targetPike, startPike));
            //Debug.Log(pf.prev);

            while (currentPike != startPike)
            {
                Vector3 p = double_transform(pf.pikeToWs(currentPike));
                p.y = playerRenderObj.transform.position.y;
                pathPoints.Add(p);

                if (!pf.prev.ContainsKey(currentPike) || pf.prev[currentPike] == null) return; //TODO - This should never happen??? Why does it???
                currentPike = pf.prev[currentPike].Value;
            }

            pathPoints.Add(start); // Add the start point to the path


            lineRenderer.startColor = Color.yellow;
            lineRenderer.endColor = Color.yellow;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;

            //render!
            lineRenderer.positionCount = pathPoints.Count;
            for (int i = 0; i < pathPoints.Count; i++)
            {
                lineRenderer.SetPosition(i, pathPoints[i]);
            }

            //render direction
            lineRenderer.startColor = Color.yellow;
            lineRenderer.endColor = Color.yellow;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
        }
    }
}
