using UnityEngine;

public class WitchAbility : MonoBehaviour
{
    [Header("Fireball Settings")]
    public GameObject fireballPrefab;
    public Transform firePoint;        // tip of the wand
    public float fireballSpeed = 12f;
    public float fireRate = 1f;        // shots per second
    public float detectionRange = 10f;

    private float fireCooldown = 0f;
    private GameObject currentFireball = null; // track active fireball

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        TreeObstacle targetTree = FindNearestTree();
        if (targetTree != null)
        {
            // Only fire if within range, cooldown finished, and no active fireball
            float dist = Vector3.Distance(firePoint.position, targetTree.transform.position);
            if (dist <= detectionRange && fireCooldown <= 0f && currentFireball == null)
            {
                ShootAt(targetTree);
                fireCooldown = 1f / fireRate;
            }
        }
    }

    void ShootAt(TreeObstacle tree)
    {
        // Aim at the center of the tree collider
        Collider treeCollider = tree.GetComponent<Collider>();
        Vector3 targetPos = tree.transform.position;
        if (treeCollider != null)
        {
            targetPos += Vector3.up * (treeCollider.bounds.size.y / 2f);
        }

        Vector3 direction = (targetPos - firePoint.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);

        // spawn a little in front to avoid overlap with the witch
        Vector3 spawnPos = firePoint.position + direction * 0.25f;
        GameObject fireball = Instantiate(fireballPrefab, spawnPos, rotation);

        // Ignore collision with the witch
        Collider fireballCollider = fireball.GetComponent<Collider>();
        Collider witchCollider = GetComponent<Collider>();
        if (fireballCollider != null && witchCollider != null)
        {
            Physics.IgnoreCollision(fireballCollider, witchCollider);
        }

        // Set speed and track the active fireball
        FireballProjectile projectile = fireball.GetComponent<FireballProjectile>();
        if (projectile != null)
        {
            projectile.speed = fireballSpeed;
            projectile.onDestroy = () => currentFireball = null;
        }

        currentFireball = fireball;
    }

    TreeObstacle FindNearestTree()
    {
        TreeObstacle[] trees = Object.FindObjectsByType<TreeObstacle>(FindObjectsSortMode.None);
        TreeObstacle closest = null;
        float minDist = Mathf.Infinity;

        foreach (var tree in trees)
        {
            if (tree == null) continue;
            if (tree.IsBurned) continue; // skip trees already burning

            float dist = Vector3.Distance(firePoint.position, tree.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = tree;
            }
        }

        return closest;
    }
}