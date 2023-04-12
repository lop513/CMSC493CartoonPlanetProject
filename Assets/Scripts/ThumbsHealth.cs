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

    public void Start()
    {
        enemyMeshRend.material = fullHealthMat;
        player = GameObject.FindWithTag("Player");
        playerTransform = player.transform;

        pf = GameObject.Find("Level Blocks").GetComponent<PathfinderV2>();

        playerHealth = player.GetComponent<PlayerHealth>();

        enemy = GetComponent<Enemy_V2>();
    }

    public void Update()
    {
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

