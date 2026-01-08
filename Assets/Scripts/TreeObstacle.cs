using UnityEngine;

public class TreeObstacle : MonoBehaviour
{
    // Trigger-based (recommended): make the tree have a Collider with 'Is Trigger' checked
    // and a kinematic Rigidbody (or ensure the moving object has a Rigidbody).
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IStoppable>(out var stoppable))
            stoppable.StopMovement();
    }

    // for physics collisions (non-trigger):
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<IStoppable>(out var stoppable))
            stoppable.StopMovement();
    }
}