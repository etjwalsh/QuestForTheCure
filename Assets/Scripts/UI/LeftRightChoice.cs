using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightChoice : MonoBehaviour
{
    //get reference to the player's movement script
    [SerializeField] public GameObject playerRef;
    private Movement playerScript;

    public void OnLeftArrowClicked()
    {
        StartCoroutine(WaitForPlayer());
        playerScript = playerRef.GetComponent<Movement>();
        Debug.Log("left clicked");
        //set the player's choice to next (left)
        playerScript.choice = "left";
    }
    
    public void OnRightArrowClicked()
    {
        StartCoroutine(WaitForPlayer());
        playerScript = playerRef.GetComponent<Movement>();
        Debug.Log("right clicked");
        //set the player choice to next (right)
        playerScript.choice = "right";
    }

    //to populate the player reference
    IEnumerator WaitForPlayer()
    {
        while (playerRef == null)
        {
            playerRef = GameObject.FindWithTag("Player");
            yield return new WaitUntil(() => playerRef != null);
        }
        Debug.Log("Found player for UI");
    }
}
