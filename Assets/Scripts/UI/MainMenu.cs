using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject aboutUI;

    void Awake()
    {
        //make sure the about section is set to inactive to start
        aboutUI.SetActive(false);
    }

    //function to start the game
    public void OnStartClicked()
    {
        //Start the game
        StartCoroutine(StartGameRoutine());
    }

    //function to exit the game
    public void OnExitClicked()
    {
        //for not in build
        // UnityEditor.EditorApplication.isPlaying = false;

        //in build
        Application.Quit();
    }

    //function to open the settings
    public void OnSettingsClicked()
    {
        Debug.Log("pressed the button to open the settings");
        Debug.Log("Settings gameobject is: " + aboutUI);
        aboutUI.SetActive(true);
    }

    public void OnExitSettingsClicked()
    {
        aboutUI.SetActive(false);
    }

    private IEnumerator StartGameRoutine()
    {
        // Debug.Log("start clicked, about to start the fade to black");
        //fade to black and wait until faded to black
        // yield return StartCoroutine(GameStateMachine.instance.FadeToBlack());

        // yield return new WaitForSeconds(1.5f);
        yield return null;

        // Debug.Log("about to start the fade from black");
        //fade back from black
        // yield return StartCoroutine(GameStateMachine.instance.FadeFromBlack());

        //change game state to the character number selection
        GameStateMachine.instance.currentState = GameStateMachine.GameState.NumCharsSelect;
    }
}
