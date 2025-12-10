using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

[System.Serializable]
public class SplinePathData
{
    public SliceData[] slices;
}

[System.Serializable]
public class SliceData
{
    public int splineIndex;
    public SplineRange range;
    public bool isEnabled;
}

public class CartController : MonoBehaviour
{
    [SerializeField] SplineContainer splineContainer;
    [SerializeField] float speed = 5f;
    [SerializeField] int startSlice = 0;
    [SerializeField] SplinePathData pathData;

    SplinePath path;
    float distanceTraveled = 0f;
    float pathLength = 0f;

    void Awake()
    {
        AutoGenerateSlices();
        RefreshPath();
    }

    void Start()
    {
        if (splineContainer == null || splineContainer.Splines.Count == 0)
        {
            Debug.LogError("SplineContainer is not assigned or has no splines.");
            return;
        }

        StartCoroutine(FollowCoroutine());
    }

    public void RefreshPath()
    {
        path = new SplinePath(CalculatePath());
        pathLength = path.GetLength();
    }

    public void toggleSlice(int sliceIndex)
    {
        if (pathData == null || pathData.slices == null || sliceIndex < 0 || sliceIndex >= pathData.slices.Length)
        {
            Debug.LogWarning("Invalid slice index or pathData not initialized.");
            return;
        }

        pathData.slices[sliceIndex].isEnabled = !pathData.slices[sliceIndex].isEnabled;
    }

    private void AutoGenerateSlices()
    {
        // if already set in inspector, do nothing
        if (pathData != null && pathData.slices != null && pathData.slices.Length > 0)
            return;

        int splineCount = splineContainer.Splines.Count;

        pathData = new SplinePathData();
        pathData.slices = new SliceData[splineCount];

        for (int i = 0; i < splineCount; i++)
        {
            var spline = splineContainer.Splines[i];
            int knotCount = spline.Knots.Count();

            pathData.slices[i] = new SliceData
            {
                splineIndex = i,
                range = new SplineRange(0, knotCount),
                isEnabled = true
            };
        }

        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"Auto-generated {splineCount} slices:");
        for (int i = 0; i < pathData.slices.Length; i++)
        {
            var s = pathData.slices[i];
            sb.AppendLine($"[{i}] splineIndex={s.splineIndex}, range={s.range}, isEnabled={s.isEnabled}");
        }
        Debug.Log(sb.ToString());
    }

    private List<SplineSlice<Spline>> CalculatePath()
    {
        var slices = new List<SplineSlice<Spline>>();
        var visited = new HashSet<int>(); // track visited splines to prevent loops

        int currentIndex = startSlice;

        while (currentIndex != -1 && !visited.Contains(currentIndex))
        {
            visited.Add(currentIndex);
            slices.Add(CreateSlice(currentIndex));

            // find next connected enabled spline
            currentIndex = FindConnectedEnabledSpline(currentIndex);
        }

        return slices;
    }

    private SplineSlice<Spline> CreateSlice(int splineIndex)
    {
        var spline = splineContainer.Splines[splineIndex];
        return new SplineSlice<Spline>(spline, pathData.slices[splineIndex].range,
            splineContainer.transform.localToWorldMatrix);
    }

    private int FindConnectedEnabledSpline(int currentIndex)
    {
        var currentSpline = splineContainer.Splines[currentIndex];
        Vector3 endPos = splineContainer.transform.TransformPoint(
            currentSpline.EvaluatePosition(1f));

        for (int i = 0; i < pathData.slices.Length; i++)
        {
            // skip disabled slices and self
            if (!pathData.slices[i].isEnabled || i == currentIndex)
                continue;

            var spline = splineContainer.Splines[i];
            Vector3 startPos = splineContainer.transform.TransformPoint(
                spline.EvaluatePosition(0f));

            // exact match because knots are linked
            if (endPos == startPos)
                return i;
        }

        return -1;
    }


    IEnumerator FollowCoroutine()
    {
        while (true)
        {
            distanceTraveled += speed * Time.deltaTime;

            // loop or stop
            if (distanceTraveled > pathLength)
                distanceTraveled = 0f;

            float t = distanceTraveled / pathLength;

            var pos = path.EvaluatePosition(t);
            var dir = path.EvaluateTangent(t);

            transform.position = pos;
            transform.LookAt(pos + dir);

            yield return null;
        }
    }

    public void StopMovement()
    {
        // Stop the coroutine that drives the cart
        StopAllCoroutines();
        Debug.Log("Cart movement stopped by SpikeTrigger.");
    }
}