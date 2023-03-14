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

    //Material enemyMat;

    public Renderer enemyMeshRend;

    public void Start()
    {
        //enemyMat = GetComponent<MeshRenderer>().material;
        enemyMeshRend.material = fullHealthMat;
        player = GameObject.FindWithTag("Player");
        playerTransform = player.transform;
        //enemyMat = fullHealthMat;
    }

    public void Update()
    {
        if (currHealth == 10 && isEnemyDead == false)
        {
            //enemyMat = orangeMat;
            enemyMeshRend.material = orangeMat;

        }

        if (currHealth == 5 && isEnemyDead == false)
        {
            //enemyMat = redMat;
            enemyMeshRend.material = redMat;
        }

        

    }

    public void TakeDamage(int damage)
    {
        currHealth -= damage;

        if(currHealth <= 0 && isEnemyDead == false)
        {
            UnityEngine.Debug.Log("Dead: " + currHealth);
            //gameObject.GetComponent<Animator>().Play("DeathAnim");
            Destroy(gameObject);
            isEnemyDead = true;
        }

    }

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
}

