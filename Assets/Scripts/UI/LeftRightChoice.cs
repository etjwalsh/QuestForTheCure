using System.Collections;
using UnityEngine;

public class LeftRightChoice : MonoBehaviour
{
    //get reference to the player's movement script
    [SerializeField] public GameObject playerRef;
    private Movement playerScript;

    void Update()
    {
        StartCoroutine(WaitForPlayer());
    }

    public void OnLeftArrowClicked()
    {
        // StartCoroutine(WaitForPlayer());
        playerScript = playerRef.GetComponent<Movement>();
        Debug.Log("left clicked");
        //set the player's choice to next (left)
        playerScript.choice = "left";
    }

    public void OnRightArrowClicked()
    {
        // StartCoroutine(WaitForPlayer());
        playerScript = playerRef.GetComponent<Movement>();
        Debug.Log("right clicked");
        //set the player choice to next (right)
        playerScript.choice = "right";
    }

    //to populate the player reference
    IEnumerator WaitForPlayer()
    {
        yield return new WaitUntil(() => GameObject.FindWithTag("ActivePlayer") != null);
        playerRef = GameObject.FindWithTag("ActivePlayer");
        Debug.Log("Found player for UI = " + playerRef);
    }
}
