using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Game/Character")]
public class CharacterData : ScriptableObject
{
    public string charName;
    public string description;
    public Sprite model;
    public Sprite portrait;
    public Color charColor;
}
