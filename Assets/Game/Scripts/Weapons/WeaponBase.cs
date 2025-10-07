using UnityEngine;

/// <summary>
/// Abstract base class for all weapon types (Gun, Bow, Staff...).
/// </summary>
public abstract class WeaponBase : MonoBehaviour
{
    /// <summary>
    /// Called when the player presses or holds the fire button.
    /// </summary>
    public abstract void HandleFireInput(bool isFiring);
}