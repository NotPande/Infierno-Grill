using UnityEngine;

/// <summary>
/// Simple gun weapon — fires bullets straight upward.
/// Designed for the Gunslinger character.
/// </summary>
public class GunWeapon : WeaponBase
{
    [Header("Gun Settings")]
    [Tooltip("Prefab of the bullet projectile to spawn.")]
    public GameObject bulletPrefab;

    [Tooltip("Speed of the bullet when fired.")]
    public float bulletSpeed = 10f;

    [Tooltip("Time between consecutive shots.")]
    public float fireRate = 0.25f;

    private float _lastFireTime;

    /// <summary>
    /// Called every frame by the PlayerWeaponHandler if this weapon is equipped.
    /// </summary>
    public override void HandleFireInput(bool isFiring)
    {
        if (!isFiring) return;

        // Prevent spamming shots faster than fireRate
        if (Time.time - _lastFireTime < fireRate)
            return;

        FireBullet();
        _lastFireTime = Time.time;
    }

    /// <summary>
    /// Instantiates a bullet and propels it upward.
    /// </summary>
    private void FireBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("[GunWeapon] Bullet prefab not assigned!");
            return;
        }

        // Spawn bullet at weapon position
        GameObject bullet = Instantiate(
            bulletPrefab,
            transform.position,
            Quaternion.identity
        );

        // Apply velocity upward
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.up * bulletSpeed;

        // Optional: destroy bullet after 3 seconds to avoid clutter
        Destroy(bullet, 3f);

        Debug.Log("[GunWeapon] Bullet fired!");
    }
}