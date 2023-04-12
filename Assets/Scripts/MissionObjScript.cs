using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObjScript : MonoBehaviour
{
    float timeLeft = 6.0f;

    public GameObject obj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        obj.SetActive(true);

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            obj.SetActive(false);
        }
    }
}
