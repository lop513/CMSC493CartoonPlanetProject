using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class SatelliteScript : MonoBehaviour
{
    private GameObject player;

    public Material openMat;

    public Renderer satMeshRend;

    public bool activated;
    public bool started;

    private GameObject[] thumbsList;

    public GameObject thumbs1;
    public GameObject thumbs2;
    public GameObject thumbs3;
    public GameObject thumbs4;
    public GameObject thumbs5;
    public GameObject thumbs6;

    int timeLeft = 60;

    float timeLeft2 = 3.0f;

    public GameObject pressE;
    public GameObject beginTimer;
    public GameObject victCanvas;

    public GameObject rescueShip;

    private Animation rescueAnim;

    public bool victory;

    float test = 60.0f;

    public GameObject timerObj;

    public Image timerImage;

    public Sprite fiveNineSeconds;
    public Sprite fiveEightSeconds;
    public Sprite fiveSevenSeconds;
    public Sprite fiveSixSeconds;
    public Sprite fiveFiveSeconds;
    public Sprite fiveFourSeconds;
    public Sprite fiveThreeSeconds;
    public Sprite fiveTwoSeconds;
    public Sprite fiveOneSeconds;
    public Sprite fiveZeroSeconds;

    public Sprite fourNineSeconds;
    public Sprite fourEightSeconds;
    public Sprite fourSevenSeconds;
    public Sprite fourSixSeconds;
    public Sprite fourFiveSeconds;
    public Sprite fourFourSeconds;
    public Sprite fourThreeSeconds;
    public Sprite fourTwoSeconds;
    public Sprite fourOneSeconds;
    public Sprite fourZeroSeconds;

    public Sprite threeNineSeconds;
    public Sprite threeEightSeconds;
    public Sprite threeSevenSeconds;
    public Sprite threeSixSeconds;
    public Sprite threeFiveSeconds;
    public Sprite threeFourSeconds;
    public Sprite threeThreeSeconds;
    public Sprite threeTwoSeconds;
    public Sprite threeOneSeconds;
    public Sprite threeZeroSeconds;

    public Sprite twoNineSeconds;
    public Sprite twoEightSeconds;
    public Sprite twoSevenSeconds;
    public Sprite twoSixSeconds;
    public Sprite twoFiveSeconds;
    public Sprite twoFourSeconds;
    public Sprite twoThreeSeconds;
    public Sprite twoTwoSeconds;
    public Sprite twoOneSeconds;
    public Sprite twoZeroSeconds;

    public Sprite oneNineSeconds;
    public Sprite oneEightSeconds;
    public Sprite oneSevenSeconds;
    public Sprite oneSixSeconds;
    public Sprite oneFiveSeconds;
    public Sprite oneFourSeconds;
    public Sprite oneThreeSeconds;
    public Sprite oneTwoSeconds;
    public Sprite oneOneSeconds;
    public Sprite oneZeroSeconds;

    public Sprite nineSeconds;
    public Sprite eightSeconds;
    public Sprite sevenSeconds;
    public Sprite sixSeconds;
    public Sprite fiveSeconds;
    public Sprite fourSeconds;
    public Sprite threeSeconds;
    public Sprite twoSeconds;
    public Sprite oneSeconds;
    public Sprite zeroSeconds;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        activated = false;
        started = false;

        //rescueAnim = rescueShip.GetComponent<Animation>();

    }

    // Update is called once per frame
    void Update()
    {
        //timeLeft -= Time.deltaTime;

        float distance = Vector3.Distance(player.transform.position, this.transform.position);

        if (distance < 20)
        {
            pressE.SetActive(true);
        }

        else
        {
            pressE.SetActive(false);
        }

        if ((distance < 20) && Input.GetKeyDown("e"))
        {
            started = true;
            pressE.SetActive(false);
            GameObject.Find("ScuffedRadar").GetComponent<ScuffedRadar>().press_button();
            GameObject.Find("Badery Spawner").GetComponent<BaderySpawner>().stall = false;
        }

        if (started == true)
        {
            pressE.SetActive(false);

            // Start countdown
            beginTimer.SetActive(true);
            timerObj.SetActive(true);

            timeLeft2 -= Time.deltaTime;

            if (timeLeft2 <= 0)
            {
                beginTimer.SetActive(false);
            }

            thumbs1.SetActive(true);
            thumbs2.SetActive(true);
            thumbs3.SetActive(true);
            thumbs4.SetActive(true);
            thumbs5.SetActive(true);
            thumbs6.SetActive(true);

            test -= Time.deltaTime;
            timeLeft = Mathf.RoundToInt(test);
            
            if (timerImage != null) {
                switch (timeLeft)
                {
                    case 59:
                        timerImage.sprite = fiveNineSeconds;
                        break;

                    case 58:
                        timerImage.sprite = fiveEightSeconds;
                        break;

                    case 57:
                        timerImage.sprite = fiveSevenSeconds;
                        break;

                    case 56:
                        timerImage.sprite = fiveSixSeconds;
                        break;

                    case 55:
                        timerImage.sprite = fiveFiveSeconds;
                        break;

                    case 54:
                        timerImage.sprite = fiveFourSeconds;
                        break;

                    case 53:
                        timerImage.sprite = fiveThreeSeconds;
                        break;

                    case 52:
                        timerImage.sprite = fiveTwoSeconds;
                        break;

                    case 51:
                        timerImage.sprite = fiveOneSeconds;
                        break;

                    case 50:
                        timerImage.sprite = fiveZeroSeconds;
                        break;

                    case 49:
                        timerImage.sprite = fourNineSeconds;
                        break;

                    case 48:
                        timerImage.sprite = fourEightSeconds;
                        break;

                    case 47:
                        timerImage.sprite = fourSevenSeconds;
                        break;

                    case 46:
                        timerImage.sprite = fourSixSeconds;
                        break;

                    case 45:
                        timerImage.sprite = fourFiveSeconds;
                        break;

                    case 44:
                        timerImage.sprite = fourFourSeconds;
                        break;

                    case 43:
                        timerImage.sprite = fourThreeSeconds;
                        break;

                    case 42:
                        timerImage.sprite = fourTwoSeconds;
                        break;

                    case 41:
                        timerImage.sprite = fourOneSeconds;
                        break;

                    case 40:
                        timerImage.sprite = fourZeroSeconds;
                        break;

                    case 39:
                        timerImage.sprite = threeNineSeconds;
                        break;

                    case 38:
                        timerImage.sprite = threeEightSeconds;
                        break;

                    case 37:
                        timerImage.sprite = threeSevenSeconds;
                        break;

                    case 36:
                        timerImage.sprite = threeSixSeconds;
                        break;

                    case 35:
                        timerImage.sprite = threeFiveSeconds;
                        break;

                    case 34:
                        timerImage.sprite = threeFourSeconds;
                        break;

                    case 33:
                        timerImage.sprite = threeThreeSeconds;
                        break;

                    case 32:
                        timerImage.sprite = threeTwoSeconds;
                        break;

                    case 31:
                        timerImage.sprite = threeOneSeconds;
                        break;

                    case 30:
                        timerImage.sprite = threeZeroSeconds;
                        break;

                    case 29:
                        timerImage.sprite = twoNineSeconds;
                        break;

                    case 28:
                        timerImage.sprite = twoEightSeconds;
                        break;

                    case 27:
                        timerImage.sprite = twoSevenSeconds;
                        break;

                    case 26:
                        timerImage.sprite = twoSixSeconds;
                        break;

                    case 25:
                        timerImage.sprite = twoFiveSeconds;
                        break;

                    case 24:
                        timerImage.sprite = twoFourSeconds;
                        break;

                    case 23:
                        timerImage.sprite = twoThreeSeconds;
                        break;

                    case 22:
                        timerImage.sprite = twoTwoSeconds;
                        break;

                    case 21:
                        timerImage.sprite = twoOneSeconds;
                        break;

                    case 20:
                        timerImage.sprite = twoZeroSeconds;
                        break;

                    case 19:
                        timerImage.sprite = oneNineSeconds;
                        break;

                    case 18:
                        timerImage.sprite = oneEightSeconds;
                        break;

                    case 17:
                        timerImage.sprite = oneSevenSeconds;
                        break;

                    case 16:
                        timerImage.sprite = oneSixSeconds;
                        break;

                    case 15:
                        timerImage.sprite = oneFiveSeconds;
                        break;

                    case 14:
                        timerImage.sprite = oneFourSeconds;
                        break;

                    case 13:
                        timerImage.sprite = oneThreeSeconds;
                        break;

                    case 12:
                        timerImage.sprite = oneTwoSeconds;
                        break;

                    case 11:
                        timerImage.sprite = oneOneSeconds;
                        break;

                    case 10:
                        timerImage.sprite = oneZeroSeconds;
                        break;

                    case 9:
                        timerImage.sprite = nineSeconds;
                        break;

                    case 8:
                        timerImage.sprite = eightSeconds;
                        break;

                    case 7:
                        timerImage.sprite = sevenSeconds;
                        break;

                    case 6:
                        timerImage.sprite = sixSeconds;
                        break;

                    case 5:
                        timerImage.sprite = fiveSeconds;
                        break;

                    case 4:
                        timerImage.sprite = fourSeconds;
                        break;

                    case 3:
                        timerImage.sprite = threeSeconds;
                        break;

                    case 2:
                        timerImage.sprite = twoSeconds;
                        break;

                    case 1:
                        timerImage.sprite = oneSeconds;
                        break;

                    case 0:
                        timerImage.sprite = zeroSeconds;
                        break;
                }
            }
            if (timeLeft < 0)
            {
                activated = true;
                victCanvas.SetActive(true);
                timerObj.SetActive(false);

                GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
                List<GameObject> gameObjectsWithMatchingName = new List<GameObject>();
                foreach (GameObject obj in allGameObjects)
                {
                    if (obj.name.Contains("ThumbsV2"))
                    {
                        obj.transform.position = new Vector3(-999, -999, -999);
                    }
                }
            }
        }


        if (activated == true) 
        {
            // Stop spawns
            pressE.SetActive(false);
            satMeshRend.material = openMat;

            rescueShip.SetActive(true);

            // Player wins game
            victory = true;
        }
    }
}
