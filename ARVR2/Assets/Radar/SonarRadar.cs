using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SonarRadar : MonoBehaviour
{
    [Header("Radar Settings")]
    public float radarRange = 100f; // The maximum range of the radar.
    public float detectionAngle = 180f; // The half-circle angle in degrees for radar detection.
    public float sweepSpeed = 45f; // Speed of the radar line (degrees per second).
    public LayerMask targetLayer; // Layer of enemy ships.

    [Header("Radar UI Elements")]
    public RectTransform radarLine; // The line representing radar sweep.
    public GameObject dotPrefab; // Prefab for detected dots.
    public RectTransform radarDisplay; // Parent RectTransform to hold dots on the radar.

    private float currentAngle = -90f; // Start at leftmost angle (-90 for half-circle)
    private bool sweepingRight = true; // Direction of sweep
    private List<Dot> activeDots = new List<Dot>(); // Store active dots for later fading

    void Update()
    {
        // Sweep the radar line back and forth
        if (sweepingRight)
        {
            currentAngle += sweepSpeed * Time.deltaTime;
            if (currentAngle >= 90f) // Reached rightmost angle
            {
                currentAngle = 90f;
                sweepingRight = false; // Reverse direction
            }
        }
        else
        {
            currentAngle -= sweepSpeed * Time.deltaTime;
            if (currentAngle <= -90f) // Reached leftmost angle
            {
                currentAngle = -90f;
                sweepingRight = true; // Reverse direction
            }
        }

        // Rotate the radar line in the UI
        radarLine.rotation = Quaternion.Euler(0, 0, currentAngle);

        // Detect targets in front of the submarine within the radar range
        DetectAndDisplayTargets();

        // Fade out old dots
        FadeOutDots();
    }

    void DetectAndDisplayTargets()
    {
        // Get all targets within radar range and within detection angle
        Collider[] targets = Physics.OverlapSphere(transform.position, radarRange, targetLayer);

        foreach (var target in targets)
        {
            Vector3 directionToTarget = target.transform.position - transform.position;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            // Check if the target is within the half-circle angle
            if (angleToTarget <= detectionAngle / 2f)
            {
                // Convert the world position to radar position
                Vector3 radarPos = RadarPosition(target.transform.position);

                // Check if radar line is passing over the target
                if (IsTargetInRadarSweep(target.transform.position))
                {
                    // Create a dot on the radar if it is within the sweep
                    Dot dot = Instantiate(dotPrefab, radarDisplay).GetComponent<Dot>();
                    dot.Initialize(radarPos);
                    activeDots.Add(dot);
                }
            }
        }
    }

    // Check if the radar line is currently sweeping over the target
    bool IsTargetInRadarSweep(Vector3 targetPos)
    {
        Vector3 directionToTarget = targetPos - transform.position;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        // The radar sweep is passing over the target if the angle to the target is less than the detection angle
        // and if the radar's current angle is close enough to the target's angle.
        return angleToTarget <= detectionAngle / 2f && Mathf.Abs(currentAngle - angleToTarget) < sweepSpeed * Time.deltaTime;
    }

    // Convert a world position to the radar's local position on the UI
    Vector3 RadarPosition(Vector3 targetPos)
    {
        // Calculate the direction and distance from the submarine to the target
        Vector3 offset = targetPos - transform.position;
        float distance = Mathf.Clamp(offset.magnitude, 0, radarRange);

        // Calculate the angle between the forward direction of the submarine and the target
        float angle = Mathf.Atan2(offset.z, offset.x) * Mathf.Rad2Deg;

        // Adjust the angle to fit within the radar's 180-degree field
        float radarAngle = Mathf.Clamp(angle, -detectionAngle / 2, detectionAngle / 2);

        // Map the distance to the radar's range and calculate position relative to bottom center
        float radius = (distance / radarRange) * (radarDisplay.rect.height); // Scale based on radar range

        // Convert polar coordinates (radius, angle) to Cartesian coordinates
        float x = radius * Mathf.Cos(radarAngle * Mathf.Deg2Rad);
        float y = radius * Mathf.Sin(radarAngle * Mathf.Deg2Rad);

        // Now, shift the Y position to match the radar's bottom (since radar's pivot is at (0.5, 0))
        y -= radarDisplay.rect.height / 2f; // Start from the bottom of the radar

        // Return the radar position as a Vector3 for UI anchoring
        return new Vector3(x, y, 0);
    }

    // Fade out dots gradually
    void FadeOutDots()
    {
        foreach (var dot in activeDots)
        {
            dot.Fade();
        }

        // Remove dots that are fully faded
        activeDots.RemoveAll(dot => dot.IsFaded());
    }
}
