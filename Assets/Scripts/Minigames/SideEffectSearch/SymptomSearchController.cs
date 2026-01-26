using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;


/*
Still need to add:
- more characters to the list
- prolly more idk
*/

public class SymptomSearchController : MonoBehaviour
{
    public int stage = 1;

    [Header("Spawner Settings")]
    public List<GameObject> peopleToSpawn = new List<GameObject>(); //list to pull from to get the prefabs to spawn in 
    public List<GameObject> people = new List<GameObject>(); //list of people that have been spawned in
    public int gridX = 10; //width of the grid
    public int gridY = 10; //height of the grid
    public float spacing = 2f;
    public bool center = true;
    public Camera mainCam;
    private float sideEffectRate;

    [Header("Camera Positions")]
    public Vector3 camPos1 = new Vector3(1f, 4.5f, -8f);
    public Vector3 camPos2 = new Vector3(3.7f, 6f, -13f);
    public Vector3 camPos3 = new Vector3(0f, 5f, -9f);

    [Header("UI Settings")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI numSickText;
    private int numSickPeople;
    public int totalSickPeople;
    private float elapsedTime = 0.0f;

    //for adding people to spawned in list
    private GameObject personSpawned;

    void Awake()
    {
        //set the stage to be what the current player's clinical stage is
        // stage = PlayerManager.instance.current.clinicalStage;
        stage = 1;

        //increase it for next time
        // PlayerManager.instance.current.clinicalStage++;
    }
    // Start is called before the first frame update
    void Start()
    {
        switch (stage)
        {
            case 1:
                {
                    //set up the scene for the first level of the minigame
                    gridX = 3;
                    gridY = 4;
                    spacing = 2;
                    mainCam.transform.position = camPos1;
                    sideEffectRate = 50;
                    SpawnGrid();
                    break;
                }

            case 2:
                {
                    //set up the scene for the second level of the minigame
                    gridX = 5;
                    gridY = 10;
                    spacing = 1.5f;
                    mainCam.transform.position = camPos2;
                    sideEffectRate = 25;
                    SpawnGrid();
                    break;
                }

            case 3:
                {
                    //set up the scene for the third level of the minigame
                    gridX = 10;
                    gridY = 10;
                    spacing = 1;
                    mainCam.transform.position = camPos3;
                    sideEffectRate = 5;
                    SpawnGrid();
                    break;
                }
        }
    }
    void Update()
    {
        if (totalSickPeople > numSickPeople)
        {
            //start the timer
            elapsedTime += Time.deltaTime;

            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            int milliseconds = Mathf.FloorToInt(elapsedTime * 100f % 100f);

            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
        else
        {
            //end the game
            Time.timeScale = 0;
        }
    }

    //function for spawning in the first stage
    private void SpawnGrid()
    {
        Vector3 offset = Vector3.zero;

        if (center)
        {
            // Calculate offset to center the grid
            offset = new Vector3(
                -(gridX - 1) * spacing / 2f,
                0,
                -(gridY - 1) * spacing / 2f
            );
        }

        //spawn the grid of people
        for (int x = 0; x < gridY; x++)
        {
            for (int z = 0; z < gridX; z++)
            {
                //spawn each person with an offset
                Vector3 position = new Vector3(x * spacing, 0, z * spacing) + offset;
                personSpawned = Instantiate(peopleToSpawn[Random.Range(0, peopleToSpawn.Count - 1)], position, Quaternion.identity, transform);

                //make some of the people sick
                personSpawned.GetComponent<SymptomChecker>().CheckSideEffects(sideEffectRate);

                //add the spawned person to the list
                people.Add(personSpawned);
            }
        }
    }

    public void IncreaseScore()
    {
        //increase the number of sick people
        numSickPeople++;

        //update the UI
        numSickText.text = "x" + numSickPeople.ToString();
    }
}
