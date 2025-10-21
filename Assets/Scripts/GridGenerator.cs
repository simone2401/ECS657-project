using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject cellPrefab; // The object to use for each cell (e.g., a Quad or Cube)
    public int width = 10;        // Number of cells across the X-axis
    public int height = 10;       // Number of cells across the Z-axis
    public float cellSize = 1f;   // The size of a single cell
    public float cellRotationX = 90f;
    public GridCell[,] gridArray;

    void Start()
    {
        gridArray = new GridCell[width, height]; // Initialize the array
        GenerateGrid();
        ColorPath();
    }

    void GenerateGrid()
    {
        if (cellPrefab == null)
        {
            Debug.LogError("Cell Prefab is not assigned!");
            return;
        }

        // Loop through the width (X-axis)
        for (int x = 0; x < width; x++)
        {
            // Loop through the height (Z-axis)
            for (int z = 0; z < height; z++)
            {
                // Calculate the position for the new cell
                // Offset by half the total size to center the grid around (0, 0, 0)
                Vector3 position = new Vector3(
                    x * cellSize - (width * cellSize * 0.5f) + (cellSize * 0.5f),
                    0f, // Keep the Y-axis flat on the plane
                    z * cellSize - (height * cellSize * 0.5f) + (cellSize * 0.5f)
                );

                Quaternion cellRotation = Quaternion.Euler(cellRotationX, 0f, 0f);

                // Instantiate the prefab at the calculated position
                GameObject newCell = Instantiate(cellPrefab, position, cellRotation);

                // Set the parent for a clean Hierarchy
                newCell.transform.parent = transform;

                // Name the object for easy identification
                newCell.name = $"Cell ({x}, {z})";

                GridCell cellScript = newCell.GetComponent<GridCell>();
                gridArray[x, z] = cellScript;
            }
        }
    }

    void ColorPath()
    {
        // Define the color for the path
        Color pathColor = Color.green;

        // Define the coordinates of the path (X, Z)

        gridArray[4, 0].SetColor(pathColor);

        gridArray[4, 1].SetColor(pathColor);

        gridArray[4, 2].SetColor(pathColor);

        gridArray[5, 2].SetColor(pathColor);

        gridArray[6, 2].SetColor(pathColor);

        gridArray[6, 3].SetColor(pathColor);

        gridArray[6, 4].SetColor(pathColor);

        gridArray[6, 5].SetColor(pathColor);

        gridArray[5, 5].SetColor(pathColor);

        gridArray[4, 5].SetColor(pathColor);

        gridArray[3, 5].SetColor(pathColor);

        gridArray[2, 5].SetColor(pathColor);

        gridArray[1, 5].SetColor(pathColor);

        gridArray[1, 6].SetColor(pathColor);

        gridArray[1, 7].SetColor(pathColor);

        gridArray[2, 7].SetColor(pathColor);

        gridArray[3, 7].SetColor(pathColor);

        gridArray[4, 7].SetColor(pathColor);

        gridArray[5, 7].SetColor(pathColor);

        gridArray[6, 7].SetColor(pathColor);

        gridArray[7, 7].SetColor(pathColor);

        gridArray[7, 8].SetColor(pathColor);

        gridArray[7, 9].SetColor(pathColor);

        // Color the start and end point differently
        gridArray[4, 0].SetColor(Color.blue); // Start (blue)
        gridArray[7, 9].SetColor(Color.red);  // End (red)
    }
}