using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public int GRID_SIZE = 25;
    public Vector3[,] pts;
    public HashSet<Vector2Int> spawn_candidates;
    public Dictionary<Vector2Int, Vector2Int> agent_path;

    private Transform plane;
    private Transform player;
    private HashSet<Vector2Int> in_terrain;

    // Start is called before the first frame update
    void Start()
    {
        //references
        plane = transform.Find("Plane");
        player = GameObject.Find("Player").transform;

        //calculate grid points
        pts = new Vector3[GRID_SIZE, GRID_SIZE];
        for (int x = 0; x < GRID_SIZE; x++)
        {
            for (int z = 0; z < GRID_SIZE; z++)
            {
                pts[x, z] = plane.localToWorldMatrix.MultiplyPoint3x4(new Vector3(
                    (x / (float)(GRID_SIZE - 1) - 0.5f) * plane.localScale.x,
                    1.0f,
                    (z / (float)(GRID_SIZE - 1) - 0.5f) * plane.localScale.z
                ));
            }
        }

        //calculate grid points inside terrain
        //First, need all terrains
        Transform[] planes = transform.GetComponentsInChildren<Transform>(true /* includeInactive */);
        planes = System.Array.FindAll<Transform>(planes, t => t.name == "Plane" && t.tag != "gnd");

        //now, see if point lies in any
        in_terrain = new HashSet<Vector2Int>();
        for (int x = 0; x < GRID_SIZE; x++)
        {
            for (int z = 0; z < GRID_SIZE; z++)
            {
                foreach(Transform plane in planes)
                {
                    Vector3 closest = plane.gameObject.GetComponent<BoxCollider>().ClosestPoint(pts[x, z]);
                    if(closest == pts[x, z])
                    {
                        in_terrain.Add(new Vector2Int(x, z));
                        break;
                    }
                }
            }
        }
    }

    void BFS(Vector2Int start, bool nine_way, HashSet<Vector2Int> in_terrain, out HashSet<ValueTuple<Vector2Int, Vector2Int>> edges_, out HashSet<Vector2Int> visited_, out Dictionary<Vector2Int, Vector2Int> path_)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>(new Vector2Int[] { start });
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>(new Vector2Int[] { start });
        HashSet<ValueTuple<Vector2Int, Vector2Int>> edges = new HashSet<ValueTuple<Vector2Int, Vector2Int>>();

        Dictionary<Vector2Int, Vector2Int> path = new Dictionary<Vector2Int, Vector2Int>();
        path.Add(start, start);

        while(queue.Count != 0)
        {
            Vector2Int v = queue.Dequeue();

            //find neighbors
            HashSet<Vector2Int> neighbors = new HashSet<Vector2Int>();
            for(int dx = -1; dx <= 1; ++dx)
            {
                for(int dz = -1; dz <= 1; ++dz)
                {
                    if(!nine_way && Mathf.Abs(dx) + Mathf.Abs(dz) > 1) continue;

                    Vector2Int n = new Vector2Int(
                        Mathf.Clamp(v.x + dx, 0, GRID_SIZE - 1),
                        Mathf.Clamp(v.y + dz, 0, GRID_SIZE - 1)
                    );
                    neighbors.Add(n);
                }
            }
            neighbors.ExceptWith(in_terrain);
            
            //expand BFS wavefront over neighbors
            foreach(Vector2Int neighbor in neighbors)
            {
                if(!visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                    path.Add(neighbor, v);
                    edges.Add(new ValueTuple<Vector2Int, Vector2Int>(v, neighbor));
                }
            }
        }

        edges_ = edges;
        visited_ = visited;
        path_ = path;
    }

    // Update is called once per frame
    void Update()
    {
        //Compute which pike the player is closest to
        Vector3 playerPosInPlaneSpace = plane.worldToLocalMatrix.MultiplyPoint3x4(player.position);
        float cx = playerPosInPlaneSpace.x / plane.localScale.x + 0.5f;
        float cz = playerPosInPlaneSpace.z / plane.localScale.z + 0.5f;
        int ix = Mathf.Clamp(Mathf.RoundToInt(cx * (GRID_SIZE - 1)), 0, GRID_SIZE - 1);
        int iz = Mathf.Clamp(Mathf.RoundToInt(cz * (GRID_SIZE - 1)), 0, GRID_SIZE - 1);
        Vector2Int closestPike = new Vector2Int(ix, iz);

        //BFS all reachable nodes
        HashSet<ValueTuple<Vector2Int, Vector2Int>> edges;
        HashSet<Vector2Int> visited;
        BFS(new Vector2Int(ix, iz), true, in_terrain, out edges, out visited, out agent_path);

        //Compute set of pikes out of 'player view'
        spawn_candidates = new HashSet<Vector2Int>();
        foreach(Vector2Int candidate in visited)
        {
            Vector3 point = pts[candidate.x, candidate.y];
            Vector3 playerOnGround = new Vector3(
                player.position.x,
                1.0f,
                player.position.z
            );
            Vector3 direction = point - playerOnGround;

            if (Physics.Raycast(playerOnGround, direction, direction.magnitude)) spawn_candidates.Add(candidate);
        }

        //Draw edges
        foreach (ValueTuple<Vector2Int, Vector2Int> edge in edges)
        {
            Vector3 point1 = pts[edge.Item1.x, edge.Item1.y];
            Vector3 point2 = pts[edge.Item2.x, edge.Item2.y];
            Debug.DrawLine(point1, point2, Color.yellow);
        }

        //Draw debug pikes
        for (int x = 0; x < GRID_SIZE; x++)
        {
            for(int z = 0; z < GRID_SIZE; z++)
            {
                Vector3 point = pts[x,z];
                Vector2Int pair = new Vector2Int(x, z);

                if (pair == closestPike)
                {
                    Debug.DrawLine(point, point - new Vector3(0, 1, 0), Color.green);
                }
                else if(in_terrain.Contains(pair))
                {
                    Debug.DrawLine(point, point - new Vector3(0, 1, 0), Color.red);
                }
                else if(spawn_candidates.Contains(pair))
                {
                    Debug.DrawLine(point, point - new Vector3(0, 1, 0), Color.cyan);
                }
                else
                {
                    Debug.DrawLine(point, point - new Vector3(0, 1, 0), Color.white);
                }
            }
        }
    }
}
