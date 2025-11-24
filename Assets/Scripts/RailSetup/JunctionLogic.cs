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
    readonly float arrowVerticalOffset = 1f;
    [SerializeField] float leftArrowAheadDistance = 0.2f;
    [SerializeField] float rightArrowAheadDistance = 0.2f;

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
        float arrowAheadDistance = pointLeftBranch ? leftArrowAheadDistance : rightArrowAheadDistance;
        Spline spline = splineContainer.Splines[splineIndex];

        float t = spline.ConvertIndexUnit(0, PathIndexUnit.Knot, PathIndexUnit.Normalized);
        float tAhead = Mathf.Min(t + arrowAheadDistance, 1f); // get position a little head of junction knot
        Vector3 pointAhead = splineContainer.transform.TransformPoint(spline.EvaluatePosition(tAhead));

        // Tangent along spline
        Vector3 tangentWorld = splineContainer.transform.TransformDirection(spline.EvaluateTangent(tAhead)).normalized;
        // Compute perpendicular angle in horizontal plane (Y-axis) 
        float angleY = Mathf.Atan2(tangentWorld.x, tangentWorld.z) * Mathf.Rad2Deg;

        arrow.position = pointAhead + Vector3.up * arrowVerticalOffset;
        // Set rotation: X = 90 to point down, Y = perpendicular to spline
        arrow.rotation = Quaternion.Euler(90f, angleY, 0f);
    }
}
