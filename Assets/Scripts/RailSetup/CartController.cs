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
    [SerializeField] float speed = 0.8f;
    [SerializeField] SplinePathData pathData;

    SplinePath path;
    float distanceTraveled = 0f;
    float pathLength = 0f;

    void Start()
    {
        if (splineContainer == null || splineContainer.Splines.Count == 0)
        {
            Debug.LogError("SplineContainer is not assigned or has no splines.");
            return;
        }

        RefreshPath();

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

    private List<SplineSlice<Spline>> CalculatePath()
    {
        var localToWorldMatrix = splineContainer.transform.localToWorldMatrix;
        var enabledSlices = pathData.slices.Where(slice => slice.isEnabled).ToList();
        var slices = new List<SplineSlice<Spline>>();

        foreach (var sliceData in enabledSlices)
        {
            var spline = splineContainer.Splines[sliceData.splineIndex];
            var slice = new SplineSlice<Spline>(spline, sliceData.range, localToWorldMatrix);
            slices.Add(slice);
        }

        return slices;
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
}