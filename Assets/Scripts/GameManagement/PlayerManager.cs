using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static int numPlayers;

    //list of players
    public List<Player> players = new List<Player>();
    public List<GameObject> playerPieces = new List<GameObject>();
    public int currentPlayerIndex = 0;
    public Movement current;
    CinemachineVirtualCamera activeCamera;

    //singleton pattern
    private static PlayerManager _instance;
    public static PlayerManager instance
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
        //set instance of state machine and make sure one doesn't already exist
        if (instance != null)
        {
            Debug.LogWarning("warning: too many instances of player manager");
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartTurn()
    {
        //set the game state to spinning
        GameStateMachine.instance.currentState = GameStateMachine.GameState.Spinning;
        //set the current player
        current = playerPieces[currentPlayerIndex].GetComponent<Movement>();

        //activate the current player
        current.canMove = true;

        //tag the player so the game knows which one to move
        current.gameObject.tag = "ActivePlayer";

        //set the active camera
        activeCamera = current.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        activeCamera.Priority = 10;
    }

    public void EndTurn()
    {
        //untag the player
        current.gameObject.tag = "InactivePlayer";

        //disable the current player
        current.canMove = false;

        //increment player index
        currentPlayerIndex = (currentPlayerIndex + 1) % numPlayers;

        //set the active camera
        activeCamera = current.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        activeCamera.Priority = 0;

        //start next player's turn
        StartTurn();
    }
}
