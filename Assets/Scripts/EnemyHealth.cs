using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class EnemyHealth : MonoBehaviour
{
    public int currHealth = 15;
    public static bool isEnemyDead = false;
    public Material fullHealthMat;
    public Material orangeMat;
    public Material redMat;
    
    //Material enemyMat;

    public Renderer enemyMeshRend;

    public void Start()
    {
        //enemyMat = GetComponent<MeshRenderer>().material;
        enemyMeshRend.material = fullHealthMat;
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
}
