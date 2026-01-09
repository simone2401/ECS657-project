using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Base class for obstacles; can be extended for specific obstacle types
    
    // Trigger-based (recommended): make the tree have a Collider with 'Is Trigger' checked
    // and a kinematic Rigidbody (or ensure the moving object has a Rigidbody).
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IStoppable>(out var stoppable))
            stoppable.StopMovement();
    }

    // If you prefer physics collisions (non-trigger), use this:
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<IStoppable>(out var stoppable))
            stoppable.StopMovement();
    }
}