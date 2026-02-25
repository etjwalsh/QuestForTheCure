using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowManyPlayers : MonoBehaviour
{
    //reference to the character select ui and its script
    [SerializeField] private GameObject csUI;
    private CharacterSelect cs;

    void Awake()
    {
        cs = csUI.GetComponent<CharacterSelect>();
    }

    //single player selected
    public void OnOneClicked()
    {
        PlayerManager.numPlayers = 1; //set number of players
        cs.numPlayersToSelect = PlayerManager.numPlayers;
        GameStateMachine.instance.currentState = GameStateMachine.GameState.Tutorial; //change the current game state to tutorial
    }

    //two player selected
    public void OnTwoClicked()
    {
        PlayerManager.numPlayers = 2; //set number of players
        cs.numPlayersToSelect = PlayerManager.numPlayers;
        GameStateMachine.instance.currentState = GameStateMachine.GameState.Tutorial; //change the current game state to tutorial
    }

    //three player selected
    public void OnThreeClicked()
    {
        PlayerManager.numPlayers = 3; //set number of players
        cs.numPlayersToSelect = PlayerManager.numPlayers;
        GameStateMachine.instance.currentState = GameStateMachine.GameState.Tutorial; //change the current game state to tutorial
    }

    //four player selected
    public void OnFourClicked()
    {
        PlayerManager.numPlayers = 4; //set number of players
        cs.numPlayersToSelect = PlayerManager.numPlayers;
        GameStateMachine.instance.currentState = GameStateMachine.GameState.Tutorial; //change the current game state to tutorial
    }

}
