using UnityEngine;
using UnityEngine.Splines;
using System.Linq;

public class JunctionController : MonoBehaviour
{
    [Header("Spline Settings")]
    public SplineContainer splineContainer;
    public int leftSplineIndex = 0;
    public int rightSplineIndex = 1;
    public int leftJunctionKnotIndex = 0;
    public int rightJunctionKnotIndex = 0;

    [Header("Junction Objects")]
    public Transform arrow;
    public Transform lever;

    [Header("Computation Values")]
    private readonly float arrowVerticalOffset = 1f;
    public float leftArrowAheadDistance = 0.2f;
    public float rightArrowAheadDistance = 0.2f;

    [Header("Direction")]
    public bool pointLeftBranch = true;

    void Start()
    {
        UpdateArrow();
        UpdateLever();
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
    }

    public void UpdateArrow()
    {
        if (splineContainer == null || arrow == null) return;

        (int splineIndex, int junctionKnotIndex, float arrowAheadDistance) =
            pointLeftBranch
                ? (leftSplineIndex, leftJunctionKnotIndex, leftArrowAheadDistance)
                : (rightSplineIndex, rightJunctionKnotIndex, rightArrowAheadDistance);
        Spline spline = splineContainer.Splines[splineIndex];

        float t = spline.ConvertIndexUnit(junctionKnotIndex, PathIndexUnit.Knot, PathIndexUnit.Normalized);
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
