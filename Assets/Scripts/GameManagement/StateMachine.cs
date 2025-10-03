using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Palmmedia.ReportGenerator.Core;
using UnityEngine.TextCore.Text;


public class GameStateMachine : MonoBehaviour
{
    [SerializeField] public GameObject menuUI; //reference to the menu ui
    [SerializeField] public GameObject wheelUI; //reference to the wheel spinner ui
    [SerializeField] public GameObject settingsUI; //reference to the settings ui
    [SerializeField] public GameObject characterSelectUI; //reference to the char select screen ui
    [SerializeField] public GameObject lrUI; //reference to the left/right choice UI

    public static int numPlayers;
    public static string playerOneCharacter = "";
    public static string playerTwoCharacter = "";
    public static string playerThreeCharacter = "";
    public static string playerFourCharacter = "";

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
    public enum GameState { KickStart, MainMenu, Settings, CharSelect, GameStart, Spinning, PlayerMoving, LRChoice, MinigameEnter, Minigame, TriviaEnter, Trivia }
    public GameState currentState = GameState.KickStart; //for tracking current state

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
        }
    }

    public void KickStart()
    {
        Debug.Log("set menu false");
        menuUI.SetActive(false);
        Debug.Log("set char select false");
        characterSelectUI.SetActive(false);
        Debug.Log("set wheel false");
        wheelUI.SetActive(false);
        Debug.Log("set l/r false");
        lrUI.SetActive(false);

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

    public void CharSelect()
    {
        menuUI.SetActive(false);
        characterSelectUI.SetActive(true);
    }

    public void GameStart()
    {
        characterSelectUI.SetActive(false);
        currentState = GameState.Spinning; //will def need to change this later to include tutorial type stuff
    }

    public void Spinning()
    {
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

    }

    public void TriviaEnter()
    {
        Debug.Log("yayyy trivia");
        currentState = GameState.Trivia;
    }

    public void Trivia()
    {

    }
}
