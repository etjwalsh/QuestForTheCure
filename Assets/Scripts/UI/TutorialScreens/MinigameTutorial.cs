using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTutorial : MonoBehaviour
{
    public int time = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitThenStop());
    }

    public void OnClicked()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    private IEnumerator WaitThenStop()
    {
        yield return new WaitForSeconds(1.0f);
        Time.timeScale = time;
    }
}
