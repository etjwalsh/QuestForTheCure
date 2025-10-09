using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static int numPlayers = 0;
    public static string playerOneCharacter = "";
    public static string playerTwoCharacter = "";
    public static string playerThreeCharacter = "";
    public static string playerFourCharacter = "";

    //list of players
    public List<Player> players = new List<Player>();
    public int currentPlayerIndex = 0;

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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("player one character is = " + playerOneCharacter);
    }
}
