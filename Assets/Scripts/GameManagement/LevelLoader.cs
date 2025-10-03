using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    //this will load the next level in the unity build order
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    //for transitioning between scenes
    IEnumerator LoadLevel(int levelIndex)
    {
        //trigger the crossfade to start
        transition.SetTrigger("start");

        //wait a sec
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(levelIndex);
    }
}
