using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CockpitDoor : MonoBehaviour
{
    private GameObject player;
    private bool lockCheck;

    public GameObject lockScript;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        lockCheck = lockScript.GetComponent<LockedScript>().unlocked;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player && lockCheck == true)
        {
            SceneManager.LoadScene("StarshipCargoBay");
        }
    }

}
