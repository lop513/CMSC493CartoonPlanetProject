using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class ThumbsHealth : MonoBehaviour
{
    public int currHealth = 25;
    public static bool isEnemyDead = false;
    public Material fullHealthMat;
    public Material orangeMat;
    public Material redMat;

    private Transform playerTransform;
    private GameObject player;
    private Rigidbody playerrgbd;

    private PathfinderV2 pf;

    private PlayerHealth playerHealth;

    private Enemy_V2 enemy;

    public GameObject healthDrop;

    public Renderer enemyMeshRend;

    public SpriteRenderer healthSpriteRend;

    public Sprite twoFiveHealthSprite;
    public Sprite twentyHealthSprite;
    public Sprite oneFiveHealthSprite;
    public Sprite tenHealthSprite;
    public Sprite fiveHealthSprite;
    public Sprite zeroHealthSprite;

    private LineRenderer lineRenderer;

    private Vector3 e, target;
    private Vector3 swingVelocity = Vector3.zero;
    private float ticksRelativelyOnTarget = 0f;
    private float thresh = 5.0f;
    private float ticksFiring = 0f;

    private const float TARGET_WAIT = 5f;
    private const float SHOOT_WAIT = 500f;

    /*
    void OnDrawGizmos()
    {
        if (e == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(e, 1f);
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(target, 1f);
    }
    */

    public void Start()
    {
        enemyMeshRend.material = fullHealthMat;
        player = GameObject.FindWithTag("Player");
        playerTransform = player.transform;

        pf = GameObject.Find("Level Blocks").GetComponent<PathfinderV2>();

        playerHealth = player.GetComponent<PlayerHealth>();

        enemy = GetComponent<Enemy_V2>();

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    }

    public void Update()
    {
        //move target
        if (target == null) target = pf.player.transform.position + new Vector3(0f, 0.1f, 0f);
        else if(ticksFiring < SHOOT_WAIT)
        {
            target = Vector3.SmoothDamp(target, pf.player.transform.position + new Vector3(0f, 0.1f, 0f), ref swingVelocity, 0.15f);
        }

        //Calculate target line
        e = transform.position + new Vector3(0f, 1.9f, 0f);
        Vector3 d = (target - e);

        //Calculate LOS
        bool has_los;
        RaycastHit r;
        if (!Physics.Raycast(e + 0.05f * d, d, out r, 0.95f * Vector3.Magnitude(d))) 
        {
            has_los = true;
            lineRenderer.positionCount = 2;
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.01f;
            lineRenderer.SetPosition(0, e);
            lineRenderer.SetPosition(1, e + d);
        }
        else
        {
            has_los = false;
            lineRenderer.positionCount = 0;
        }

        //see if we're on-target
        if (has_los && Vector3.Magnitude(swingVelocity) < thresh)
        {
            ticksRelativelyOnTarget += 1;
        }
        else
        {
            ticksRelativelyOnTarget = 0;
            ticksFiring = 0;
        }

        //if we're on target, do some things
        if(ticksRelativelyOnTarget > TARGET_WAIT)
        {
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            ticksFiring += 1;
            if(ticksFiring >= SHOOT_WAIT)
            {
                lineRenderer.startColor = Color.cyan;
                lineRenderer.endColor = Color.cyan;
                if(ticksFiring == SHOOT_WAIT)
                {
                    //FIRE CODE GOES HERE
                    UnityEngine.Debug.Log("Fired!");
                    playerHealth.PlayerTakeDamage(5, d);
                }
                else if(ticksFiring > 2f * SHOOT_WAIT) {
                    ticksFiring = 0;
                }
            }
        }
        else
        {
            lineRenderer.startColor = Color.yellow;
            lineRenderer.endColor = Color.yellow;
            ticksFiring = 0;
        }


        //etc
        if (healthSpriteRend != null)
        {
            switch (currHealth)
            {
                case 25:
                    healthSpriteRend.sprite = twoFiveHealthSprite;
                    break;

                case 20:
                    healthSpriteRend.sprite = twentyHealthSprite;
                    break;

                case 15:
                    healthSpriteRend.sprite = oneFiveHealthSprite;
                    break;

                case 10:
                    healthSpriteRend.sprite = tenHealthSprite;
                    enemyMeshRend.material = orangeMat;
                    break;

                case 5:
                    healthSpriteRend.sprite = fiveHealthSprite;
                    enemyMeshRend.material = redMat;
                    break;

                case 0:
                    healthSpriteRend.sprite = zeroHealthSprite;
                    break;
            }
        }

        //knockback player if within grid cell radius from them
        float grid_diff = Vector3.Magnitude(pf.pts[0, 1] - pf.pts[0, 0]);
        float pos_diff = Vector3.Magnitude(player.transform.position - transform.position);

        /*
        if (pos_diff < 1.0f * grid_diff)
        {
            playerrgbd = player.GetComponent<Rigidbody>();
            Vector2 difference = playerrgbd.transform.position - transform.position;
            difference = difference.normalized * thrust;
            playerHealth.PlayerTakeDamage(5, Vector3.zero); //FIX
        }
        */

        if (pos_diff < .2f * grid_diff)
        {
            //return; //TODO - fix force calc
            playerrgbd = player.GetComponent<Rigidbody>();
            Vector3 difference = playerrgbd.transform.position - transform.position;
            difference = new Vector3(difference.x, 0f, difference.z);
            difference = difference.normalized;
            playerHealth.PlayerTakeDamage(5, difference);
        }
    }

    public void TakeDamage(int damage)
    {
        currHealth -= damage;

        if (currHealth <= 0)// && isEnemyDead == false)
        {
            playerHealth.playEnemyDie();

            Instantiate(healthDrop, transform.position + new Vector3(1, 1, 0), transform.rotation);

            //gameObject.GetComponent<Animator>().Play("DeathAnim");
            Destroy(gameObject);
            isEnemyDead = true;
            
            //currHealth = 25;
            //enemyMeshRend.material = fullHealthMat;
            //enemy.reset();



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

