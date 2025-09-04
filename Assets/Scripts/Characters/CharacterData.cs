using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Game/Character")]
public class CharacterData : ScriptableObject
{
    public string charName;
    [TextArea(10,15)]
    public string description;
    public Sprite model;
    public Sprite portrait;
    public Color charColor;
}
