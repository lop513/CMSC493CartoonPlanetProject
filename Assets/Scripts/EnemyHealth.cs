using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;


public class EnemyHealth : MonoBehaviour
{
    public int currHealth = 15;
    public static bool isEnemyDead = false;
    public Material fullHealthMat;
    public Material orangeMat;
    public Material redMat;
    public float thrust = 20;

    private Transform playerTransform;
    private GameObject player;
    private Rigidbody playerrgbd;

    private PathfinderV2 pf;

    private PlayerHealth playerHealth;

    private Enemy enemy;

    public GameObject healthDrop;

    public Renderer enemyMeshRend;

    public void Start()
    {
        //enemyMat = GetComponent<MeshRenderer>().material;
        enemyMeshRend.material = fullHealthMat;
        player = GameObject.FindWithTag("Player");
        playerTransform = player.transform;
        //enemyMat = fullHealthMat;

        pf = GameObject.Find("Level Blocks").GetComponent<PathfinderV2>();

        playerHealth = player.GetComponent<PlayerHealth>();

        enemy = GetComponent<Enemy>();
    }

    public void Update()
    {
        if (currHealth == 10)// && isEnemyDead == false)
        {
            //enemyMat = orangeMat;
            enemyMeshRend.material = orangeMat;

        }

        if (currHealth == 5)// && isEnemyDead == false)
        {
            //enemyMat = redMat;
            enemyMeshRend.material = redMat;
        }

        //knockback player if within grid cell radius from them
        float grid_diff = Vector3.Magnitude(pf.pts[0, 1] - pf.pts[0, 0]);
        float pos_diff = Vector3.Magnitude(player.transform.position - transform.position);

        if (pos_diff < 1.0f * grid_diff)
        { 
            playerrgbd = player.GetComponent<Rigidbody>();
            Vector2 difference = playerrgbd.transform.position - transform.position;
            difference = difference.normalized * thrust;
            playerrgbd.AddForce(difference, ForceMode.Impulse);
            playerHealth.PlayerTakeDamage(5);
        }
    }

    public void TakeDamage(int damage)
    {
        currHealth -= damage;

        if(currHealth <= 0)// && isEnemyDead == false)
        {
            playerHealth.playEnemyDie();

            Instantiate(healthDrop, transform.position, transform.rotation);
    
            /*
            UnityEngine.Debug.Log("Dead: " + currHealth);
            //gameObject.GetComponent<Animator>().Play("DeathAnim");
            Destroy(gameObject);
            isEnemyDead = true;
            */
            currHealth = 15;
            enemyMeshRend.material = fullHealthMat;
            enemy.reset();

            

        }
        else
        {
            playerHealth.playEnemyHit();
        }
    }

    /*
    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            playerrgbd = player.GetComponent<Rigidbody>();
            Vector2 difference = playerrgbd.transform.position - transform.position;
            difference = difference.normalized * thrust;
            playerrgbd.AddForce(difference, ForceMode.Impulse);
            
        }
    }
    */
}

