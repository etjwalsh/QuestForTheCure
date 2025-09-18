using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] GameObject spacesParent;
    public Transform[] spaces;
    public WheelSpin wheel;

    private int currentSpaceIndex = -1; // initialize to -1 so that the first one checked is index 0
    private int moveSpeed = 5; 
    // private int roll = 5; 
    public bool canMove;
    public string spaceTag = "";

    private void Awake()
    {
        //put all spaces into the array
        if (spacesParent != null)
        {
            spaces = new Transform[spacesParent.transform.childCount];

            for (int i = 0; i < spacesParent.transform.childCount; i++)
            {
                spaces[i] = spacesParent.transform.GetChild(i).gameObject.transform;
            }
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

            // Debug.Log("nextSpace == " + nextSpace);

            // Move smoothly to next space
            yield return StartCoroutine(MoveToPosition(nextSpace.position));

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
}
