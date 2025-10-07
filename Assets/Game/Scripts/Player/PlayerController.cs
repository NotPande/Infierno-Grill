using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles all player movement using Rigidbody2D.
/// Responsible only for movement logic (no combat or animation).
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("How fast the player moves in world units per second.")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        InitializeComponents();
    }

    private void Update()
    {
        HandleMovement();
    }

    /// <summary>
    /// Finds and initializes necessary components.
    /// </summary>
    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // Disable gravity for top-down movement
        rb.freezeRotation = true;
    }

    /// <summary>
    /// Updates player velocity based on input.
    /// </summary>
    private void HandleMovement()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    /// <summary>
    /// Reads movement input from Unity’s Input System.
    /// </summary>
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    /// <summary>
    /// Stops player movement completely (e.g., when dying or during cutscenes).
    /// </summary>
    public void StopMovement()
    {
        moveInput = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
    }
}