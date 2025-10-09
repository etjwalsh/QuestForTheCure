using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpaceType : MonoBehaviour
{
    public enum TileType { Regular, Trivia, Minigame }
    public TileType tileType;

    // Update is called once per frame
    void Update()
    {
        switch (tileType)
        {
            case TileType.Regular:
                {
                    Regular();
                    break;
                }
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

    private void Regular()
    {
        // Debug.Log("this is a regular space");
    }
    private void Trivia()
    {
        // Debug.Log("this is a trivia space");
    }
    private void Minigame()
    {
        // Debug.Log("this is a minigame space");
    }
}
