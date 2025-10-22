using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    private Renderer cellRenderer;
    private Material cellMaterialInstance;
    private Color originalColor;

    // Public data fields for identification in the path initializer
    public int gridX = -1;
    public int gridZ = -1;

    void Awake()
    {
        cellRenderer = GetComponent<Renderer>();

        if (cellRenderer != null)
        {
            // Get an instance of the material so changes only affect this cell
            cellMaterialInstance = cellRenderer.material;
            originalColor = cellMaterialInstance.color;
        }
    }

    // Public method to change the color
    public void SetColor(Color newColor)
    {
        if (cellMaterialInstance != null)
        {
            cellMaterialInstance.color = newColor;
        }
    }

    // Public method to reset the color
    public void ResetColor()
    {
        if (cellMaterialInstance != null)
        {
            cellMaterialInstance.color = originalColor;
        }
    }
}