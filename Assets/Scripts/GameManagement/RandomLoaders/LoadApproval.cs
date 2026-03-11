using UnityEngine;

public class LoadApproval : MonoBehaviour
{
    public GameObject newSpacesTree;
    public GameObject approvalEnvironment;

    // Start is called before the first frame update
    void Start()
    {
        //make sure the new stuff is inactive
        newSpacesTree.SetActive(false);
        approvalEnvironment.SetActive(false);

        //activate the whole previous scene
        LevelLoader.instance.environment.SetActive(true);
        LevelLoader.instance.spacesTree.SetActive(true);

        //destroy everything from the precious scene NOT the players though
        Destroy(GameObject.FindWithTag("Environment"));
        Destroy(GameObject.FindWithTag("SpacesTree"));

        //set the new environment active
        newSpacesTree.SetActive(true);
        approvalEnvironment.SetActive(true);

        //set the level loader to have this scene's stuff in it
        LevelLoader.instance.environment = approvalEnvironment;
        LevelLoader.instance.spacesTree = newSpacesTree;

        float spacing = 2.0f;
        float startOffset = -(spacing * (PlayerManager.numPlayers - 1) / 2f);
        Movement playerScript;

        //set the players to be in the correct position
        for (int i = 0; i < PlayerManager.numPlayers; i++)
        {
            Debug.Log("setting all of the player's starting space to " + GameObject.Find("SpacesTree/StartingSpace"));
            playerScript = PlayerManager.instance.players[i].characterPiece.GetComponent<Movement>();

            //offset along Z axis for spawning players
            Vector3 offset = new Vector3(0, 0, startOffset + (i * spacing));

            //locate the starting spot 
            playerScript.startingSpot = GameObject.Find("SpacesTree/StartingSpace");
            playerScript.space = playerScript.startingSpot.GetComponent<SpacesTree>();

            //set each player's position to be the position of the 
            Debug.Log("player script starting spot is: " + playerScript.startingSpot);
            Debug.Log("player position: " + PlayerManager.instance.playerPieces[i].transform.position);
            PlayerManager.instance.playerPieces[i].transform.position = playerScript.startingSpot.transform.position + offset + new Vector3(-0.5f, 0.05f, -0.5f);
            PlayerManager.instance.playerPieces[i].transform.rotation = playerScript.startingSpot.transform.rotation;
        }

        //refresh the roles list
        GameStateMachine.instance.roles = GameStateMachine.instance.ResetRolesList(GameStateMachine.instance.roles);
        //shuffle the player's roles
        for (int i = 0; i < PlayerManager.instance.players.Count; i++)
        {
            //assign roles to players
            PlayerManager.instance.playerPieces[i].GetComponent<Movement>().currentRole = GameStateMachine.instance.AssignRoleToPlayer();
        }
    }

    void Update()
    {
        //load the next scene
        LevelLoader.instance.LoadScene("Approval");
    }
}
