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

    void OnTriggerEnter(Collider other)
    {
        if (isBurned) return;

        if (other.TryGetComponent<IStoppable>(out var stoppable))
        {
            stoppable.StopMovement();
            // Track stopped movers by attaching them to this tree if needed (optional)
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isBurned) return;

        if (collision.collider.TryGetComponent<IStoppable>(out var stoppable))
        {
            stoppable.StopMovement();
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

        // Start coroutine that waits burnDuration, then disable collision/visuals and resume movers
        StartCoroutine(BurnCoroutine());
    }

    private IEnumerator BurnCoroutine()
    {
        // Optionally play burn animation / VFX here
        yield return new WaitForSeconds(burnDuration);

        // Disable collider so things can pass
        if (treeCollider != null)
            treeCollider.enabled = false;

        // Hide visuals (disable renderers)
        foreach (var r in renderers)
            r.enabled = false;

        // Resume any nearby stoppable objects that may have been halted by this tree.
        // We search a small radius around the tree center; adjust radius as needed.
        float resumeRadius = 3f;
        var colliders = Physics.OverlapSphere(transform.position, resumeRadius);
        var resumed = new HashSet<IStoppable>();
        foreach (var c in colliders)
        {
            if (c.TryGetComponent<IStoppable>(out var s))
            {
                if (!resumed.Contains(s))
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