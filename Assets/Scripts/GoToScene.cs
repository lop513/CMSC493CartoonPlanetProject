using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    // Upon contact with the player, sends them to the scene specified by the following name
    public string targetScene;

    // Start is called before the first frame update
    void Start()
    {
        // Make sure the object has a collider, and it's set as a trigger
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("No collider attached to the GoToScene object. Please add a collider component.");
        }
        else
        {
            collider.isTrigger = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Load the target scene
            SceneManager.LoadScene(targetScene);
        }
    }
}

