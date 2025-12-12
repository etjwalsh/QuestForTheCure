using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
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
        GameStateMachine.instance.currentState = GameStateMachine.GameState.Settings;
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
