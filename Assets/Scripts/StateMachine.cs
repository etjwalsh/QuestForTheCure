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
    [SerializeField] protected GameObject menuUI; //reference to the menu ui
    [SerializeField] protected GameObject wheelUI; //reference to the wheel spinner ui
    [SerializeField] protected GameObject settingsUI; //reference to the settings ui
    [SerializeField] protected GameObject characterSelectUI; //reference to the char select screen ui
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
    public enum GameState { KickStart, MainMenu, Settings, CharSelect, GameStart, Spinning, MinigameEnter, Minigame, TriviaEnter, Trivia }
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
        menuUI.SetActive(true);
        characterSelectUI.SetActive(false);
        currentState = GameState.MainMenu;
    }
    public void MainMenu()
    {

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
    }
    public void Spinning()
    {
        wheelUI.SetActive(true);
    }
    public void MinigameEnter()
    {

    }
    public void Minigame()
    {

    }
    public void TriviaEnter()
    {

    }
    public void Trivia()
    {

    }
}
