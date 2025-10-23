using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathInitializer : MonoBehaviour
{
    // Data structure to easily input X and Z coordinates in the Inspector
    [System.Serializable]
    public struct PathCoordinate
    {
        public int x;
        public int z;
    }

    // Main Path Setup
    [Header("Main Path Setup (Green)")]
    [Tooltip("List the grid coordinates (X, Z) in order for the main path.")]
    public PathCoordinate[] pathCoordinates;
    public Color pathColor = Color.green;

    // Start Points Setup
    [Header("Start Point Setup (Blue)")]
    [Tooltip("List the 3 specific grid coordinates (X, Z) for the start options.")]
    public PathCoordinate[] startPointCoordinates;
    public Color startPointColor = Color.blue;

    // End Point Setup
    [Header("End Point Setup (Red)")]
    [Tooltip("The single coordinate (X, Z) for the end point.")]
    public PathCoordinate endPointCoordinate;
    public Color endPointColor = Color.red; // Set to red

    void Start()
    {
        if (GridManager.Instance == null)
        {
            Debug.LogError("GridManager not found! Ensure it's attached and running on the same object.");
            return;
        }

        // Color all sections when the scene loads
        ColorThePath();
        ColorStartPoints();
        // Call the new function for the End Point
        ColorEndPoint();
    }

    // Colors the squares defined in Start Point Coordinates array
    void ColorStartPoints()
    {
        foreach (PathCoordinate coord in startPointCoordinates)
        {
            GridCell cell = GridManager.Instance.GetCell(coord.x, coord.z);

            if (cell != null)
            {
                cell.SetColor(startPointColor);
            }
        }
    }

    // Colors the squares defined in Path Coordinates array
    void ColorThePath()
    {
        foreach (PathCoordinate coord in pathCoordinates)
        {
            GridCell cell = GridManager.Instance.GetCell(coord.x, coord.z);

            if (cell != null)
            {
                cell.SetColor(pathColor);
            }
        }
    }

    // Colors the single end square red
    void ColorEndPoint()
    {
        // Get the single coordinate
        int x = endPointCoordinate.x;
        int z = endPointCoordinate.z;

        GridCell cell = GridManager.Instance.GetCell(x, z);

        if (cell != null)
        {
            cell.SetColor(endPointColor);
        }
        else
        {
            Debug.LogWarning($"End Point coordinate ({x}, {z}) is outside grid bounds or missing.");
        }
    }
}