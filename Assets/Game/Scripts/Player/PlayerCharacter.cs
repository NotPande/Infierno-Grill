using Characters;
using UnityEngine;

/// <summary>
/// Handles initialization of the player character:
/// - Applies base stats (speed, health)
/// - Chooses and equips the appropriate weapon
/// - Sets the correct sprite according to CharacterData
/// - Updates visuals in real time when CharacterData changes (Editor only)
/// </summary>
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerWeaponHandler))]
public class PlayerCharacter : MonoBehaviour
{
    [Header("Character Data")]
    [Tooltip("Contains stats, type, and visuals for this character.")]
    public CharacterData characterData;

    [Header("Weapon Prefabs (assign in inspector)")]
    public WeaponBase gunWeaponPrefab;
    public WeaponBase bowWeaponPrefab;
    public WeaponBase staffWeaponPrefab;

    // Cached references
    private PlayerController controller;
    private PlayerWeaponHandler weaponHandler;
    private SpriteRenderer spriteRenderer;

    // Current state
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
            Debug.LogWarning("[PlayerCharacter] Missing CharacterData reference.");
    }

    /// <summary>
    /// Automatically finds and caches all required components.
    /// </summary>
    private void InitializeReferences()
    {
        controller = GetComponent<PlayerController>();
        weaponHandler = GetComponent<PlayerWeaponHandler>();

        // Look for a SpriteRenderer in this object or a "Visual" child
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Transform visualChild = transform.Find("Visual");
            if (visualChild != null)
                spriteRenderer = visualChild.GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer == null)
            Debug.LogWarning("[PlayerCharacter] No SpriteRenderer found on player or child 'Visual'.");
    }

    /// <summary>
    /// Sets up stats, visuals, and weapons from the given CharacterData.
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
        ApplySpriteFromData(data);
        EquipStartingWeapon(data.characterType);

        Debug.Log($"[PlayerCharacter] Initialized as {data.characterType} ({data.characterName})");
    }

    /// <summary>
    /// Applies numeric stats such as movement speed and health.
    /// </summary>
    private void ApplyStatsFromData(CharacterData data)
    {
        controller.moveSpeed = data.moveSpeed;
    }

    /// <summary>
    /// Applies the sprite defined in CharacterData to the player SpriteRenderer.
    /// </summary>
    private void ApplySpriteFromData(CharacterData data)
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("[PlayerCharacter] Cannot apply sprite: missing SpriteRenderer.");
            return;
        }

        if (data.characterSprite != null)
        {
            spriteRenderer.sprite = data.characterSprite;
            spriteRenderer.sortingLayerName = "Player"; // optional layer
            spriteRenderer.sortingOrder = 5;            // ensure above background
        }
        else
        {
            Debug.LogWarning($"[PlayerCharacter] No sprite assigned for {data.characterName}.");
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Automatically updates visuals in the Editor when CharacterData changes.
    /// </summary>
    private void OnValidate()
    {
        if (!Application.isPlaying && characterData != null)
        {
            // Ensure references stay linked even outside Play mode
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>() ?? transform.Find("Visual")?.GetComponent<SpriteRenderer>();

            if (spriteRenderer != null && characterData.characterSprite != null)
                spriteRenderer.sprite = characterData.characterSprite;
        }
    }
#endif

    /// <summary>
    /// Spawns and equips the correct weapon for the character type.
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
    /// Returns the appropriate weapon prefab for a character type.
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
    /// Handles receiving damage and death.
    /// </summary>
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth == 0)
            Die();
    }

    /// <summary>
    /// Handles player death (to expand with animations, UI, etc.).
    /// </summary>
    private void Die()
    {
        controller.StopMovement();
        Debug.Log("[PlayerCharacter] Player died!");
        // TODO: Add death animation or game over logic
    }
}
