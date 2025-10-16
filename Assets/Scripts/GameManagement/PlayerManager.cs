using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static int numPlayers;

    //list of players
    public List<Player> players = new List<Player>();
    public int currentPlayerIndex = 0;
    public Player current;

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
        current = players[currentPlayerIndex];
        current.active = true;
        Debug.Log("it is now " + current.playerName + "'s turn and they are set to active = " + current.active);
    }

    public void EndTurn()
    {
        //disable the current player
        current.active = false;

        //increment player index
        currentPlayerIndex = (currentPlayerIndex + 1) % numPlayers;

        //start next player's turn
        StartTurn();
    }
}
