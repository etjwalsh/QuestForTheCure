using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform[] spaces;
    public WheelSpin wheel;

    private int currentSpaceIndex = -1; // initialize to -1 so that the first one checked is index 0
    private int moveSpeed = 5; 
    // private int roll = 5; 
    public bool canMove;

    private void Awake()
    {
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
        Debug.Log("got here");
        Debug.Log("canMove is = " + canMove + " and roll is = " + roll);
        if (canMove && roll > 0)
        {
            StartCoroutine(MovePlayer(roll));
        }
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (canMove)
    //     {
    //         StartCoroutine(MovePlayer(roll));
    //         roll--;
    //     }
    // }

    IEnumerator MovePlayer(int steps)
    {
        canMove = false;

        while (steps > 0)
        {
            //to make the game spaces loopable
            //reset the index to the beginning once reaching the end
            if (currentSpaceIndex >= spaces.Length - 1)
            {
                currentSpaceIndex = 0;
            }
            else
            {
                currentSpaceIndex++;
            }

            Transform nextSpace = spaces[currentSpaceIndex];

            Debug.Log("nextSpace == " + nextSpace);

            // Move smoothly to next space
            yield return StartCoroutine(MoveToPosition(nextSpace.position));

            steps--;
        }

        canMove = true;
        // Player finished moving — trigger space event
        // if (steps <= 0)
        // {
        //     canMove = false;
        // }
        // TriggerSpaceEvent(currentSpaceIndex); // ADD THIS LATER
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
}
