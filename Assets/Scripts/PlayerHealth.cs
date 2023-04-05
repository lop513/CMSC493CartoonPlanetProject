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

    public float lastHitTime = -1.0f;
    public const float INVUL_TIME = 0.66f;

    public int kills = 0;

    //public bool enemyKnockedBack;
    public AudioClip hit, die, enemy_hit, enemy_die;

    private AudioSource speaker;

    private int healthPackValue = 20;

    void Start()
    {
        playerHealth = maxHealth;

        enemy = GameObject.FindWithTag("Enemy");
        //enemyTransform = enemy.transform;

        speaker = GetComponent<AudioSource>();
    }

    void Update()
    {
        //enemyKnockedBack = false;
        if(playerHealth <= 0)
        {
            gameObject.GetComponent<PlayerMovement>().acceleration = 0;
            gameObject.GetComponent<PlayerMovement>().jumpVelocity = 0;
            speaker.PlayOneShot(die);
        }

        if(HealthPack.hasPickedUpHealthPack)
        {
            playerHealth = playerHealth + healthPackValue;
            HealthPack.hasPickedUpHealthPack = false;

            if(playerHealth > maxHealth)
            {
                playerHealth = maxHealth;
            }
        }
    }

    public void PlayerTakeDamage(int damage)
    {
        if(Time.time - INVUL_TIME > lastHitTime)
        {
            lastHitTime = Time.time;
            playerHealth -= damage;
            playerHealth = Mathf.Max(playerHealth, 0);

            speaker.PlayOneShot(hit);
        }
    }

    public void playEnemyHit()
    {
        Invoke("playEnemyHit_", 0.1f);
    }

    private void playEnemyHit_()
    {
        speaker.PlayOneShot(enemy_hit);
        
    }

    public void playEnemyDie()
    {
        kills += 1;
        Invoke("playEnemyDie_", 0.1f);
    }

    private void playEnemyDie_()
    {
        speaker.PlayOneShot(enemy_die);
    }
    /*
    void OnGUI()
    {
        GUI.Label(new Rect(100, 10, 300, 300), "Kill 25 Enemies to win!");


        GUI.Label(new Rect(10, 10, 200, 60), "Health: " + playerHealth);
        GUI.Label(new Rect(10, 20, 200, 60), "Kills: " + kills);

        if (playerHealth == 0)
        {
            GUI.Label(new Rect(10, 75, 200, 50), "YOU HAVE DIED!!");
        }
        else if(kills == 25)
        {
            playerHealth = 9999;
            GUI.Label(new Rect(10, 75, 200, 50), "You're Winner!!");
        }
    }
    */
        /*
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
        */
    }
