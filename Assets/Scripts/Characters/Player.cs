using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Game/Character")]
public class Player : ScriptableObject
{
    public string playerName; //the name of the character that the player chose
    public GameObject playerModel; //reference to the player's physical game piece
    public bool active = false; //bool for if this player can move 
    public GameObject characterPiece; //game object for the player's actual game piece
    public Vector3 location; //for saving the current space the player is on
}