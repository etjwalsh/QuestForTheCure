using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform[] spaces;

    private int currentSpaceIndex = -1;
    private int moveSpeed = 5;
    private int roll = -1;


    // Start is called before the first frame update
    void Start()
    {
        roll = Random.Range(1, 7); // 1-6 dice roll
        Debug.Log("roll number == " + roll);
    }

    // Update is called once per frame
    void Update()
    {
        while (roll > 0)
        {
            Debug.Log("got to the while loop and roll is == " + roll); 
            StartCoroutine(MovePlayer(roll));
            roll--;
        }
    }

    IEnumerator MovePlayer(int steps)
    {
        Debug.Log("about to move player " + steps + " steps");
        while (steps > 0)
        {
            Debug.Log("currentSpaceIndex == " + currentSpaceIndex);
            currentSpaceIndex++;
            Transform nextSpace = spaces[currentSpaceIndex];

            Debug.Log("nextSpace == " + nextSpace);

            // Move smoothly to next space
            yield return StartCoroutine(MoveToPosition(nextSpace.position));

            steps--;
        }
        // Player finished moving — trigger space event
        // TriggerSpaceEvent(currentSpaceIndex); // ADD THIS LATER
    }

    IEnumerator MoveToPosition(Vector3 target)
    {
        Debug.Log("got to the MoveToPosition coroutine");
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            Debug.Log("moving towards target == " + target);
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);
            yield return null;
        }
    }
}
