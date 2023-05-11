using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutdoorBarrierCheck : MonoBehaviour
{
    private int outdoorKills;

    public GameObject playerHealthScript;

    private GameObject barrier;
    public Renderer exitMeshRend;
    public Material purpleMat;

    private GameObject[] enemies;

    private GameObject player;
    public GameObject doorOpen;
    public GameObject remainingEnemy;

    private GUIStyle labelStyle;

    // Start is called before the first frame update
    void Start()
    {
        outdoorKills = 0;
        barrier = GameObject.Find("FakeBarrier");
    }

    public void make_outdoor_kill()
    {
        outdoorKills += 1;
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // Spawn enemies until then, need to set to how many enemies we want to kill
        if (outdoorKills >= 5)
        {
           
            doorOpen.SetActive(true);
            barrier.SetActive(false);
            exitMeshRend.material = purpleMat;

            //trigger door open on radar
            GameObject.Find("ScuffedRadar").GetComponent<ScuffedRadar>().press_button();
        }
        
    }

    void OnGUI()
    {
        labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.fontSize = 50; 
        labelStyle.normal.textColor = Color.yellow;
        string text = string.Format("Snipers killed: {0} out of 5", outdoorKills);
        GUI.Label(new Rect(668, 100, 600, 300), text, labelStyle);
    }
}
