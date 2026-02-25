using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    public void OnClicked()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
