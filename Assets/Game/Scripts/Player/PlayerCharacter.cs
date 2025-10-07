using Characters;
using UnityEngine;

/// <summary>
/// Handles the initialization of the player character:
/// - Assigns base stats (speed, health)
/// - Chooses and equips the appropriate weapon
/// - Acts as a bridge between character data and game logic
/// </summary>
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerWeaponHandler))]
public class PlayerCharacter : MonoBehaviour
{
    [Header("Character Data")]
    [Tooltip("Contains character stats and type information.")]
    public CharacterData characterData;

    [Header("Weapon Prefabs (assign in inspector)")]
    public WeaponBase gunWeaponPrefab;
    public WeaponBase bowWeaponPrefab;
    public WeaponBase staffWeaponPrefab;

    private PlayerController controller;
    private PlayerWeaponHandler weaponHandler;
    private int currentHealth;

    private void Awake()
    {
        InitializeReferences();
    }

    private void Start()
    {
        if (characterData != null)
            InitializeCharacter(characterData);
        else
            Debug.LogWarning("[PlayerCharacter] Missing CharacterData.");
    }

    /// <summary>
    /// Finds and caches components for faster access.
    /// </summary>
    private void InitializeReferences()
    {
        controller = GetComponent<PlayerController>();
        weaponHandler = GetComponent<PlayerWeaponHandler>();
    }

    /// <summary>
    /// Initializes the player’s stats and assigns a weapon based on class.
    /// </summary>
    public void InitializeCharacter(CharacterData data)
    {
        if (data == null)
        {
            Debug.LogError("[PlayerCharacter] Tried to initialize with null CharacterData.");
            return;
        }

        characterData = data;
        currentHealth = data.maxHealth;

        ApplyStatsFromData(data);
        EquipStartingWeapon(data.characterType);

        Debug.Log($"[PlayerCharacter] Initialized as {data.characterType} ({data.characterName})");
    }

    /// <summary>
    /// Applies base movement and health stats from the CharacterData asset.
    /// </summary>
    private void ApplyStatsFromData(CharacterData data)
    {
        controller.moveSpeed = data.moveSpeed;
    }

    /// <summary>
    /// Instantiates and equips the weapon prefab corresponding to the character type.
    /// </summary>
    private void EquipStartingWeapon(CharacterType type)
    {
        WeaponBase prefab = GetWeaponPrefabByType(type);

        if (prefab == null)
        {
            Debug.LogError($"[PlayerCharacter] No weapon prefab assigned for {type}.");
            return;
        }

        WeaponBase newWeapon = Instantiate(prefab, transform);
        weaponHandler.EquipWeapon(newWeapon);
    }

    /// <summary>
    /// Returns the correct weapon prefab for the character type.
    /// </summary>
    private WeaponBase GetWeaponPrefabByType(CharacterType type)
    {
        return type switch
        {
            CharacterType.Gunslinger => gunWeaponPrefab,
            CharacterType.Archer => bowWeaponPrefab,
            CharacterType.Mage => staffWeaponPrefab,
            _ => null
        };
    }

    /// <summary>
    /// Called when the player takes damage.
    /// </summary>
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth == 0)
            Die();
    }

    /// <summary>
    /// Handles player death (placeholder — expand later).
    /// </summary>
    private void Die()
    {
        controller.StopMovement();
        Debug.Log("[PlayerCharacter] Player died!");
        // TODO: trigger death animation or game over screen
    }
}
