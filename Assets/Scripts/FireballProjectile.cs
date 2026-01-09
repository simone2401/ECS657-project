using UnityEngine;
using System;

public class FireballProjectile : MonoBehaviour
{
    public float speed = 3f;            // starting speed
    public float acceleration = 35f;      // units/sec^2, set >0 to accelerate
    public float maxSpeed = 0f;          // 0 = no cap
    public float maxDistance = 5f;
    public float lifetime = 5f;

    private float distanceTraveled = 0f;
    private float currentSpeed;

    public Action onDestroy;

    void Start()
    {
        currentSpeed = speed;
        // Safety destroy
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (acceleration != 0f)
        {
            currentSpeed += acceleration * Time.deltaTime;
            if (maxSpeed > 0f)
                currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        }

        float moveStep = currentSpeed * Time.deltaTime;
        transform.position += transform.forward * moveStep;

        distanceTraveled += moveStep;
        if (distanceTraveled >= maxDistance)
        {
            onDestroy?.Invoke();
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<TreeObstacle>(out var tree))
        {
            tree.BurnDown();
        }

        onDestroy?.Invoke();
        Destroy(gameObject);
    }
}
