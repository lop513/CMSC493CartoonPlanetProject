using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 125;
    public int playerHealth;
    public float thrust = 20;

    private Transform enemyTransform;
    public GameObject enemy;
    private Rigidbody enemyrgbd;

    //public bool enemyKnockedBack;

    void Start()
    {
        playerHealth = maxHealth;

        enemy = GameObject.FindWithTag("Enemy");
        //enemyTransform = enemy.transform;
    }

    void Update()
    {
        //enemyKnockedBack = false;
        if(playerHealth <= 0)
        {
            gameObject.GetComponent<PlayerMovement>().acceleration = 0;
            gameObject.GetComponent<PlayerMovement>().jumpVelocity = 0;
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        playerHealth -= damage;
    }
    
    void OnCollisionEnter(Collision coll)
    {
        Enemy enemy = coll.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            enemyrgbd = enemy.GetComponent<Rigidbody>();
            Vector2 difference = enemyrgbd.transform.position - transform.position;
            difference = difference.normalized * thrust;
            enemyrgbd.AddForce(difference, ForceMode.Impulse);
            //enemyKnockedBack = true;
            UnityEngine.Debug.Log("Check");
            PlayerTakeDamage(25);
        }
        
    }
    
}
