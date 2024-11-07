using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 5f;               // Movement speed
    public float patrolRange = 20f;        // Range within which the ship will patrol
    public float waypointRadius = 1f;      // How close to the waypoint the ship needs to be before changing destination

    private Vector3 targetWaypoint;

    private void Start()
    {
        SetRandomWaypoint(); // Set initial waypoint
    }

    private void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        // Move towards the target waypoint
        Vector3 direction = (targetWaypoint - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Check if we're close enough to the waypoint to change to a new one
        if (Vector3.Distance(transform.position, targetWaypoint) < waypointRadius)
        {
            SetRandomWaypoint();
        }

        // Face the direction of movement
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        }
    }

    private void SetRandomWaypoint()
    {
        // Pick a new random point within the patrol range
        Vector3 randomPoint = Random.insideUnitSphere * patrolRange;
        randomPoint.y = 0f; // Ensure the waypoint is on the water surface

        targetWaypoint = transform.position + randomPoint;
    }
}
