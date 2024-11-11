using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radar : MonoBehaviour
{
    public Transform player;                  // Reference to the player�s transform (U-boat)
    public float radarRange = 100.0f;         // Maximum range for the radar (distance in world units)
    public RectTransform radarPanel;          // Reference to the Radar UI panel (should be a square/circle image)
    public GameObject radarDotPrefab;         // Prefab for radar dots
    public float radarScale = 1.0f;           // Scale factor for the radar display

    private List<GameObject> radarDots = new List<GameObject>(); // Holds instantiated radar dots

    void Update()
    {
        // Clear existing radar dots to refresh positions
        foreach (var dot in radarDots)
            Destroy(dot);
        radarDots.Clear();

        // Find all objects tagged as "Enemy" (make sure enemy ships have this tag)
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(player.position, enemy.transform.position);

            // Skip enemies that are out of radar range
            if (distanceToEnemy > radarRange)
                continue;

            // Create a new radar dot
            GameObject radarDot = Instantiate(radarDotPrefab, radarPanel);
            radarDots.Add(radarDot);

            // Calculate enemy position relative to the player
            Vector3 relativePos = enemy.transform.position - player.position;

            // Calculate the position on the radar using the player's forward as the radar's "up"
            Vector3 radarPos = new Vector3(relativePos.x, 0, relativePos.z) * radarScale;

            // Rotate according to player rotation
            radarPos = Quaternion.Euler(0, -player.eulerAngles.y, 0) * radarPos;

            // Convert to UI space
            radarDot.GetComponent<RectTransform>().anchoredPosition = new Vector2(radarPos.x, radarPos.z);
        }
    }
}
