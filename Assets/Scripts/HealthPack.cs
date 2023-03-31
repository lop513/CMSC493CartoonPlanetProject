using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public static bool hasPickedUpHealthPack = false;
    private bool hasEnteredTrigger = false;

    void Update()
    {
        if(hasEnteredTrigger)
        {
            hasPickedUpHealthPack = true;
            hasEnteredTrigger = false;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().CompareTag("Player"))
        {
            hasEnteredTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Collider>().CompareTag("Player"))
        {
            hasPickedUpHealthPack = false;
            hasEnteredTrigger = false;
        }
    }
}
