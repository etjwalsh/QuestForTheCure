using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpaceType : MonoBehaviour
{
    public enum TileType { Trivia, Minigame }
    public TileType tileType;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (tileType)
        {
            case TileType.Trivia:
                {
                    Trivia();
                    break;
                }
            case TileType.Minigame:
                {
                    Minigame();
                    break;
                }
        }
    }

    private void Trivia()
    {
        Debug.Log("this is a trivia space");
    }
    private void Minigame()
    {
        Debug.Log("this is a minigame space");
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("COLLIDED WITH SOMETHING");
        // if (canMove)
        // {
        // }
    }
}
