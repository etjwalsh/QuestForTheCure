using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

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

        // //print out the player pieces before trivia enter
        // for (int j = 0; j < playerPieces.Count; j++)
        // {
        //     Debug.Log("here are the player pieces before trivia: " + playerPieces[j]);
        // }

        yield return null;
    }

    public IEnumerator LoadPlayerLocations(string sceneName)
    {
        //change the game state to scene change
        GameStateMachine.instance.currentState = GameStateMachine.GameState.SceneChange;
        LevelLoader.instance.LoadScene(sceneName);

        yield return new WaitUntil(() => !LevelLoader.instance.isLoading);

        Debug.Log("bookmark to figure out where i am bruh");

        //loop through all of the pieces and set their player location
        for (int i = 0; i < playerPieces.Count; i++)
        {
            //get reference to the first space in the tree
            Debug.Log("inside of the for loop for loading player locations");
            playerPieces[currentPlayerIndex].GetComponent<Movement>().space = GameObject.Find(playerLocations[i]).GetComponent<SpacesTree>();
        }
        current = playerPieces[currentPlayerIndex].GetComponent<Movement>();
    }

    // private void SearchPlayerTree(SpacesTree space, int i)
    // {
    //     Debug.Log("checking space: " + space);
    //     //check if the current space is what you're looking for
    //     if (space.gameObject.name == playerLocations[i])
    //     {
    //         Debug.Log("about to set " + playerPieces[i] + "'s space to " + space);
    //         //set the current player's piece to this space
    //         playerPieces[i].GetComponent<Movement>().space = space;
    //         return;
    //     }

    //     if (space.left) //if left exists
    //     {
    //         SearchPlayerTree(space.left, i);
    //     }
    //     if (space.right) //if right exists
    //     {
    //         SearchPlayerTree(space.right, i);
    //     }
    //     if (space.next) //if next exists
    //     {
    //         SearchPlayerTree(space.next, i);
    //     }
    // }
}
