using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [Header("Trash Can Sprites")]
    public Sprite closedTrash;
    public Sprite openTrash;

    void OnMouseEnter()
    {
        //open the trash can
        gameObject.GetComponent<SpriteRenderer>().sprite = openTrash;
    }

    void OnMouseExit()
    {
        //close the trash can
        gameObject.GetComponent<SpriteRenderer>().sprite = closedTrash;
    }
}
