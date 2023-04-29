using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObjScript : MonoBehaviour
{
    float timeLeft = 6.0f;

    public GameObject obj;

    private bool check;

    // Start is called before the first frame update
    void Start()
    {
        check = false;
    }

    // Update is called once per frame
    void Update()
    {
        obj.SetActive(true);

        if (Input.GetButtonDown("Vertical") || Input.GetButtonDown("Horizontal"))
        {
            check = true;
        }
         
        if(check) { 

            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0)
            {
                obj.SetActive(false);
            }
        }
    }
}
