using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 125;
    public int playerHealth;

    void Start()
    {
        playerHealth = maxHealth;
    }

    void Update()
    {
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
            PlayerTakeDamage(25);
        }
    }
    
}
