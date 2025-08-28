using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Palmmedia.ReportGenerator.Core;


public class GameStateMachine : MonoBehaviour
{
    [SerializeField] protected GameObject menuUI; //reference to the menu ui
    [SerializeField] protected GameObject wheelUI; //reference to the wheel spinner ui
    [SerializeField] protected GameObject settingsUI; //reference to the settings ui
    [SerializeField] protected GameObject CharacterSelectUI; //reference to the char select screen ui
    public string playerOneCharacter = "";
    public string playerTwoCharacter = "";
    public string playerThreeCharacter = "";
    public string playerFourCharacter = "";

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
    public enum GameState { KickStart, MainMenu, Settings, CharSelect, GameStart, Spinning }
    public GameState currentState = GameState.MainMenu; //for tracking current state

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
        }
    }

    public void KickStart()
    {

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

    }
    public void GameStart()
    {

    }
    public void Spinning()
    {
        wheelUI.SetActive(true);

    }
}
