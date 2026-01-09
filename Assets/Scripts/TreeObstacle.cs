using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObstacle : MonoBehaviour
{
    private Collider treeCollider;
    private Renderer[] renderers;
    private bool isBurned = false;

    void Awake()
    {
        treeCollider = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    public bool IsBurned => isBurned;

    public void BurnDown()
    {
        if (isBurned) return;
        isBurned = true;

        Debug.Log("Tree burn initiated.");

        // Start coroutine that disables visuals/collision and resumes movers
        StartCoroutine(BurnCoroutine());
    }

    private IEnumerator BurnCoroutine()
    {
        // Hide visuals first so tree is no longer visible
        foreach (var r in renderers)
            r.enabled = false;

        // Disable collider after visuals are hidden
        if (treeCollider != null)
            treeCollider.enabled = false;

        // Resume any nearby stoppable objects that may have been halted by this tree.
        float resumeRadius = 3f;
        var colliders = Physics.OverlapSphere(transform.position, resumeRadius);
        var resumed = new HashSet<IStoppable>();
        foreach (var c in colliders)
        {
            var comps = c.GetComponents<MonoBehaviour>();
            foreach (var comp in comps)
            {
                if (comp is IStoppable s && !resumed.Contains(s))
                {
                    s.StartMovement();
                    resumed.Add(s);
                }
            }
        }
        yield break;
    }

    public void Reset()
    {
        // Stop any active burn coroutine
        StopAllCoroutines();

        // Re-enable the GameObject
        gameObject.SetActive(true);

        // Re-enable visuals
        foreach (var r in renderers)
            r.enabled = true;

        // Re-enable collider
        if (treeCollider != null)
            treeCollider.enabled = true;

        // Reset burned flag
        isBurned = false;

        Debug.Log("Tree reset to initial state.");
    }
}