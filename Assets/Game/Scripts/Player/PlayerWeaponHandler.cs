using UnityEngine;

/// <summary>
/// Handles equipping, managing, and switching between weapons.
/// Does not decide *which* weapon to use — that is handled by PlayerCharacter.
/// </summary>
public class PlayerWeaponHandler : MonoBehaviour
{
    private WeaponBase equippedWeapon;

    /// <summary>
    /// Equips a new weapon. If an old one exists, it is destroyed first.
    /// </summary>
    public void EquipWeapon(WeaponBase newWeapon)
    {
        if (newWeapon == null)
        {
            Debug.LogWarning("[PlayerWeaponHandler] Tried to equip a null weapon.");
            return;
        }

        UnequipCurrentWeapon();

        equippedWeapon = newWeapon;
        AttachWeaponToPlayer(newWeapon);

        Debug.Log($"[PlayerWeaponHandler] Equipped weapon: {newWeapon.name}");
    }

    /// <summary>
    /// Destroys the currently equipped weapon.
    /// </summary>
    public void UnequipCurrentWeapon()
    {
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon.gameObject);
            equippedWeapon = null;
        }
    }

    /// <summary>
    /// Attaches the weapon to the player's hierarchy and resets its position.
    /// </summary>
    private void AttachWeaponToPlayer(WeaponBase weapon)
    {
        weapon.transform.SetParent(transform);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// Returns the currently equipped weapon (null if none).
    /// </summary>
    public WeaponBase GetCurrentWeapon() => equippedWeapon;

    /// <summary>
    /// Fires the equipped weapon (if available).
    /// </summary>
    public void Fire()
    {
        if (equippedWeapon != null)
        {
            // equippedWeapon.Fire();
        }
    }
}