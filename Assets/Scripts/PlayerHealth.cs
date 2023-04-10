using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 125;
    public int playerHealth;

    private Transform enemyTransform;
    public GameObject enemy;
    private Rigidbody enemyrgbd;

    public float lastHitTime = -1.0f;
    public float INVUL_TIME = 0.66f;
    public float KNOCKBACK_FORCE = 4f;

    public int kills;

    //public bool enemyKnockedBack;
    public AudioClip hit, die, enemy_hit, enemy_die;

    private AudioSource speaker;

    private int healthPackValue = 20;

    public Image healthImg;

    public Sprite fiftyHealthSprite;
    public Sprite fourFiveHealthSprite;
    public Sprite fourtyHealthSprite;
    public Sprite threeFiveHealthSprite;
    public Sprite thirtyHealthSprite;
    public Sprite twoFiveHealthSprite;
    public Sprite twentyHealthSprite;
    public Sprite oneFiveHealthSprite;
    public Sprite tenHealthSprite;
    public Sprite fiveHealthSprite;
    public Sprite zeroHealthSprite;

    void Start()
    {
        playerHealth = maxHealth;

        enemy = GameObject.FindWithTag("Enemy");

        speaker = GetComponent<AudioSource>();

        kills = 0;
    
    }

    void Update()
    {
        if (healthImg != null)
        {
            switch (playerHealth)
            {
                case 50:
                    healthImg.sprite = fiftyHealthSprite;
                    break;

                case 45:
                    healthImg.sprite = fourFiveHealthSprite;
                    
                    break;

                case 40:
                    healthImg.sprite = fourtyHealthSprite;
                    break;

                case 35:
                    healthImg.sprite = threeFiveHealthSprite;
                    break;

                case 30:
                    healthImg.sprite = thirtyHealthSprite;
                    break;

                case 25:
                    healthImg.sprite = twoFiveHealthSprite;
                    break;

                case 20:
                    healthImg.sprite = twentyHealthSprite;
                    break;

                case 15:
                    healthImg.sprite = oneFiveHealthSprite;
                    break;

                case 10:
                    healthImg.sprite = tenHealthSprite;
                    break;

                case 5:
                    healthImg.sprite = fiveHealthSprite;
                    break;

                case 0:
                    healthImg.sprite = zeroHealthSprite;
                    break;

            }
        }
        if (playerHealth <= 0)
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

    public void PlayerTakeDamage(int damage, Vector3 fdir)
    {
        if(Time.time - INVUL_TIME > lastHitTime)
        {
            lastHitTime = Time.time;
            playerHealth -= damage;
            playerHealth = Mathf.Max(playerHealth, 0);

            //knockback TODO - fix when directly atop enemy
            GetComponent<Rigidbody>().AddForce(fdir * KNOCKBACK_FORCE, ForceMode.Impulse);

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
