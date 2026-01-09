using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeObstacle : MonoBehaviour
{
    private Collider treeCollider;
    private Renderer[] renderers;
    private bool isBurned = false;

    [Header("Burn Settings")]
    public float burnDuration = 1.5f; // how long before the tree visually disappears
    public GameObject burnVFXPrefab;  // optional VFX prefab

    void Awake()
    {
        treeCollider = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    public bool IsBurned => isBurned;

    // stop movers that hit the tree
    void OnTriggerEnter(Collider other)
    {
        if (isBurned) return;
        StopStoppableOnCollider(other);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isBurned) return;
        StopStoppableOnCollider(collision.collider);
    }

    void StopStoppableOnCollider(Collider col)
    {
        // iterate all MonoBehaviour components and check for the interface
        var comps = col.GetComponents<MonoBehaviour>();
        foreach (var comp in comps)
        {
            if (comp is IStoppable stoppable)
            {
                stoppable.StopMovement();
            }
        }
    }

    public void BurnDown()
    {
        if (isBurned) return;
        isBurned = true;

        Debug.Log("Tree burn initiated: playing VFX and scheduling removal.");

        // Spawn VFX if provided
        if (burnVFXPrefab != null)
        {
            Instantiate(burnVFXPrefab, transform.position, Quaternion.identity, transform);
        }

        // Start coroutine that waits burnDuration, then disable visuals/collision and resume movers
        StartCoroutine(BurnCoroutine());
    }

    private IEnumerator BurnCoroutine()
    {
        // wait for burn animation/VFX to play
        yield return new WaitForSeconds(burnDuration);

        // Hide visuals first so tree is no longer visible
        foreach (var r in renderers)
            r.enabled = false;

        // Disable collider after visuals are hidden so actors aren't allowed through while the tree still looks present
        if (treeCollider != null)
            treeCollider.enabled = false;

        // Resume any nearby stoppable objects that may have been halted by this tree.
        float resumeRadius = 3f;
        var colliders = Physics.OverlapSphere(transform.position, resumeRadius);
        var resumed = new HashSet<IStoppable>();
        foreach (var c in colliders)
        {
            // check all MonoBehaviours on the collider's root object and pick those implementing the interface
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

        // Finally, destroy this gameobject after a short delay to keep scene clean
        Destroy(gameObject, 0.5f);
    }
}