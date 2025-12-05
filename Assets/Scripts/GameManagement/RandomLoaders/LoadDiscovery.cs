using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadDiscovery : MonoBehaviour
{
    void Update()
    {
        LevelLoader.instance.LoadScene("Discovery");
    }
}
