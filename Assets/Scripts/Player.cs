using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Game/Character")]
public class Player : ScriptableObject
{
    public string playerName; //the name of the character that the player chose
    public Sprite playerPiece; //reference to the player's physical game piece
    public bool active = false; //bool for if this player can move 
}