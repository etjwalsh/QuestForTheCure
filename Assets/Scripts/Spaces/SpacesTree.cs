using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacesTree : MonoBehaviour
{
    //to get the type of space landed on
    public string spaceType;

    //parent node directly before this one
    public GameObject previous;

    //left and right children
    public GameObject left;
    public GameObject right;
}
