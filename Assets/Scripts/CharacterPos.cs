using UnityEngine;

public class CartController : MonoBehaviour
{
    public Transform startPoint;
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f; // for smooth turning
    public float yOffset = 1f;

    private Transform currentRail;
    private Transform nextRail;
    private Transform previousRail; // prevent going backwards
    private bool moving = false;

    void Start()
    {
        if (startPoint == null)
            startPoint = GameObject.Find("StartRail").transform;

        transform.position = startPoint.position + Vector3.up * yOffset;

        currentRail = startPoint;
        nextRail = FindNextRail();

        moving = nextRail != null;

        //moving = false; // don't start moving straight away
    }

    void Update()
    {
        if (!moving || nextRail == null) return;

        // move toward the next rail
        Vector3 targetPos = nextRail.position + Vector3.up * yOffset;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        // rotate to face next rail smoothly
        Vector3 direction = (nextRail.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero) {
            Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }

        if (Vector3.Distance(transform.position, targetPos) < 0.01f) {
            transform.position = targetPos; // snap to center
            previousRail = currentRail;
            currentRail = nextRail;
            nextRail = FindNextRail();
            if (nextRail == null)
                moving = false;
        }
    }

    private Transform FindNextRail() {
        Vector3[] directions = { Vector3.right, Vector3.left, Vector3.forward, Vector3.back };
        float checkDistance = 1.2f;

        foreach (var dir in directions) {
            Vector3 checkPos = currentRail.position + dir * checkDistance;
            Collider[] hits = Physics.OverlapSphere(checkPos, 0.3f);

            foreach (var hit in hits) {
                if (hit.CompareTag("Rail") && hit.transform != currentRail) {
                    if (previousRail != null) {
                        // ensure cart does not go backwards
                        Vector3 moveDir = (hit.transform.position - currentRail.position).normalized;
                        Vector3 lastDir = (currentRail.position - previousRail.position).normalized;

                        // only exclude exact backwards
                        if (Vector3.Dot(moveDir, lastDir) < -0.9f) continue;
                    }
                    return hit.transform;
                }
            }
        }

        return null;
    }
    public void StartMoving() {
        moving = true;
    }
    public void StopMovement(){
        moving = nextRail = null;
        Debug.Log("Cart collided + stopped moving");
    }
}
