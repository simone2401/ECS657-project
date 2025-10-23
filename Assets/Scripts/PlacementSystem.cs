using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject cellIndicator;

    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private float raycastHeight = 5f;

    [SerializeField]
    private LayerMask groundMask = ~0;

    private void Update()
    {
        Vector3 mousePosition = inputManager.GetWorldPositionFromMouse();
        Vector3Int cellPosition = grid.WorldToCell(mousePosition);

        Vector3 cellWorld = grid.CellToWorld(cellPosition);

        // Start the raycast from above the cell and cast downward to find the plane surface
        Vector3 rayStart = cellWorld + Vector3.up * raycastHeight;
        float maxDistance = raycastHeight * 2f;
        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, maxDistance, groundMask))
        {
            cellIndicator.transform.position = hit.point;
        }
        else
        {
            // fallback: place at cell world position (preserve grid placement if no ground found)
            cellIndicator.transform.position = cellWorld;
        }
    }
}
