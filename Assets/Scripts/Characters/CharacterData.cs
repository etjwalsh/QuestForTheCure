using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Game/Character")]
public class CharacterData : ScriptableObject
{
    public string charName;
    [TextArea(10,15)]
    public string description;
    public GameObject model;
    public Sprite portrait;
    public Color charColor;
    public GameObject charPiece;
}
