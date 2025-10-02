using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightChoice : MonoBehaviour
{
    //get reference to the player's movement script
    [SerializeField] GameObject playerRef;
    private void Awake()
    {

    }
    public void OnLeftArrowClicked()
    {
        Debug.Log("left clicked");
        //set the player's choice to next
    }
    
    public void OnRightArrowClicked()
    {
        Debug.Log("right clicked");
    }
}
