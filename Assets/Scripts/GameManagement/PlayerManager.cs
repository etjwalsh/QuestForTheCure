using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public static int numPlayers;

    //list of players
    public List<Player> players = new List<Player>();
    public List<GameObject> playerPieces = new List<GameObject>();
    public Vector3[] playerLocations;
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

    public void SavePlayerLocations()
    {
        //initialize the array
        playerLocations = new Vector3[players.Count];

        //save all the player locations to the list
        for (int i = 0; i < players.Count; i++)
        {
            //save the player pieces to the list
            playerPieces[i] = players[i].characterPiece;

            //save the locations of the players
            players[currentPlayerIndex].location = current.space.transform.position;
            playerLocations[i] = players[i].location;
        }

        for (int j = 0; j < playerPieces.Count; j++)
        {
            Debug.Log("here are the player pieces before trivia: " + playerPieces[j]);
        }
    }

    public void LoadPlayerLocations()
    {
        SceneManager.LoadScene("Sandbox");

        for (int j = 0; j < playerPieces.Count; j++)
        {
            Debug.Log("here are the player pieces after trivia: " + playerPieces[j]); //this is working?
        }

        for (int i = 0; i < playerLocations.Length; i++)
        {
            playerPieces[i].transform.position = playerLocations[i];
        }
    }
}
