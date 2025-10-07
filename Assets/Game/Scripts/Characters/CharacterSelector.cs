using System.Collections.Generic;
using Characters;
using UnityEngine;
using UnityEngine.InputSystem; // ✅ New Input System

/// <summary>
/// Handles character switching at runtime using the new Input System.
/// Example: Press 1–9 on keyboard to spawn the corresponding character.
/// </summary>
public class CharacterSelector : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Player prefab that contains PlayerCharacter, PlayerController, etc.")]
    public GameObject playerPrefab;

    [Tooltip("List of CharacterData assets to test.")]
    public List<CharacterData> availableCharacters = new List<CharacterData>();

    private GameObject currentPlayerInstance;
    private int currentIndex = 0;

    private Keyboard keyboard; // ✅ New Input System reference

    private void Awake()
    {
        // Cache keyboard reference for readability
        keyboard = Keyboard.current;

        if (keyboard == null)
            Debug.LogError("[CharacterSelector] No keyboard detected — ensure Input System package is enabled.");
    }

    private void Start()
    {
        // Spawn the first available character automatically
        if (availableCharacters.Count > 0)
        {
            SpawnCharacter(0);
        }
        else
        {
            Debug.LogWarning("[CharacterSelector] No characters assigned in the list.");
        }
    }

    private void Update()
    {
        if (keyboard == null) return;

        // ✅ Use the new Input System keys dynamically (Alpha1 → Alpha9)
        for (int i = 0; i < availableCharacters.Count && i < 9; i++)
        {
            Key key = Key.Digit1 + i; // Key.Digit1, Key.Digit2, etc.
            if (keyboard[key].wasPressedThisFrame)
            {
                SpawnCharacter(i);
            }
        }
    }

    /// <summary>
    /// Destroys the current player and spawns a new one based on the selected CharacterData.
    /// </summary>
    private void SpawnCharacter(int index)
    {
        if (index < 0 || index >= availableCharacters.Count)
            return;

        // Destroy the current player instance if one exists
        if (currentPlayerInstance != null)
            Destroy(currentPlayerInstance);

        // Instantiate new player prefab at origin
        currentPlayerInstance = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

        // Get PlayerCharacter component and initialize with CharacterData
        var playerCharacter = currentPlayerInstance.GetComponent<PlayerCharacter>();
        if (playerCharacter != null)
        {
            playerCharacter.InitializeCharacter(availableCharacters[index]);
            Debug.Log($"[CharacterSelector] Spawned: {availableCharacters[index].characterName}");
        }
        else
        {
            Debug.LogError("[CharacterSelector] Player prefab missing PlayerCharacter component!");
        }

        currentIndex = index;
    }
}
