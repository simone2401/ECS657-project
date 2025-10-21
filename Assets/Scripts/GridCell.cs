using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    private Renderer cellRenderer;
    private Color originalColor;

    void Awake()
    {
        // Get the Renderer component once when the cell is created
        cellRenderer = GetComponent<Renderer>();

        // Store the initial color (assuming you've applied a base material)
        // Use .material because want an instance specific to this cell
        originalColor = cellRenderer.material.color;
    }

    // Public method to change the color
    public void SetColor(Color newColor)
    {
        // Assign the new color to the material instance
        cellRenderer.material.color = newColor;
    }

    // Public method to reset the color
    public void ResetColor()
    {
        cellRenderer.material.color = originalColor;
    }
}