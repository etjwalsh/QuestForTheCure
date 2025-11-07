using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Palmmedia.ReportGenerator.Core;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;
using Cinemachine;
using System;


public class GameStateMachine : MonoBehaviour
{
    [SerializeField] public GameObject menuUI; //reference to the menu ui
    [SerializeField] public GameObject wheelUI; //reference to the wheel spinner ui
    [SerializeField] public GameObject settingsUI; //reference to the settings ui
    [SerializeField] public GameObject characterSelectUI; //reference to the char select screen ui
    [SerializeField] public GameObject lrUI; //reference to the left/right choice UI
    [SerializeField] public GameObject numPlayersUI; //reference to the selection screen for the number of players
    [SerializeField] public Image fadeUI; //reference to the black fade in / fade out UI element
    public float fadeDuration = 1f;


    //for spawning players
    private Movement playerScript;

    //for the rotating roles
    public List<string> roles = new List<string> { "Patient", "Physician", "Community Advocate", "Research Coordinator", "Safety & Ethics", "Caregiver" };
    // private int maxRoles = 4;

    //for trivia
    public string currentPlayerRole;
    public QuestionTemplate currentQuestion;
    public string currentStage = "Discovery";

    //list and dictionary for storing trivia questions
    public List<QuestionTemplate> triviaQuesitons = new List<QuestionTemplate> { };
    private Dictionary<(string role, string stage), List<QuestionTemplate>> questionsDictionary;

    //singleton pattern
    private static GameStateMachine _instance;
    public static GameStateMachine instance
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

    //enum for state machine
    public enum GameState { KickStart, MainMenu, Settings, NumCharsSelect, CharSelect, GameStart, Spinning, PlayerMoving, LRChoice, MinigameEnter, Minigame, TriviaEnter, Trivia, EndTurn }
    public GameState currentState = GameState.KickStart; //for tracking current state

    private void Awake()
    {
        //initialize the dictionary
        questionsDictionary = new Dictionary<(string role, string stage), List<QuestionTemplate>>();

        //populate the dictionary
        foreach (QuestionTemplate qt in triviaQuesitons)
        {
            //check if that spot in the triviaQuestions list is not populated
            if (qt == null)
            {
                continue;
            }

            //set the key
            var key = (qt.questionRole, qt.questionStage);

            //make a list within the dictionary if it doesn't already exist
            if (!questionsDictionary.ContainsKey(key))
            {
                questionsDictionary[key] = new List<QuestionTemplate>();
            }

            //add this question to that list based on the key
            questionsDictionary[key].Add(qt);
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        //set instance of state machine and make sure one doesn't already exist
        if (instance != null)
        {
            Debug.LogWarning("warning: too many instances of game state machine");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("color of image = " + fadeUI.color);
        // Debug.Log(currentState);
        //state machine switch statement
        switch (currentState)
        {
            case GameState.KickStart:
                {
                    KickStart();
                    break;
                }
            case GameState.MainMenu:
                {
                    MainMenu();
                    break;
                }
            case GameState.Settings:
                {
                    Settings();
                    break;
                }
            case GameState.NumCharsSelect:
                {
                    NumCharsSelect();
                    break;
                }
            case GameState.CharSelect:
                {
                    CharSelect();
                    break;
                }
            case GameState.GameStart:
                {
                    GameStart();
                    break;
                }
            case GameState.Spinning:
                {
                    Spinning();
                    break;
                }
            case GameState.PlayerMoving:
                {
                    PlayerMoving();
                    break;
                }
            case GameState.LRChoice:
                {
                    LRChoice();
                    break;
                }
            case GameState.MinigameEnter:
                {
                    MinigameEnter();
                    break;
                }
            case GameState.Minigame:
                {
                    Minigame();
                    break;
                }
            case GameState.TriviaEnter:
                {
                    TriviaEnter();
                    break;
                }
            case GameState.Trivia:
                {
                    Trivia();
                    break;
                }
            case GameState.EndTurn:
                {
                    EndTurn();
                    break;
                }
        }
    }

    public void KickStart()
    {
        menuUI.SetActive(false);
        characterSelectUI.SetActive(false);
        wheelUI.SetActive(false);
        lrUI.SetActive(false);
        numPlayersUI.SetActive(false);

        //fade the screen in
        // StartCoroutine(FadeFromBlack());

        //change game state to main menu
        currentState = GameState.MainMenu;
    }

    public void MainMenu()
    {
        menuUI.SetActive(true);
    }

    public void Settings()
    {
        menuUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    public void NumCharsSelect()
    {
        //activate correct menus
        menuUI.SetActive(false);
        numPlayersUI.SetActive(true);
    }

    public void CharSelect()
    {
        numPlayersUI.SetActive(false);
        characterSelectUI.SetActive(true);
    }

    public void GameStart()
    {
        //set UI correctly
        characterSelectUI.SetActive(false);

        //set a function call for after the scene is loaded
        SceneManager.sceneLoaded += OnLoaded;

        //change scenes
        SceneManager.LoadScene("Sandbox");
    }

    //this method is just for use when going from the main menu to the sandbox scene
    public void OnLoaded(Scene scene, LoadSceneMode mode)
    {
        //make sure the correct scene loaded
        if (scene.name != "Sandbox")
        {
            return;
        }

        //remove this function call so it doesn't dupe next time
        SceneManager.sceneLoaded -= OnLoaded;

        //get how many players need to be spawned in
        int numPlayersToSpawn = PlayerManager.numPlayers;
        Debug.Log("number of players to spawn = " + numPlayersToSpawn);

        //spawn in characters based on the characters inside of the player list (access from PlayerManager)
        //add this later, for now just spawn in 4 of the same generic character

        //vars for the spawning
        float spacing = 2.0f;
        float startOffset = -(spacing * (PlayerManager.numPlayers - 1) / 2f);

        //for loop to spawn all the players
        for (int i = 0; i < PlayerManager.numPlayers; i++)
        {
            playerScript = PlayerManager.instance.players[i].characterPiece.GetComponent<Movement>();

            //offset along Z axis for spawning players
            Vector3 offset = new Vector3(0, 0, startOffset + (i * spacing));

            //locate the starting spot 
            playerScript.startingSpot = GameObject.Find("SpacesTree/StartingSpace");

            //spawn in a new player
            GameObject spawnedPlayer = Instantiate(PlayerManager.instance.players[i].characterPiece, playerScript.startingSpot.transform.position + offset + new Vector3(0, 0.05f, 0), playerScript.startingSpot.transform.rotation);

            //add this gameObject to the list of player pieces
            PlayerManager.instance.playerPieces.Add(spawnedPlayer);

            //assign the player a role
            spawnedPlayer.GetComponent<Movement>().currentRole = AssignRoleToPlayer();
            Debug.Log("player's role is now: " + spawnedPlayer.GetComponent<Movement>().currentRole);

            //set the priority of the spawned player's camera to 0
            Debug.Log("about to get the camera and set the priority to 0, and then print them both");
            var camera = spawnedPlayer.GetComponentInChildren<CinemachineVirtualCamera>();
            camera.Priority = 0;

            Debug.Log("camera = " + camera);
            Debug.Log("priority = " + camera.Priority);
        }

        //reset the list of roles
        roles = ResetRolesList(roles);

        currentState = GameState.Spinning; //will def need to change this later to include tutorial type stuff
    }

    public void Spinning()
    {
        //start the player's turn
        PlayerManager.instance.StartTurn();
        // Debug.Log("current player is = " + PlayerManager.instance.players[PlayerManager.instance.currentPlayerIndex].playerName);

        //set reference to the current player's script
        lrUI.GetComponent<LeftRightChoice>().playerRef = PlayerManager.instance.players[PlayerManager.instance.currentPlayerIndex].characterPiece;

        //activate the wheel spinner UI
        wheelUI.SetActive(true);
    }

    public void PlayerMoving()
    {
        lrUI.SetActive(false);
        wheelUI.SetActive(false);
    }

    public void LRChoice()
    {
        lrUI.SetActive(true);
    }

    public void MinigameEnter()
    {
        Debug.Log("yayyy minigame");
        currentState = GameState.Minigame;
    }

    public void Minigame()
    {
        //change eventually to enter a minigame
        currentState = GameState.EndTurn;
    }

    public void TriviaEnter()
    {
        //get the current player's role
        currentPlayerRole = PlayerManager.instance.current.currentRole;

        //get the trivia question based on the current stage
        currentQuestion = GetQuestion(currentPlayerRole, currentStage);

        Debug.Log("the selected question is now: " + currentQuestion);

        //transition

        //enter the trivia scene
        SceneManager.LoadScene("Trivia");

        //set the first trivia UI up
        if (currentQuestion.questionType == "TrueFalse")
        {
            Debug.Log("this is a true false question");
        }
        else if (currentQuestion.questionType == "MultipleChoice")
        {
            Debug.Log("this is a multiple choice question");
        }

        //change to trivia state
        // currentState = GameState.Trivia;
    }

    public void Trivia()
    {
        currentState = GameState.EndTurn;
    }

    public void EndTurn()
    {
        PlayerManager.instance.EndTurn(); //end the player's turn
    }

    public string AssignRoleToPlayer()
    {
        if (roles.Count == 0)
        {
            Debug.LogWarning("no more roles to pick from!");
        }

        //get a random role
        int randIndex = UnityEngine.Random.Range(0, roles.Count);
        string chosenRole = roles[randIndex];

        //remove that number from the list of possible numbers
        roles.RemoveAt(randIndex);

        //return the role that was taken from the roles array
        return chosenRole;
    }

    public List<string> ResetRolesList(List<string> list)
    {
        //reset the roles list
        list.Clear();

        //declare the roles list again to make sure they are there for next time
        list = new List<string> { "Patient", "Physician", "Community Advocate", "Research Coordinator", "Safety & Ethics", "Caregiver" };

        //return the list
        return list;
    }

    public QuestionTemplate GetQuestion(string role, string stage)
    {
        //make sure dictionary has been set up
        if (questionsDictionary == null)
        {
            Debug.LogError("Questions dictionary does not exist");
            return null;
        }

        List<QuestionTemplate> qs;
        
        //search the dictionary for the right question
        if (questionsDictionary.TryGetValue((role, stage), out qs))
        {
            return qs[UnityEngine.Random.Range(0, qs.Count)];
        }
        return null;
    }

    // public IEnumerator FadeToBlack() //this one fades FROM NORMAL TO BLACK
    // {
    //     Debug.Log("inside of fade to black");
    //     Color c = fadeUI.color;
    //     float elapsed = 0f;
    //     float startAlpha = c.a;

    //     while (elapsed < fadeDuration)
    //     {
    //         elapsed += Time.deltaTime;
    //         c.a = Mathf.Lerp(startAlpha, 1f, elapsed / fadeDuration); // gradually increase alpha
    //         fadeUI.color = c;
    //         yield return null;
    //     }

    //     //make sure it is fully black
    //     c.a = 1f;
    //     fadeUI.color = c;
    // }
    // public IEnumerator FadeFromBlack() //this one fades FROM BLACK TO NORMAL
    // {
    //     Debug.Log("inside of fade from black");
    //     Color c = fadeUI.color;
    //     float elapsed = 0f;
    //     float startAlpha = c.a;

    //     Debug.Log("Starting FadeFromBlack, alpha=" + fadeUI.color.a);
    //     // gradually reduce alpha of the black image
    //     while (elapsed < fadeDuration)
    //     {
    //         elapsed += Time.deltaTime;
    //         c.a = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
    //         fadeUI.color = c;

    //         Debug.Log("Loop: elapsed=" + elapsed + " alpha=" + fadeUI.color.a);

    //         yield return null;

    //     }
    //     Debug.Log("FadeFromBlack finished, alpha=" + fadeUI.color.a);

    //     //make sure it is fully transparent
    //     c.a = 0f;
    //     fadeUI.color = c;
    // }

    // --------------- for loading levels ---------------

    //this will load the next level in the unity build order
    // public void LoadNextLevel()
    // {
    //     StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    // }

    // //for transitioning between scenes
    // IEnumerator LoadLevel(int levelIndex)
    // {
    //     //trigger the crossfade to start
    //     transition.SetTrigger("start");

    //     //wait a sec
    //     yield return new WaitForSeconds(transitionTime);

    //     SceneManager.LoadScene(levelIndex);
    // }
}
