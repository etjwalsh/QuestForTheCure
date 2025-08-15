using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //function to start the game
    public void OnStartClicked()
    {
        //change game state to the start
        GameStateMachine.instance.currentState = GameStateMachine.GameState.CharSelect;
        //change scene to the character select screen
        SceneManager.LoadScene("CharacterSelect");
    }

    //function to exit the game
    public void OnExitClicked()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        // Application.Quit();
    }

    //function to open the settings
    public void OnSettingsClicked()
    {
        GameStateMachine.instance.currentState = GameStateMachine.GameState.Settings;
    }
}
