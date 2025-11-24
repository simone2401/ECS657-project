using System;
using UnityEngine;
using UnityEngine.Splines;

public class JunctionController : MonoBehaviour
{
    [Header("Spline Settings")]
    [SerializeField] SplineContainer splineContainer;
    [SerializeField] int leftSplineIndex = 0;
    [SerializeField] int rightSplineIndex = 1;

    [Header("Objects")]
    [SerializeField] Transform arrow;
    [SerializeField] Transform lever;
    [SerializeField] CartController cartController;

    [Header("Computation Values")]
    [SerializeField] float leftAddedRotation = 0f;
    [SerializeField] float rightAddedRotation = 0f;

    [Header("Direction")]
    [SerializeField] bool pointLeftBranch = true;

    void Start()
    {
        UpdateArrow();
        // disable right slice at start
        cartController.toggleSlice(rightSplineIndex);
        cartController.RefreshPath();
        Debug.Log($"end of start in junction logic");
    }

    // Toggle when lever clicked
    public void ToggleJunction()
    {
        pointLeftBranch = !pointLeftBranch;
        UpdateArrow();
        UpdateLever();
    }

    private void UpdateLever()
    {
        if (lever == null) return;

        // flip 180° on Y when switching
        if (pointLeftBranch)
            lever.localRotation *= Quaternion.Euler(0f, 180f, 0f);
        else
            lever.localRotation *= Quaternion.Euler(0f, -180f, 0f);
        cartController.toggleSlice(leftSplineIndex);
        cartController.toggleSlice(rightSplineIndex);
        cartController.RefreshPath();
    }

    public void UpdateArrow()
    {
        if (splineContainer == null || arrow == null) return;

        int splineIndex = pointLeftBranch ? leftSplineIndex : rightSplineIndex;
        float addedRotation = pointLeftBranch ? leftAddedRotation : rightAddedRotation;
        Spline spline = splineContainer.Splines[splineIndex];

        // Tangent along spline a little ahead (0.2f) of junction
        Vector3 tangentWorld = splineContainer.transform.TransformDirection(spline.EvaluateTangent(0.2f)).normalized;
        float angleY = (Mathf.Atan2(tangentWorld.x, tangentWorld.z) * Mathf.Rad2Deg) + addedRotation;
        arrow.rotation = Quaternion.Euler(0f, angleY, 0f);
    }
}
