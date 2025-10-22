using UnityEngine;

public class SwitchRotate : MonoBehaviour
{
    public Transform[] tiles; //4 grids
    private bool isRotating = false;
    public float moveSpeed = 3f;

    private Vector3[] targetPositions;

    void Start()
    {
        // Initialize the target position array
        targetPositions = new Vector3[tiles.Length];
        for (int i = 0; i < tiles.Length; i++)
        {
            targetPositions[i] = tiles[i].localPosition;
        }
    }

    void Update()
    {
        if (isRotating)
        {
            bool allDone = true;
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].localPosition = Vector3.MoveTowards(
                    tiles[i].localPosition,
                    targetPositions[i],
                    moveSpeed * Time.deltaTime
                );
                if (Vector3.Distance(tiles[i].localPosition, targetPositions[i]) > 0.01f)
                    allDone = false;
            }
            if (allDone)
                isRotating = false;
        }
    }

    private void OnMouseDown()
    {
        if (isRotating) return;

        // Move clockwise: A - B - D - C - A
        Vector3 temp = targetPositions[0];
        targetPositions[0] = targetPositions[2];
        targetPositions[2] = targetPositions[3];
        targetPositions[3] = targetPositions[1];
        targetPositions[1] = temp;

        isRotating = true;
    }
}
