using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClinicalLoaded : MonoBehaviour
{
    //reference to about section UI
    public GameObject aboutUI;

    void Start()
    {
        //set all of the characters to have the starting space be the starting space
        Movement playerScript;

        //set the players to be in the correct position
        for (int i = 0; i < PlayerManager.numPlayers; i++)
        {
            playerScript = PlayerManager.instance.playerPieces[i].GetComponent<Movement>();

            if (!playerScript.space)
            {
                //locate the starting spot 
                playerScript.startingSpot = GameObject.Find("SpacesTree/StartingSpace");
                playerScript.space = playerScript.startingSpot.GetComponent<SpacesTree>();
            }
        }

        //set the correct stage of the game
        GameStateMachine.instance.currentStage = "Clinical";

        //activate this scene's about section
        aboutUI.SetActive(true);

        //remove this from the scene
        Destroy(gameObject);
    }
}
