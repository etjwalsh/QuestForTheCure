using UnityEngine;

public class Activate : MonoBehaviour
{
    public string scene;

    // Update is called once per frame
    void Update()
    {
        //if the current scene is not the same as the one that this stuff is supposed to be in
        if (LevelLoader.instance.currentScene != scene)
        {
            //deactivate it
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
