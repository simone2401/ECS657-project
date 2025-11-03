using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    // Static instance for easy access from PathInitializer
    public static GridManager Instance;

    [Tooltip("The total width of your grid (X dimension)")]
    public int gridWidth = 10;

    [Tooltip("The total height/depth of your grid (Z dimension)")]
    public int gridHeight = 10;

    // Store all cells in a dictionary for quick lookup by coordinate string
    private Dictionary<string, GridCell> cellDictionary;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        InitializeCells();
    }

    void InitializeCells()
    {
        cellDictionary = new Dictionary<string, GridCell>();
        GridCell[] allCells = GetComponentsInChildren<GridCell>();

        foreach (GridCell cell in allCells)
        {
            // Manually set up coordinates
            string key = cell.gridX + "," + cell.gridZ;

            if (!cellDictionary.ContainsKey(key))
            {
                cellDictionary.Add(key, cell);
            }
            else
            {
                Debug.LogError($"Duplicate grid coordinates found: {key} on {cell.name}", cell);
                cellDictionary[key] = cell; // Replace instead of error
            }
        }
    }

    // Public method to get a cell by coordinates (used by PathInitializer)
    public GridCell GetCell(int x, int z)
    {
        string key = x + "," + z;
        if (cellDictionary.ContainsKey(key))
        {
            return cellDictionary[key];
        }
        return null;
    }
}