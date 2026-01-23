using System.Collections.Generic;
using UnityEngine;


/*
Still need to add:
- random assignment of sick vs healthy
- more characters to the list
- removing the sick people when clicked
- counter to keep track of sick people
- timer counting up to keep track of length of minigame
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

    [Header("Camera Positions")]
    public Vector3 camPos1 = new Vector3(0.5f, 4.5f, -8f);
    public Vector3 camPos2 = new Vector3(3.7f, 6f, -13f);
    public Vector3 camPos3 = new Vector3(0f, 5f, -9f);

    //for adding people to spawned in list
    private GameObject personSpawned;

    void Awake()
    {
        //set the stage to be what the current player's clinical stage is
        // stage = PlayerManager.instance.current.clinicalStage;
        stage = 3;

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
                    SpawnGrid();
                    break;
                }

            case 2:
                {
                    //set up the scene for the second level of the minigame
                    gridX = 5;
                    gridY = 10;
                    spacing = 1.5f;
                    mainCam.transform.position = camPos1;
                    SpawnGrid();
                    break;
                }

            case 3:
                {
                    //set up the scene for the third level of the minigame
                    gridX = 10;
                    gridY = 10;
                    spacing = 1;
                    SpawnGrid();
                    mainCam.transform.position = camPos1;
                    break;
                }
        }
    }
    void Update()
    {

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

        for (int x = 0; x < gridY; x++)
        {
            for (int z = 0; z < gridX; z++)
            {
                //spawn each person with an offset
                Vector3 position = new Vector3(x * spacing, 0, z * spacing) + offset;
                personSpawned = Instantiate(peopleToSpawn[Random.Range(0, peopleToSpawn.Count - 1)], position, Quaternion.identity, transform);

                //add the spawned person to the list
                people.Add(personSpawned);

                //assign a sick or not sick value to the person spawned
                SymptomChecker sc = personSpawned.GetComponent<SymptomChecker>();
                //if it exists
                if (sc)
                {
                    //set isSick based on a 20% chance
                    sc.isSick = Random.value < 0.2;
                }
                else
                {
                    Debug.LogError("There is no symptom checker on this game object");
                }
            }
        }
    }
}
