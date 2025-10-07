using System;
using UnityEngine;

namespace Enemies
{
    /// <summary>
    /// EnemyMovement
    /// -   Moves the enemy straight down at a fixed speed.
    /// - Triggers an event when it goes off-screen (e.g. to damage the player).
    /// </summary>
    public class EnemyMovement : MonoBehaviour
    {
        [Header("Movement Settings")] public float speed = 2f; // Downward speed (units per second)

        [Header("Bounds")] public float offscreenMargin = 1f; // How far below the screen to trigger event

        // Event triggered when the enemy leaves the screen
        public static event Action<EnemyMovement> OnEnemyExitedScreen;

        private Camera _mainCam;

        private void Start()
        {
            _mainCam = Camera.main;
        }

        private void Update()
        {
            MoveStraightDown();
            CheckOffscreen();
        }

        /// <summary>
        /// Moves the enemy straight down along the Y axis.
        /// </summary>
        private void MoveStraightDown()
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }

        /// <summary>
        /// Checks if the enemy has moved below the visible area.
        /// If so, invokes an event and destroys the object.
        /// </summary>
        private void CheckOffscreen()
        {
            if (_mainCam == null) return;

            var bottomY = _mainCam.transform.position.y - _mainCam.orthographicSize - offscreenMargin;

            if (!(transform.position.y < bottomY)) return;
            // Trigger global event
            OnEnemyExitedScreen?.Invoke(this);

            // Destroy enemy
            Destroy(gameObject);
        }
    }
}