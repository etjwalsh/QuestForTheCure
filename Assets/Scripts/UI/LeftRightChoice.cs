using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightChoice : MonoBehaviour
{
    //get reference to the player's movement script
    [SerializeField] 
    private void Awake()
    {

    }
    public void OnLeftArrowClicked()
    {
        Debug.Log("left clicked");
    }
    
    public void OnRightArrowClicked()
    {
        Debug.Log("right clicked");
    }
}
