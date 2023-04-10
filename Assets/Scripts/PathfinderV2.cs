using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathfinderV2 : MonoBehaviour
{
    public int GRID_SIZE = 25;
    public Vector3[,] pts;

    //refs
    public  Transform plane;
    public  Transform player;
    public  Transform[] obstacles;

    public float cell_dim;

    //previous state - if changed, recompute
    private Vector3 lastPosition;
    private Quaternion lastRotation;
    private Vector3 lastScale;

    //Vec2 storing of points
    public Vector2Int closestPike;
    public HashSet<Vector2Int> obstructed_pts;
    public HashSet<Vector2Int> hidden_pts;

    //Graph
    private HashSet<Vector2Int> graph_v;
    private Dictionary<ValueTuple<Vector2Int, Vector2Int>, float> graph_e;
    private Dictionary<Vector2Int, float> dist;
    public  Dictionary<Vector2Int, Vector2Int?> prev;


    Vector3[,] compute_points()
    {
        Vector3[,] points = new Vector3[GRID_SIZE, GRID_SIZE];

        for (int x = 0; x < GRID_SIZE; x++)
        {
            for (int z = 0; z < GRID_SIZE; z++)
            {
                points[x, z] = plane.TransformPoint(new Vector3(
                    -5.0f + 10.0f * (x / (float)(GRID_SIZE - 1)),
                    0.5f,
                    -5.0f + 10.0f * (z / (float)(GRID_SIZE - 1))
                ));
            }
        }
        cell_dim = Vector3.Distance(points[0, 1], points[0, 0]);
        return points;
    }

    HashSet<Vector2Int> compute_obstructed_points()
    {
        HashSet<Vector2Int> in_terrain = new HashSet<Vector2Int>();

        LayerMask obstacleLayer = LayerMask.GetMask("pf_blk");
        Vector3 halfExtents = new Vector3(
            Mathf.Abs(0.4f * (pts[1,0].x - pts[0,0].x)),
            0.5f,
            Mathf.Abs(0.4f * (pts[0,1].z - pts[0,0].z))
        );

        for (int x = 0; x < GRID_SIZE; x++)
        {
            for (int z = 0; z < GRID_SIZE; z++)
            { 
                bool isObstructed = Physics.CheckBox(pts[x,z], halfExtents, plane.rotation, obstacleLayer);
                if(isObstructed)
                {
                    in_terrain.Add(new Vector2Int(x, z));
                }
            }
        }
        return in_terrain;
    }

    HashSet<Vector2Int> compute_hidden_points()
    {
        HashSet<Vector2Int> hidden_points_ = new HashSet<Vector2Int>();
        foreach (Vector2Int candidate in graph_v)
        {
            Vector3 point = pts[candidate.x, candidate.y];

            if(!has_los_to_player(point, 1f, 5)) hidden_points_.Add(candidate);
        }
        return hidden_points_;
    }

    public bool has_los_to_player(Vector3 point, float y_off, int n)
    {
        for (int i = 0; i < n; ++i)
        {
            Vector3 playerOnGround = new Vector3(
                player.position.x,
                y_off + i,
                player.position.z
            );

            Vector3 direction = point - playerOnGround;

            if (!Physics.Raycast(playerOnGround, direction, direction.magnitude))
            {
                return true; //successfully cast ray
            }
        }
        return false;
    }

    public float dist_to_player(Vector3 point)
    {
        return Vector3.Distance(point, player.position);
    }

    void make_weighted_graph(out HashSet<Vector2Int> V_, out Dictionary<ValueTuple<Vector2Int, Vector2Int>, float> E_)
    {
        //Vertices
        HashSet<Vector2Int> V = Enumerable.Range(0, GRID_SIZE).SelectMany(x => Enumerable.Range(0, GRID_SIZE), (x, y) => new Vector2Int(x, y)).ToHashSet();
        V.ExceptWith(obstructed_pts);

        //Edges
        //Edges can be straight between two V2Is (weight=1.0f) or diagonal (weight~1.41f)
        //Diagonal edges should not *cross* obstructed pts on the off-diagonal.
        //e.g.  X 3
        //      1 2
        //
        // 1 cannot form an edge with 3, as 'X' is obstructed.
        Dictionary<ValueTuple<Vector2Int, Vector2Int>, float> E = new Dictionary<ValueTuple<Vector2Int, Vector2Int>, float>();
        Vector2Int[] offsets = {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1)
        };

        foreach (Vector2Int vertex in V)
        {
            foreach (Vector2Int offset in offsets)
            {
                Vector2Int neighbor = vertex + offset;

                if (V.Contains(neighbor))
                {
                    float edgeWeight;
                    bool isDiagonal = Mathf.Abs(offset.x) == Mathf.Abs(offset.y);

                    if (isDiagonal)
                    {
                        bool obstructedCross = obstructed_pts.Contains(vertex + new Vector2Int(offset.x, 0)) || obstructed_pts.Contains(vertex + new Vector2Int(0, offset.y));

                        if (obstructedCross)
                        {
                            continue;
                        }

                        edgeWeight = 1.41f;
                    }
                    else
                    {
                        edgeWeight = 1.0f;
                    }

                    ValueTuple<Vector2Int, Vector2Int> edge = new ValueTuple<Vector2Int, Vector2Int>(vertex, neighbor);
                    E[edge] = edgeWeight;
                }
            }
        }

        V_ = V;
        E_ = E;
    }

    void dijkstra(Vector2Int src, HashSet<Vector2Int> V, Dictionary<ValueTuple<Vector2Int, Vector2Int>, float> E, out Dictionary<Vector2Int, float> dist_, out Dictionary<Vector2Int, Vector2Int?> prev_)
    {
        dist_ = new Dictionary<Vector2Int, float>();
        prev_ = new Dictionary<Vector2Int, Vector2Int?>();

        // Initialize distances and previous vertices
        foreach (Vector2Int vertex in V)
        {
            dist_[vertex] = float.PositiveInfinity;
            prev_[vertex] = null;
        }

        dist_[src] = 0;

        PriorityQueue<Vector2Int> pq = new PriorityQueue<Vector2Int>();
        pq.Enqueue(src, 0);

        while (pq.Count > 0)
        {
            Vector2Int current_vertex = pq.Dequeue();
            float current_distance = dist_[current_vertex];

            foreach (Vector2Int neighbor in V)
            {
                if (E.TryGetValue((current_vertex, neighbor), out float edge_weight))
                {
                    float tentative_distance = current_distance + edge_weight;

                    if (tentative_distance < dist_[neighbor])
                    {
                        dist_[neighbor] = tentative_distance;
                        prev_[neighbor] = current_vertex;
                        pq.Enqueue(neighbor, (int)tentative_distance);
                    }
                }
            }
        }
    }

    bool update_closest_pike()
    {
        bool changed_cell = false;

        Vector3 playerPosInPlaneSpace = plane.InverseTransformPoint(player.position);
        float cx = (playerPosInPlaneSpace.x + 5f) / 10f;
        float cz = (playerPosInPlaneSpace.z + 5f) / 10f;
        int ix = Mathf.Clamp(Mathf.RoundToInt(cx * (GRID_SIZE - 1)), 0, GRID_SIZE - 1);
        int iz = Mathf.Clamp(Mathf.RoundToInt(cz * (GRID_SIZE - 1)), 0, GRID_SIZE - 1);

        //Debug.Log(string.Format("{0}, {1}, {2}, {3}", cx, cz, ix, iz));

        Vector2Int closestPikeCandidate = new Vector2Int(ix, iz);
        if(!obstructed_pts.Contains(closestPikeCandidate))
        {
            if(closestPikeCandidate != closestPike)
            {
                changed_cell = true; //Rerun pathfinder
            }

            closestPike = closestPikeCandidate;
        }
        return changed_cell;
    }


    bool has_plane_changed()
    {
        return plane.position != lastPosition || plane.rotation != lastRotation || plane.localScale != lastScale;
    }

    void update_plane_change()
    {
        lastPosition = plane.position;
        lastRotation = plane.rotation;
        lastScale = plane.localScale;
    }

    // Start is called before the first frame update
    void Start()
    {
        plane = transform.Find("Plane");
        player = GameObject.Find("Player").transform;
        obstacles = System.Array.FindAll<Transform>(
            transform.GetComponentsInChildren<Transform>(true /* includeInactive */),
            x => x.gameObject.layer == LayerMask.NameToLayer("pf_blk")
        );

        //Construct grid points
        pts = compute_points();
        obstructed_pts = compute_obstructed_points();

        //Construct graph
        make_weighted_graph(out graph_v, out graph_e);

        //update
        closestPike = new Vector2Int(0, 0);
        update_plane_change();
    }

    // Update is called once per frame
    void Update()
    {
        if (has_plane_changed()) //recompute points
        {
            pts = compute_points();
            obstructed_pts = compute_obstructed_points();
            make_weighted_graph(out graph_v, out graph_e);
            update_plane_change();
        }

        
        if(update_closest_pike()) //Player moved into different cell, rerun Dijkstra, obstructed points
        {
            dijkstra(closestPike, graph_v, graph_e, out dist, out prev);
            hidden_pts = compute_hidden_points();
        }

        //Draw debug pikes
        if(true)
        {
            for (int x = 0; x < GRID_SIZE; x++)
            {
                for (int z = 0; z < GRID_SIZE; z++)
                {
                    Vector3 point = pts[x, z];
                    Vector2Int pt_i = new Vector2Int(x, z);

                    Color c = Color.green;
                    if (pt_i == closestPike)
                    {
                        c = Color.blue;
                    }
                    else if (obstructed_pts.Contains(pt_i))
                    {
                        c = Color.red;
                    }
                    else if (hidden_pts.Contains(pt_i))
                    {
                        c = Color.magenta;
                    }
                    else
                    {
                        c = Color.green;
                    }

                    if (c != Color.blue) continue;
                    Debug.DrawLine(point, point + new Vector3(0, 20, 0), c);
                }
            }
        }
        

        //Draw graph edges
        if(false)
        {
            foreach (ValueTuple<Vector2Int, Vector2Int> edge in graph_e.Keys)
            {
                Vector3 point1 = pts[edge.Item1.x, edge.Item1.y];
                Vector3 point2 = pts[edge.Item2.x, edge.Item2.y];
                Debug.DrawLine(point1, point2, Color.yellow);
            }
        }
        

        //Draw dijkstra edges
        if(true)
        {
            foreach (Vector2Int v in graph_v)
            {
                Vector2Int? parent;
                if ((parent = prev[v]) != null)
                {
                    Vector3 point1 = pts[v.x, v.y] + new Vector3(0f, 0.5f, 0f);
                    Vector3 point2 = pts[parent.Value.x, parent.Value.y] + new Vector3(0f, 0.5f, 0f);
                    Debug.DrawLine(point1, point2, Color.blue);
                }
            }
        }
        

        /*
        Color c = Color.yellow;
        Vector3 topLeft = plane.TransformPoint(new Vector3(-5, 0, 5));
        Vector3 topRight = plane.TransformPoint(new Vector3(5, 0, 5));
        Vector3 bottomLeft = plane.TransformPoint(new Vector3(-5, 0, -5));
        Vector3 bottomRight = plane.TransformPoint(new Vector3(5, 0, -5));

        Debug.DrawLine(topLeft, topLeft  + new Vector3(0, 10, 0), c);
        Debug.DrawLine(topRight, topRight + new Vector3(0, 10, 0), c);
        Debug.DrawLine(bottomLeft, bottomLeft + new Vector3(0, 10, 0), c);
        Debug.DrawLine(bottomRight, bottomRight + new Vector3(0, 10, 0), c);
        */
    }
}
