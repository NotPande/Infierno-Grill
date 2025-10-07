using UnityEngine;

namespace Characters
{
    /// <summary>
    /// ScriptableObject that defines a playable character’s base data:
    /// - Core stats (health, speed)
    /// - Character type (used to determine weapon & behavior)
    /// - Visual representation (sprite)
    /// </summary>
    [CreateAssetMenu(
        fileName = "NewCharacterData",
        menuName = "Game/Character Data",
        order = 0
    )]
    public class CharacterData : ScriptableObject
    {
        [Header("Basic Info")]
        [Tooltip("Display name of the character.")]
        public string characterName = "Unnamed";

        [Tooltip("Class type used to determine weapon and logic.")]
        public CharacterType characterType;

        [Header("Stats")]
        [Tooltip("Maximum health points for this character.")]
        public int maxHealth = 100;

        [Tooltip("Base movement speed of the character.")]
        public float moveSpeed = 5f;

        [Header("Visuals")]
        [Tooltip("Sprite that represents this character in the scene.")]
        public Sprite characterSprite;

        [Tooltip("Optional icon used for UI / selection menus.")]
        public Sprite characterIcon;
    }
}