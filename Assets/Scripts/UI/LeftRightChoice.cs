using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightChoice : MonoBehaviour
{
    //get reference to the player's movement script
    [SerializeField] GameObject playerRef;
    private Movement playerScript;

    private void Awake()
    {
        //get reference to player movement script
        playerScript = playerRef.GetComponent<Movement>();
    }
    public void OnLeftArrowClicked()
    {
        Debug.Log("left clicked");
        //set the player's choice to next (left)
        playerScript.choice = "left";
    }
    
    public void OnRightArrowClicked()
    {
        Debug.Log("right clicked");
        //set the player choice to next (right)
        playerScript.choice = "right";
    }
}
