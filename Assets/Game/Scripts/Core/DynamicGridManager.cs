using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    /// <summary>
    /// DynamicGridManager
    /// - Creates and manages a dynamic grid layout for enemy spawning.
    /// - Calculates precise column centers so enemies spawn perfectly centered.
    /// - Can expand grid columns dynamically as difficulty increases.
    /// </summary>
    public class DynamicGridManager : MonoBehaviour
    {
        [Header("Grid Settings")] [SerializeField]
        private int startColumns = 5; // Initial number of columns

        [SerializeField] private float cellWidth = 1.5f; // World space distance between columns
        [SerializeField] private float gridOffsetX = 0f; // Optional horizontal offset

        private readonly List<Vector3> _columnCenters = new List<Vector3>();

        /// <summary>
        /// Returns a copy of the current column centers in world space.
        /// </summary>
        public List<Vector3> GetColumnCenters()
        {
            // Regenerate if list is empty or column count changed
            if (_columnCenters.Count != startColumns)
                GenerateColumnCenters();

            return new List<Vector3>(_columnCenters);
        }

        /// <summary>
        /// Builds evenly spaced column centers based on the grid width and column count.
        /// </summary>
        private void GenerateColumnCenters()
        {
            _columnCenters.Clear();

            // Calculate total grid width and leftmost position
            var totalWidth = startColumns * cellWidth;
            var leftEdge = -totalWidth / 2f + (cellWidth / 2f) + gridOffsetX;

            // Generate center positions for each column
            for (var i = 0; i < startColumns; i++)
            {
                var x = leftEdge + i * cellWidth;
                _columnCenters.Add(new Vector3(x, 0f, 0f)); // Y will be set by spawner
            }
        }

        /// <summary>
        /// Expands the grid by adding more columns (e.g., as difficulty increases).
        /// </summary>
        public void IncreaseColumns(int extraColumns)
        {
            startColumns += extraColumns;
            GenerateColumnCenters();
            Debug.Log($"[DynamicGridManager] Grid expanded: {startColumns} columns total.");
        }

        // /// <summary>
        // /// Draws debug gizmos in the Scene view for visual feedback.
        // /// </summary>
        // private void OnDrawGizmos()
        // {
        //     if (columnCenters.Count == 0)
        //         GenerateColumnCenters();
        //
        //     Gizmos.color = Color.cyan;
        //
        //     foreach (var c in columnCenters)
        //     {
        //         Vector3 pos = transform.TransformPoint(c);
        //         Gizmos.DrawSphere(pos, 0.1f);
        //     }
        //
        //     // Draw grid bounds line for reference
        //     float leftX = columnCenters[0].x;
        //     float rightX = columnCenters[columnCenters.Count - 1].x;
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawLine(new Vector3(leftX - cellWidth / 2f, 0f, 0f), new Vector3(rightX + cellWidth / 2f, 0f, 0f));
        // }
    }
}