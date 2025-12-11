using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static int numPlayers;

    //list of players
    public List<Player> players = new List<Player>();
    //list of player pieces
    public List<GameObject> playerPieces = new List<GameObject>();
    //for saving player locations
    public string[] playerLocations;
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
        for (int i = 0; i < playerPieces.Count; i++)
        {
            playerPieces[i].gameObject.tag = "InactivePlayer";
        }

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

        //start next player's turn
        GameStateMachine.instance.currentState = GameStateMachine.GameState.Spinning;

    }

    public IEnumerator SavePlayerLocations()
    {
        //initialize the array
        playerLocations = new string[players.Count];

        //save all the player locations to the list
        for (int i = 0; i < players.Count; i++)
        {
            //save the player pieces to the list
            playerPieces[i] = players[i].characterPiece;

            //save the locations of the players
            Debug.Log("Printing out the space the player is currently on:" + playerPieces[i].GetComponent<Movement>().space);
            playerLocations[i] = playerPieces[i].GetComponent<Movement>().space.name;

            Debug.Log("printing out playerlocations[i] after setting it:" + playerLocations[i]);
        }

        yield return null;
    }

    public IEnumerator LoadPlayerLocations(string sceneName)
    {
        //change the game state to scene change
        GameStateMachine.instance.currentState = GameStateMachine.GameState.SceneChange;
        Debug.Log("about to load: " + sceneName);
        LevelLoader.instance.LoadScene(sceneName);

        yield return new WaitUntil(() => !LevelLoader.instance.isLoading);

        Debug.Log("loaded scene");

        //loop through all of the pieces and set their player location
        for (int i = 0; i < playerPieces.Count; i++)
        {
            //get reference to the first space in the tree
            playerPieces[currentPlayerIndex].GetComponent<Movement>().space = GameObject.Find(playerLocations[i]).GetComponent<SpacesTree>();
        }
        current = playerPieces[currentPlayerIndex].GetComponent<Movement>();
    }
}
