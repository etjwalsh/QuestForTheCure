using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacesTree : MonoBehaviour
{
    //to get the type of space landed on
    public string spaceType;

    //the first node
    public SpacesTree first;

    //parent node directly before this one
    public SpacesTree previous;

    //left and right children
    public SpacesTree left;
    public SpacesTree right;

    //to determine which one comes next
    public SpacesTree next;
}
