using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class SymptomSearchController : MonoBehaviour
{
    private int stage;
    private bool gameEnded = false;

    [Header("Spawner Settings")]
    public List<GameObject> peopleToSpawn = new List<GameObject>(); //list to pull from to get the prefabs to spawn in 
    public List<GameObject> people = new List<GameObject>(); //list of people that have been spawned in
    public int gridX = 10; //width of the grid
    public int gridY = 10; //height of the grid
    public float spacing = 2f;
    public bool center = true;
    public Camera mainCam;
    private float sideEffectRate;
    private int wave = 1;
    private int numWaves = 3;

    [Header("Camera Positions")]
    public Vector3 camPos1 = new Vector3(1f, 4.5f, -8f);
    public Vector3 camPos2 = new Vector3(3.7f, 6f, -13f);
    public Vector3 camPos3 = new Vector3(0f, 5f, -9f);

    [Header("UI Settings")]
    public GameObject mainUI;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI numSickText;
    private int numSickPeople;
    public int everySickPeople;
    public int totalSickPeople;
    private float elapsedTime = 0.0f;
    private float finalTime;
    private int totalNumPeople = 0;

    [Header("End Screen Settings")]
    public CanvasGroup endUI;
    public EndScreenUI endUIScript;
    public List<GameObject> textToShow;
    public GameObject timeUpText;
    public float duration;
    public TextMeshProUGUI timeResult;
    public TextMeshProUGUI sickResult;
    public TextMeshProUGUI effectivenessResult;
    public TextMeshProUGUI message;
    public GameObject exitButton;


    //for adding people to spawned in list
    private GameObject personSpawned;

    void Awake()
    {
        //set the end screen to inactive
        for (int i = 0; i < textToShow.Count; i++)
        {
            textToShow[i].SetActive(false);
        }
        timeUpText.SetActive(false);
        endUI.alpha = 0f;
        endUI.interactable = false;
        endUI.blocksRaycasts = false;
        message.gameObject.SetActive(false);

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
                    spacing = 1.25f;
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

            timerText.text = "Group: " + wave + "\n" + string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
        else if (wave != numWaves && totalSickPeople == numSickPeople)
        {
            //reset for another wave
            if (totalSickPeople == numSickPeople)
            {
                //reset number of sick people and update ui
                totalSickPeople = 0;
                numSickPeople = 0;
                numSickText.text = "x" + numSickPeople.ToString();

                //increase wave number
                wave++;

                //delete all current people
                for (int i = 0; i < people.Count; i++)
                {
                    //destroy every game object in the list
                    Destroy(people[i].gameObject);
                }

                //clear the list
                people.Clear();

                //spawn new people
                SpawnGrid();
            }
        }
        else if (wave == numWaves)
        {
            if (!gameEnded)
            {
                gameEnded = true;
                //end the game
                finalTime = elapsedTime;
                mainUI.SetActive(false);
                EndMinigame();
            }
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
                totalNumPeople++;

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

    private void EndMinigame()
    {
        StartCoroutine(ShowResults());
    }

    public IEnumerator ShowResults()
    {
        //add the scores to the textToShow (time first)
        float time = finalTime;
        //get the effectiveness score
        float effectivenessScore = (int)(100 - ((float)everySickPeople / totalNumPeople * 100f));

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int hundredths = Mathf.FloorToInt(time * 100f % 100f);

        timeResult.text = $"{minutes:00}:{seconds:00}.{hundredths:00}";

        // timeResult.text = finalTime.ToString();
        sickResult.text = everySickPeople.ToString() + " / " + totalNumPeople;

        //calculate effectiveness based on sick result and total people
        effectivenessResult.text = effectivenessScore.ToString() + "%";

        //change the color of the score depending on how good it is
        //change the ending message based on the score
        if (effectivenessScore <= 50)
        {
            effectivenessResult.color = Hex("#D2082E");
            message.text = "This treatment was not very effective...";
        }
        else if (effectivenessScore > 50 && effectivenessScore < 65)
        {
            effectivenessResult.color = Hex("#FFB07C");
            message.text = "Time to take this treatment back to the lab...";
        }
        else if (effectivenessScore >= 65 && effectivenessScore <= 90)
        {
            effectivenessResult.color = Hex("#FFC526");
            message.text = "We're getting somewhere but that score could still be improved!";
        }
        else if (effectivenessScore > 90 && effectivenessScore <= 95)
        {
            effectivenessResult.color = Hex("#45A682");
            message.text = "Wow this treatment is pretty good!";
        }
        else if (effectivenessScore > 95)
        {
            effectivenessResult.color = Hex("#90D5FF");
            message.text = "This treatment is super effective!!";
        }

        //add those texts to the list of stuff to show
        textToShow.Add(timeResult.gameObject);
        textToShow.Add(sickResult.gameObject);
        textToShow.Add(effectivenessResult.gameObject);

        //make sure they are hidden to start
        timeResult.gameObject.SetActive(false);
        sickResult.gameObject.SetActive(false);
        effectivenessResult.gameObject.SetActive(false);

        //tell the player their time is up
        timeUpText.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        timeUpText.SetActive(false);

        //vars for fading the end UI in
        float elapsedTime = 0f;
        endUI.alpha = 0f; //Start fully transparent
        endUI.blocksRaycasts = true;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime; //Use unscaledDeltaTime so it works even when Time.timeScale = 0
            endUI.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            yield return null;
        }

        //wait a sec
        yield return new WaitForSeconds(0.5f);

        //loop through all the texts that are needed to show
        for (int i = 0; i < textToShow.Count; i++)
        {
            textToShow[i].SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

        //activate the message
        message.gameObject.SetActive(true);

        //set the UI to be interactable
        endUI.interactable = true;

        //activate the exit button
        exitButton.SetActive(true);

        yield return new WaitForSeconds(1.0f);
    }

    Color Hex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color c);
        return c;
    }
}
