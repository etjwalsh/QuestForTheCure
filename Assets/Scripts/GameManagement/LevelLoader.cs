using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Cinemachine;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance { get; private set; }

    public string currentScene { get; private set; }
    public string previousScene { get; private set; }
    public bool isLoading { get; private set; }

    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 0.5f;

    //all references to stuff
    public GameObject environment = null;
    public GameObject spacesTree = null;
    public GameObject activePlayer = null;
    public GameObject[] inactivePlayers = null;
    private bool val;


    //for spawning players
    private Movement playerScript;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
            currentScene = SceneManager.GetActiveScene().name;

            //Make sure fade starts transparent
            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = 0f;
                fadeImage.color = c;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        if (!isLoading)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }
    }

    public void LoadPreviousScene()
    {
        if (!string.IsNullOrEmpty(previousScene) && !isLoading)
        {
            StartCoroutine(LoadSceneAsync(previousScene));
        }
    }

    public IEnumerator LoadSceneAsync(string sceneName)
    {
        Debug.Log("about to load scene async: " + sceneName);
        Debug.Log("coming from the scene: " + currentScene);
        isLoading = true;

        //Fade out
        yield return StartCoroutine(FadeOut());

        //check if moving from the main menu
        if (currentScene == "MainMenu" || currentScene == "Discovery" || currentScene == "PreClinical" || currentScene == "Clinical" || currentScene == "Approval")
        {
            //set a function call for after the scene is loaded
            SceneManager.sceneLoaded += OnLoaded;
        }
        if (currentScene == "MoleculeMayhem" || currentScene == "ToxicologyReport" || currentScene == "SideEffectSearch" || currentScene == "") //insert approval minigame
        {
            Debug.Log("about to activate current scene: " + currentScene);

            ActivateScene(true);
        }

        //Store previous scene before loading new one
        previousScene = currentScene;
        currentScene = sceneName;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        //Wait until scene is loaded (progress reaches 0.9)
        while (operation.progress < 0.9f)
        {
            float progress = operation.progress / 0.9f;
            //You can broadcast this progress to UI elements
            //EventManager.TriggerEvent("LoadingProgress", progress);
            yield return null;
        }

        //wait, minimum loading time
        yield return new WaitForSeconds(0.5f);

        //Activate the scene
        operation.allowSceneActivation = true;

        //Wait for activation to complete
        yield return operation;

        //get references to activate the scene with
        Debug.Log("Scene name right before all this stuff is: " + sceneName);
        if (environment == null)
        {
            //get references to objects
            environment = GameObject.FindGameObjectWithTag("Environment");

            Debug.Log("environment is: " + environment);

            spacesTree = GameObject.FindGameObjectWithTag("SpacesTree");
            activePlayer = GameObject.FindGameObjectWithTag("ActivePlayer");
            inactivePlayers = GameObject.FindGameObjectsWithTag("InactivePlayer");
        }

        //set when you want the scene stuff to be active vs inactive
        if (sceneName == "Discovery" || sceneName == "PreClinical" || sceneName == "Clinical" || sceneName == "Approval")
        {
            val = true;
        }
        else
        {
            val = false;
        }

        //turn them all off
        ActivateScene(val);

        //fade in
        yield return StartCoroutine(FadeIn());

        isLoading = false;
    }

    public void ActivateScene(bool value)
    {
        //activate the scene stuff
        environment.SetActive(value);
        spacesTree.SetActive(value);

        // activePlayer = PlayerManager.instance.playerPieces[PlayerManager.instance.currentPlayerIndex];

        Debug.Log("Active Player is: " + activePlayer + "and they should be: " + value);
        if (activePlayer)
        {
            activePlayer.SetActive(value);
        }

        //activate players
        for (int i = 0; i < inactivePlayers.Length; i++)
        {
            inactivePlayers[i].SetActive(value);
        }

        Debug.Log("everything should now be: " + value);
    }

    // Optional: Get scene by index
    public void LoadScene(int sceneIndex)
    {
        string sceneName = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
        sceneName = System.IO.Path.GetFileNameWithoutExtension(sceneName);
        LoadScene(sceneName);
    }

    private IEnumerator FadeOut()
    {
        //reset the time scale
        if (Time.timeScale != 1)
        {
            Time.timeScale = 1;
        }

        if (fadeImage == null) yield break;

        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;
    }

    private IEnumerator FadeIn()
    {
        if (fadeImage == null) yield break;

        float elapsed = 0f;
        Color c = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;
    }

    //this method is just for use when going from the main menu to the discovery scene
    public void OnLoaded(Scene scene, LoadSceneMode mode)
    {
        //make sure the correct scene loaded
        if (scene.name != "LoadDiscovery")
        {
            return;
        }

        //remove this function call so it doesn't dupe next time
        SceneManager.sceneLoaded -= OnLoaded;

        //get how many players need to be spawned in
        int numPlayersToSpawn = PlayerManager.numPlayers;
        // Debug.Log("number of players to spawn = " + numPlayersToSpawn);

        //spawn in characters based on the characters inside of the player list (access from PlayerManager)
        //add this later, for now just spawn in 4 of the same generic character

        //vars for the spawning
        float spacing = 2.0f;
        float startOffset = -(spacing * (PlayerManager.numPlayers - 1) / 2f);

        //refresh the roles list
        GameStateMachine.instance.roles = GameStateMachine.instance.ResetRolesList(GameStateMachine.instance.roles);

        //for loop to spawn all the players
        for (int i = 0; i < PlayerManager.numPlayers; i++)
        {
            //get reference to current player's script
            playerScript = PlayerManager.instance.players[i].characterPiece.GetComponent<Movement>();

            //offset along Z axis for spawning players
            Vector3 offset = new Vector3(0, 0, startOffset + (i * spacing));

            //locate the starting spot 
            playerScript.startingSpot = GameObject.Find("SpacesTree/StartingSpace");

            //spawn in a new player
            Debug.Log("About to spawn in gameobject:" + PlayerManager.instance.players[i].characterPiece);
            GameObject spawnedPlayer = Instantiate(PlayerManager.instance.players[i].characterPiece, playerScript.startingSpot.transform.position + offset + new Vector3(0, 0.05f, 0), playerScript.startingSpot.transform.rotation);

            //add this gameObject to the list of player pieces
            PlayerManager.instance.playerPieces.Add(spawnedPlayer);

            //assign the player a role
            spawnedPlayer.GetComponent<Movement>().currentRole = GameStateMachine.instance.AssignRoleToPlayer();
            Debug.Log("player's role is now: " + spawnedPlayer.GetComponent<Movement>().currentRole);

            //set the priority of the spawned player's camera to 0
            Debug.Log("about to get the camera and set the priority to 0, and then print them both");
            var camera = spawnedPlayer.GetComponentInChildren<CinemachineVirtualCamera>();
            camera.Priority = 0;
        }

        //reset the list of roles
        GameStateMachine.instance.roles = GameStateMachine.instance.ResetRolesList(GameStateMachine.instance.roles);

        GameStateMachine.instance.currentState = GameStateMachine.GameState.Spinning; //will def need to change this later to include tutorial type stuff
    }
}