using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPreClinical : MonoBehaviour
{
    public GameObject newSpacesTree;
    public GameObject preClinicalEnvironment;

    // Start is called before the first frame update
    void Start()
    {
        //make sure the new stuff is inactive
        newSpacesTree.SetActive(false);
        preClinicalEnvironment.SetActive(false);

        //destroy everything from the Discovery scene NOT the players though
        Destroy(GameObject.FindWithTag("DiscoveryEnvironment"));
        Destroy(GameObject.FindWithTag("SpacesTree"));

        //set the new environment active
        newSpacesTree.SetActive(true);
        preClinicalEnvironment.SetActive(true);

        //load the next scene
        LevelLoader.instance.LoadScene("PreClinical");
    }
}
