using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoleculeSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public float spawnRate;
    public float spawnRate1;
    public float spawnRate2;
    public float spawnRate3;
    public float duration;

    [Header("Element Prefabs")]
    public List<GameObject> elements;

    [Header("Trashed Element Counters")]
    public int diamondsTrashed = 0;
    public int crossesTrashed = 0;
    public int horizTrashed = 0;
    public int vertsTrashed = 0;
    public int happyTrashed = 0;
    public int sadTrashed = 0;
    private List<int> trashStats;

    [Header("UI")]
    public TextMeshProUGUI timer;
    public CanvasGroup gameOverScreen;
    public List<GameObject> imagesToShow;
    public List<GameObject> textsToShow;
    public List<GameObject> trashToShow;
    public List<GameObject> trashToTell;
    public GameObject exitButton;

    [Header("Tubes")]
    public List<Tube> tubes;

    //singleton pattern
    private static MoleculeSpawner _instance;
    public static MoleculeSpawner instance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    void Awake()
    {
        //set up the end UI to be invisible
        gameOverScreen.alpha = 0f;
        gameOverScreen.interactable = false;
        gameOverScreen.blocksRaycasts = false;
        exitButton.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        //set instance of state machine and make sure one doesn't already exist
        if (instance != null)
        {
            Debug.LogWarning("warning: too many instances of moleculespawner");
            Destroy(gameObject);
            return;
        }
        instance = this;

        StartCoroutine(SpawnElement());
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator SpawnElement()
    {
        float startTime = Time.time;
        //reference to camera for spawn position
        Camera cam = Camera.main;
        float height = cam.orthographicSize * 2;
        bool oneFourth = false;
        bool oneHalf = false;
        bool threeFourths = false;

        while (Time.time - startTime < duration)
        {
            //decrease the spawn rate as time goes on
            if (Time.time - startTime > (duration * 0.75) && !oneFourth) //if time is 25% completed
            {
                spawnRate = spawnRate3;
                oneFourth = true;
            }
            if (Time.time - startTime > (duration * 0.50) && !oneHalf) //if time is 50% completed
            {
                spawnRate = spawnRate2;
                oneHalf = true;
            }
            if (Time.time - startTime > (duration * 0.25) && !threeFourths) //if time is 75% completed
            {
                spawnRate = spawnRate2;
                threeFourths = true;
            }

            //wait for spawn rate seconds
            yield return new WaitForSeconds(spawnRate);

            //get random index in the list of elements to spawn
            int spawnIndex = Random.Range(0, elements.Count);

            //spawn that one just above the camera's bounds
            float camX = cam.transform.position.x;
            Vector3 spawnPos = new Vector3(Random.Range(camX - 9f, camX + 5.5f), height - 0.5f, -1);
            Debug.Log($"Camera X: {camX}, Spawning at: {spawnPos}");
            GameObject newElement = Instantiate(elements[spawnIndex], spawnPos, Quaternion.identity);

            //lock rotation
            newElement.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

            //change how fast they fall at random
            newElement.GetComponent<Rigidbody>().drag = Random.Range(6, 10);

            yield return null;
        }
    }
    private IEnumerator UpdateTimer()
    {
        float timeRemaining = duration;

        while (timeRemaining > 0)
        {
            // Update the text
            int seconds = Mathf.FloorToInt(timeRemaining);
            timer.text = seconds.ToString();

            if (seconds <= 10 && seconds % 2 == 0)
            {
                timer.color = Color.red;
            }
            else
            {
                timer.color = Color.black;
            }

            yield return new WaitForSeconds(1f);
            timeRemaining -= 1f;
        }

        // Timer finished
        Time.timeScale = 0f;
        timer.color = Color.black;
        timer.text = "Time's up!";

        //wait a second
        yield return new WaitForSecondsRealtime(2.0f);

        //change to the end screen UI
        StartCoroutine(ShowScores(gameOverScreen, 2.0f, 0.5f));
    }
    private IEnumerator ShowScores(CanvasGroup canvasGroup, float duration, float delayBetween)
    {
        //get the trash stats
        trashStats = new List<int> { sadTrashed, diamondsTrashed, vertsTrashed, crossesTrashed, horizTrashed, happyTrashed };

        //Hide all first
        for (int i = 0; i < imagesToShow.Count; i++)
        {
            //set the scores correctly
            textsToShow[i].GetComponent<TextMeshProUGUI>().text = "x" + tubes[i].score.ToString();

            //set the UI inactive
            imagesToShow[i].SetActive(false);
            textsToShow[i].SetActive(false);
        }
        //set the trash stats up too
        for (int i = 0; i < trashToShow.Count; i++)
        {
            //set the trash scores correctly
            trashToTell[i].GetComponent<TextMeshProUGUI>().text = "x" + trashStats[i].ToString();

            //make them green or red depending on what it should be, skipping the sad faces
            if (trashStats[i] == 0 && i > 0)
            {
                trashToTell[i].GetComponent<TextMeshProUGUI>().color = Color.green;
            }
            else if (i > 0)
            {
                trashToTell[i].GetComponent<TextMeshProUGUI>().color = Color.red;
            }

            //set the UI inactive
            trashToShow[i].SetActive(false);
            trashToTell[i].SetActive(false);
        }

        float elapsedTime = 0f;
        canvasGroup.alpha = 0f; //Start fully transparent

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime; //Use unscaledDeltaTime so it works even when Time.timeScale = 0
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f; //Ensure it's fully visible
        gameOverScreen.interactable = true; //enable the ui
        gameOverScreen.blocksRaycasts = true; //enable the ui

        //Show pairs one at a time
        for (int i = 0; i < imagesToShow.Count; i++)
        {
            imagesToShow[i].SetActive(true);
            textsToShow[i].SetActive(true);
            yield return new WaitForSecondsRealtime(delayBetween);
        }

        //wait between showing collected and trash too
        yield return new WaitForSecondsRealtime(delayBetween);

        //Show trash one at a time
        for (int i = 0; i < trashToShow.Count; i++)
        {
            trashToShow[i].SetActive(true);
            trashToTell[i].SetActive(true);
            yield return new WaitForSecondsRealtime(delayBetween);
        }

        //spawn in the exit button
        exitButton.SetActive(true);
    }

    public void OnDoneClicked()
    {
        //change scenes
        StartCoroutine(PlayerManager.instance.LoadPlayerLocations(LevelLoader.instance.previousScene));
    }
}
