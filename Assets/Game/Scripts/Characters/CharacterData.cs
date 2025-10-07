using UnityEngine;

public enum CharacterType
{
    Gunslinger,
    Archer,
    Mage
}

/// <summary>
/// Defines data for a specific character type.
/// Stored as a ScriptableObject asset so it can be reused easily.
/// </summary>
[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Character Info")]
    public string characterName;
    public CharacterType characterType;

    [Header("Stats")]
    public int maxHealth = 100;
    public float moveSpeed = 5f;

    [Header("Visuals")]
    public Sprite characterSprite;
}