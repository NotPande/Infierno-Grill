using UnityEngine;
using UnityEngine.Tilemaps;

namespace Enemies
{
    /// <summary>
    /// Spawns enemies at the exact Tilemap cell centers above the camera.
    /// Works with any grid-based layout and keeps enemies aligned with visual cells.
    /// </summary>
    public class TilemapEnemySpawner : MonoBehaviour
    {
        [Header("References")] public Tilemap tilemap; // The Tilemap used as grid reference
        public Camera mainCamera; // Main camera (used to center the spawn area)
        public GameObject enemyPrefab; // Prefab of the enemy to spawn

        [Header("Spawn Settings")] public int columns = 5; // Number of columns available for spawning
        public int spawnRowOffset = 6; // How many rows above the camera enemies appear
        public float spawnInterval = 1.5f; // Time (seconds) between each spawn
        public int maxEnemies = 6; // Maximum active enemies allowed in the scene

        private float _timer; // Internal timer for spawn intervals

        private void Awake()
        {
            // Auto-assign references if not set in the Inspector
            if (tilemap == null) tilemap = FindAnyObjectByType<Tilemap>();
            if (mainCamera == null) mainCamera = Camera.main;

            if (tilemap == null)
                Debug.LogError("[TilemapEnemySpawner] No Tilemap found!");
            if (enemyPrefab == null)
                Debug.LogError("[TilemapEnemySpawner] Enemy prefab not assigned!");
        }

        private void Update()
        {
            // Skip if references are missing
            if (tilemap == null || mainCamera == null || enemyPrefab == null)
                return;

            _timer += Time.deltaTime;

            // Try to spawn when timer reaches interval
            if (!(_timer >= spawnInterval)) return;
            _timer = 0f;

            if (CanSpawn()) 
                SpawnEnemy();
        }

        /// <summary>
        /// Checks if we can spawn another enemy (based on current count).
        /// </summary>
        private bool CanSpawn()
        {
            var activeEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
            return activeEnemies < maxEnemies;
        }

        /// <summary>
        /// Spawns a single enemy at a random column in the top spawn row.
        /// </summary>
        private void SpawnEnemy()
        {
            var spawnPosition = GetRandomSpawnPosition();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // Debug visualization (optional)
            Debug.DrawLine(spawnPosition, spawnPosition + Vector3.down * 0.5f, Color.green, 2f);
            Debug.Log($"[Spawner] Enemy spawned at {spawnPosition}");
        }

        /// <summary>
        /// Calculates a random spawn position centered within a Tilemap cell.
        /// </summary>
        private Vector3 GetRandomSpawnPosition()
        {
            // Convert the camera's current world position to grid coordinates
            var cameraCell = tilemap.WorldToCell(mainCamera.transform.position);

            // Calculate the top row (above the camera)
            var topRow = cameraCell.y + spawnRowOffset;

            // Determine the leftmost cell so columns are centered around the camera
            var half = columns / 2;
            var leftCol = cameraCell.x - half;

            // Pick a random column index within the visible range
            var chosenColumn = leftCol + Random.Range(0, columns);

            // Convert that cell position back to world space using Tilemap API
            var spawnCell = new Vector3Int(chosenColumn, topRow, 0);
            var spawnWorld = tilemap.GetCellCenterWorld(spawnCell);

            // Enforce 2D plane (z = 0)
            spawnWorld.z = 0f;

            return spawnWorld;
        }
    }
}