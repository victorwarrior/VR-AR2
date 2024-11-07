using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SonarRadar : MonoBehaviour
{
    [Header("Radar Settings")]
    public float radarRange = 100f; // The maximum range of the radar.
    public float detectionAngle = 180f; // The full 180° detection cone.
    public float sweepSpeed = 45f; // Speed of the radar line (degrees per second).
    public LayerMask targetLayer; // Layer of enemy ships.

    [Header("Radar UI Elements")]
    public RectTransform radarLine; // The line representing radar sweep.
    public GameObject dotPrefab; // Prefab for detected dots.
    public RectTransform radarDisplay; // Parent RectTransform to hold dots on the radar.

    private float currentAngle = -90f; // Start at leftmost angle (-90 for half-circle).
    private bool sweepingRight = true; // Direction of sweep.
    private List<Dot> activeDots = new List<Dot>(); // Store active dots for later fading.

    void Update()
    {
        // Sweep the radar line back and forth.
        if (sweepingRight)
        {
            currentAngle += sweepSpeed * Time.deltaTime;
            if (currentAngle >= 90f) // Reached rightmost angle.
            {
                currentAngle = 90f;
                sweepingRight = false; // Reverse direction.
            }
        }
        else
        {
            currentAngle -= sweepSpeed * Time.deltaTime;
            if (currentAngle <= -90f) // Reached leftmost angle.
            {
                currentAngle = -90f;
                sweepingRight = true; // Reverse direction.
            }
        }

        // Rotate the radar line in the UI.
        radarLine.rotation = Quaternion.Euler(0, 0, currentAngle);

        // Detect targets in front of the submarine within the radar range.
        DetectAndDisplayTargets();

        // Fade out old dots.
        FadeOutDots();
    }

    void DetectAndDisplayTargets()
    {
        // Get all targets within radar range and within detection angle.
        Collider[] targets = Physics.OverlapSphere(transform.position, radarRange, targetLayer);

        foreach (var target in targets)
        {
            Vector3 directionToTarget = target.transform.position - transform.position;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            // Check if the target is within the full detection angle (180 degrees total).
            if (angleToTarget <= detectionAngle / 2f)
            {
                // Convert the world position to radar position.
                Vector3 radarPos = WorldToRadarPosition(target.transform.position);

                // Check if radar line is passing over the target (i.e., within the sweep).
                if (IsPointInRadarSweep(target.transform.position))
                {
                    // Create a dot on the radar if it is within the sweep.
                    Dot dot = Instantiate(dotPrefab, radarDisplay).GetComponent<Dot>();
                    dot.Initialize(radarPos); // Set the dot's position on the radar.
                    activeDots.Add(dot);
                }
            }
        }
    }

    bool IsPointInRadarSweep(Vector3 targetPos)
    {
        // Calculate the angle between the radar's forward direction and the target direction.
        Vector3 directionToTarget = targetPos - transform.position;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget); // Angle from radar's forward to target.

        // Check if the angle is within the radar's detection cone (half of the detection angle).
        return angleToTarget <= detectionAngle / 2f;
    }

    Vector3 WorldToRadarPosition(Vector3 worldPos)
    {
        // Calculate the direction and distance from the radar's center (assumed to be (0,0,0)).
        Vector3 offset = worldPos - transform.position; // Subtract radar's position (transform position).
        float distance = Mathf.Clamp(offset.magnitude, 0, radarRange); // Clamp distance to radar's range.

        // Calculate the angle of the target relative to the radar's forward direction.
        float angle = Mathf.Atan2(offset.z, offset.x) * Mathf.Rad2Deg;

        // Adjust the angle to fit within the radar's range, ensuring it's in the correct format.
        float radarAngle = Mathf.Repeat(angle + 90f, 360f) - 90f; // Converts angle to range [-90, 90].

        // Calculate the radius based on distance and radar range.
        float radius = (distance / radarRange) * radarDisplay.rect.height;

        // Convert the angle and radius to Cartesian coordinates (X and Y for the radar).
        float x = radius * Mathf.Cos(radarAngle * Mathf.Deg2Rad);
        float y = radius * Mathf.Sin(radarAngle * Mathf.Deg2Rad);
        y -= radarDisplay.rect.height / 2f; // Adjust for radar's pivot point (centered at bottom).

        return new Vector3(x, y, 0); // Return as radar's local position.
    }

    void FadeOutDots()
    {
        foreach (var dot in activeDots)
        {
            dot.Fade(); // Fade out each dot.
        }

        // Remove dots that are fully faded.
        activeDots.RemoveAll(dot => dot.IsFaded());
    }
}
