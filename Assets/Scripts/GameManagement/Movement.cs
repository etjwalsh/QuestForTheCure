using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] public SpacesTree spacesParent;
    public SpacesTree space;
    public GameObject startingSpot = null;
    public WheelSpin wheel;
    public string choice = null;
    public string tagLandedOn = "";

    public int moveSpeed = 5;
    public bool canMove;

    //string for the player's current role
    public string currentRole;


    private void Awake()
    {
        if (space == null && spacesParent == null)
        {
            spacesParent = GameObject.Find("SpacesTree/StartingSpace").GetComponent<SpacesTree>();
        }

        //assign the parent space and the left and right spaces of the parent space
        if (spacesParent != null)
        {
            space = spacesParent;
            Debug.Log("space left is now = " + space.left);
            Debug.Log("space right is now = " + space.right);
        }
        else
        {
            Debug.LogWarning("spacesParent not assigned for player " + PlayerManager.instance.players[PlayerManager.instance.currentPlayerIndex].playerName);
        }

        //set a reference to the wheel UI from the state machine
        wheel = GameStateMachine.instance.wheelUI.GetComponent<WheelSpin>();

        //the player can spin now
        // canMove = true;
    }

    // Start is called before the first frame update
    private void Start()
    {
        wheel.OnRolled += HandleDiceRoll;
    }

    private void OnDestroy()
    {
        wheel.OnRolled -= HandleDiceRoll;
    }

    private void HandleDiceRoll(int roll)
    {
        if (canMove && roll > 0)
        {
            //get reference to the active player
            Player activePlayer = PlayerManager.instance.players[PlayerManager.instance.currentPlayerIndex];

            Debug.Log("about to print out the active player");
            Debug.Log(activePlayer);

            StartCoroutine(MovePlayer(roll, activePlayer));
        }
    }

    IEnumerator MovePlayer(int steps, Player activePlayer)
    {
        Debug.Log("got to the moveplayer coroutine");
        GameStateMachine.instance.currentState = GameStateMachine.GameState.PlayerMoving;
        // canMove = false;
        choice = null;

        while (steps > 0)
        {
            //check if this space is required
            if (space.gameObject.CompareTag("RequiredTrivia") || space.gameObject.CompareTag("RequiredMinigame") || space.gameObject.CompareTag("EndOfStage"))
            {
                //stop moving
                steps = 0;
                continue;
            }
            if (space.next == null)
            {
                //check for split
                if (space.left != null && space.right != null)
                {
                    //reset choice
                    choice = null;
                    //start the l/r choice coroutine and wait for the player to make a choice
                    yield return StartCoroutine(LeftRightChoice());
                    yield return new WaitUntil(() => choice != null);
                }

                //check if there is only right available
                else if (space.left == null && space.right != null)
                {
                    space.next = space.right;
                }

                //check if there is only left available
                else if (space.left != null && space.right == null)
                {
                    space.next = space.left;
                }

                // //nowhere to go
                // else
                // {
                //     Debug.LogError("There are no spcaes assigned");
                // }
            }
            else if (space.next != null && space.left != null && space.right != null) //hit a space after another player did
            {
                //reset choice
                choice = null;
                //start the l/r choice coroutine and wait for the player to make a choice
                yield return StartCoroutine(LeftRightChoice());
                yield return new WaitUntil(() => choice != null);
            }
            //nowhere to go
            // else
            // {
            //     Debug.LogError("There are no spcaes assigned");
            // }

            //set the current space the player is on to the next one's previous space
            space.next.previous = space;

            //set the next space transform for movement
            Transform nextSpace = space.next.transform;

            // Debug.Log("nextSpace == " + nextSpace);

            // Move smoothly to next space
            yield return StartCoroutine(MoveToPosition(nextSpace.position));

            //set the current space to be the space the player just moved to
            space = space.next;

            //subtract the amount of spaces the player can move
            steps--;
        }

        //get the tag that the player landed on
        tagLandedOn = space.gameObject.tag;
        Debug.Log("landed on a " + tagLandedOn + " tag.");
        // canMove = true;

        //change game state to whatever the player landed on
        if (tagLandedOn == "Minigame" || tagLandedOn == "RequiredMinigame")
        {
            if (tagLandedOn == "RequiredMinigame")
            {
                //reset the space to be a normal space
                space.gameObject.tag = "Untagged";
                space.GetComponentInChildren<MeshRenderer>().material = GameStateMachine.instance.genericSpaceMat;

                //get rid of the stop sign
                Destroy(space.stopSign);
            }
            GameStateMachine.instance.currentState = GameStateMachine.GameState.MinigameEnter;
        }
        else if (tagLandedOn == "Trivia" || tagLandedOn == "RequiredTrivia") //make this into a normal space
        {
            //change to the trivia game state
            GameStateMachine.instance.currentState = GameStateMachine.GameState.TriviaEnter;

            if (tagLandedOn == "RequiredTrivia")
            {
                //wait to see the answer to the trivia
                StartCoroutine(WaitForTrivia(space));
            }
        }
        else if (tagLandedOn == "EndOfStage")
        {
            Debug.Log("go to the next stage!");

            //for now, just bring up the end credits UI
            GameStateMachine.instance.currentState = GameStateMachine.GameState.EndTurn;
            LevelLoader.instance.LoadScene("End");

            //move all players to the next stage

            //randomize all of the player's roles

            //go to next player's turn
        }
        //has no tag
        else
        {
            //end the player's turn
            GameStateMachine.instance.currentState = GameStateMachine.GameState.EndTurn;
        }
    }

    IEnumerator WaitForTrivia(SpacesTree space)
    {
        //reset the booleans
        TriviaController.triviaCompleted = false;
        TriviaController.answeredRight = false;

        //wait until trivia is completed
        yield return new WaitUntil(() => TriviaController.triviaCompleted);

        //check if they answered correctly
        if (TriviaController.answeredRight)
        {
            //convert the space to normal
            space.gameObject.tag = "Untagged";
            space.GetComponentInChildren<MeshRenderer>().material = GameStateMachine.instance.genericSpaceMat;

            //get rid of the stop sign
            if (space.stopSign != null)
            {
                Destroy(space.stopSign);
            }
        }
    }

    IEnumerator MoveToPosition(Vector3 target)
    {
        target.y += 0.1f; //makes the player land slightly above the space itself

        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);
            yield return null;
        }

        transform.position = target; //snap the player right to the space
    }

    IEnumerator LeftRightChoice()
    {
        //reset the choice
        choice = null;

        //change game state to choosing
        GameStateMachine.instance.currentState = GameStateMachine.GameState.LRChoice;
        yield return new WaitUntil(() => choice != null);

        //if they choose left
        if (choice == "left")
        {
            //go left
            space.next = space.left;
        }
        //if they choose right
        if (choice == "right")
        {
            //go right
            space.next = space.right;
        }
        //change game state to moving
        GameStateMachine.instance.currentState = GameStateMachine.GameState.PlayerMoving;
    }
}
