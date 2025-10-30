using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
        //set the current player
        current = playerPieces[currentPlayerIndex].GetComponent<Movement>();

        //activate the current player
        current.canMove = true;

        //tag the player so the game knows which one to move
        current.gameObject.tag = "ActivePlayer";

        //set the active camera
        activeCamera = current.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        activeCamera.Priority = 10;

        //fade back in from black
        // StartCoroutine(GameStateMachine.instance.FadeFromBlack());

        // yield return new WaitForSeconds(1.5f);
        // fadeAnim.Play("fadeToBlack", 0, 0f);
        // yield return new WaitForSeconds(fadeOutDuration);
    }

    public void EndTurn()
    {
        //untag the player
        current.gameObject.tag = "InactivePlayer";

        //disable the current player
        current.canMove = false;

        //increment player index
        // currentPlayerIndex = (currentPlayerIndex + 1) % numPlayers;
        if (currentPlayerIndex >= numPlayers - 1)
        {
            currentPlayerIndex = 0;
        }
        else
        {
            currentPlayerIndex++;
        }

        //set the active camera
        activeCamera = current.gameObject.GetComponentInChildren<CinemachineVirtualCamera>();
        activeCamera.Priority = 0;

        //fade to black screen
        // StartCoroutine(GameStateMachine.instance.FadeToBlack());

        // fadeAnim.Play("fadeToBlack", 0, 0f);
        // yield return new WaitForSeconds(fadeOutDuration);

        //start next player's turn
        GameStateMachine.instance.currentState = GameStateMachine.GameState.Spinning;

    }
}
