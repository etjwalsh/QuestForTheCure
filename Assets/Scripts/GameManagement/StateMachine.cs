using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateMachine : MonoBehaviour
{
    [SerializeField] public GameObject menuUI; //reference to the menu ui
    [SerializeField] public GameObject wheelUI; //reference to the wheel spinner ui
    [SerializeField] public GameObject settingsUI; //reference to the settings ui
    [SerializeField] public GameObject characterSelectUI; //reference to the char select screen ui
    [SerializeField] public GameObject lrUI; //reference to the left/right choice UI
    [SerializeField] public GameObject numPlayersUI; //reference to the selection screen for the number of players

    //for changing the space after a required space gets landed on
    public Material genericSpaceMat;

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
    public enum GameState { KickStart, MainMenu, Settings, NumCharsSelect, CharSelect, GameStart, Spinning, PlayerMoving, LRChoice, MinigameEnter, Minigame, TriviaEnter, Trivia, SceneChange, EndTurn }
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
            case GameState.SceneChange:
                {
                    SceneChange();
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

        //change scenes
        LevelLoader.instance.LoadScene("LoadDiscovery");
    }

    public void Spinning()
    {
        //start the player's turn
        PlayerManager.instance.StartTurn();

        //set reference to the current player's script
        lrUI.GetComponent<LeftRightChoice>().playerRef = PlayerManager.instance.players[PlayerManager.instance.currentPlayerIndex].characterPiece;

        //activate the wheel spinner UI
        wheelUI.SetActive(true);
        wheelUI.GetComponent<WheelSpin>().spinButton.interactable = true;
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
        // Debug.Log("yayyy minigame");
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


        //save all the player locations to the list
        PlayerManager.instance.playerLocations = new string[PlayerManager.instance.players.Count];

        for (int i = 0; i < PlayerManager.instance.players.Count; i++)
        {
            //save the locations of the players
            PlayerManager.instance.playerLocations[i] = PlayerManager.instance.playerPieces[i].GetComponent<Movement>().space.name;
        }

        //enter the trivia scene
        LevelLoader.instance.LoadScene("Trivia");

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
        currentState = GameState.Trivia;
    }

    public void Trivia()
    {
        wheelUI.SetActive(false);
    }

    public void SceneChange()
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
        int randIndex = Random.Range(0, roles.Count);
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
            return qs[Random.Range(0, qs.Count)];
        }
        return null;
    }
}
