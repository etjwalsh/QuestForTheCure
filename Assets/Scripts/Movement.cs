using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] SpacesTree spacesParent;
    [SerializeField] GameObject lrUI;
    public SpacesTree space;
    public WheelSpin wheel;

    // private int currentSpaceIndex = -1; // initialize to -1 so that the first one checked is index 0
    private int moveSpeed = 5;
    // private int roll = 5; 
    public bool canMove;

    private void Awake()
    {
        //put all spaces into the array
        if (spacesParent != null)
        {
            space = spacesParent;
            Debug.Log("space left is now = " + space.left);
            Debug.Log("space right is now = " + space.right);
        }
        else
        {
            Debug.LogWarning("Parent object not assigned");
        }
        canMove = true;
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
        Debug.Log("canMove is = " + canMove + " and roll is = " + roll);
        if (canMove && roll > 0)
        {
            StartCoroutine(MovePlayer(roll));
        }
    }

    IEnumerator MovePlayer(int steps)
    {
        canMove = false;

        while (steps > 0)
        {
            //check for split
            if (space.left != null && space.right != null)
            {
                Debug.Log("got to a split");
                space.next = space.right;

                //yield return StartCoroutine(LeftRightChoice());
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

            //nowhere to go
            else
            {
                Debug.LogError("There are no spcaes assigned");
            }

            //set the current space the player is on to the next one's previous space
            space.next.previous = space;

            //set the next space transform for movement
            Transform nextSpace = space.next.transform;

            Debug.Log("nextSpace == " + nextSpace);

            // Move smoothly to next space
            yield return StartCoroutine(MoveToPosition(nextSpace.position));

            //set the current space to be the space the player just moved to
            space = space.next;

            //subtract the amount of spaces the player can move
            steps--;
        }
        canMove = true;
    }

    IEnumerator MoveToPosition(Vector3 target)
    {
        target.y += 0.05f; //makes the player land slightly above the space itself

        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);
            yield return null;
        }

        transform.position = target; //snap the player right to the space
    }

    // IEnumerator LeftRightChoice()
    // {
    //     //make player chose left or right via UI (PSEUDOCODE) ---------------------------------
    //     //set L/R UI to true
    //     //get their response
    //     //if they choose left
    //     //space.next = left
    //     //if they choose right
    //     //space.next = right
    //     //deactivate L/R UI
    // }
}
