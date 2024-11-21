using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyShipPrefab;   // Prefab for the enemy ship
    public float spawnInterval = 5f;     // Time interval between spawns
    public float minSpawnZ = 10f;        // Minimum Z position for spawning in front of the player
    public float maxSpawnDistance = 100f; // Maximum distance for spawning enemies in the forward direction
    public Transform player;             // Reference to the player's transform (submarine)
    public float offsetRange = 10f;      // The range of offset for the ship to move towards
    public float spawnAngleRange = 90f;  // Half the spawn area angle, making it 180 degrees total
    public float waterHeight = 0f;       // The water surface height in the game
    public float minSpawnDistanceBetweenShips = 20f; // Minimum distance between spawned ships

    private float spawnTimer;
    private List<Vector3> recentSpawnPositions = new List<Vector3>(); // Store recent spawn positions

    private void Start()
    {
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        // Spawn a new enemy ship when the timer reaches zero
        if (spawnTimer <= 0f)
        {
            SpawnEnemyShip();
            spawnTimer = spawnInterval; // Reset timer
        }
    }

    private void SpawnEnemyShip()
    {
        Vector3 spawnPosition;
        int attempts = 0;

        // Try to find a valid spawn position (prevent overlaps)
        do
        {
            // Generate a random angle within the forward 180 degrees of the submarine's direction along the Y-axis
            float angle = Random.Range(-spawnAngleRange, spawnAngleRange);

            // Generate a random distance within the specified range in front of the player
            float distance = Random.Range(minSpawnZ, maxSpawnDistance);

            // Calculate the spawn position based on the distance and angle in front of the player
            spawnPosition = player.position + player.forward * distance;

            // Apply rotation around the Y-axis for random spread within the 180° arc
            spawnPosition = Quaternion.Euler(0, angle, 0) * (spawnPosition - player.position) + player.position;

            // Set the spawn position's Y-coordinate to the water surface height
            spawnPosition.y = waterHeight;

            attempts++;
        } while (!IsValidSpawnPosition(spawnPosition) && attempts < 10);

        if (attempts >= 10)
        {
            Debug.LogWarning("Failed to find a valid spawn position after 10 attempts.");
            return;
        }

        // Add the new spawn position to the list of recent positions
        recentSpawnPositions.Add(spawnPosition);
        if (recentSpawnPositions.Count > 10) // Keep only the last 10 positions
        {
            recentSpawnPositions.RemoveAt(0);
        }

        // Add a random offset to the target position (near the player)
        Vector3 offset = new Vector3(Random.Range(-offsetRange, offsetRange), 0, Random.Range(-offsetRange, offsetRange));

        // Calculate the adjusted target position with the offset
        Vector3 targetPosition = player.position + offset;

        // Instantiate the enemy ship at the calculated spawn position
        GameObject enemyShip = Instantiate(enemyShipPrefab, spawnPosition, Quaternion.identity);

        // Set the ship's movement direction toward the adjusted target position (with offset)
        Vector3 moveDirection = (targetPosition - spawnPosition).normalized;
        enemyShip.GetComponent<EnemyMovement>().SetDirection(moveDirection);
    }

    private bool IsValidSpawnPosition(Vector3 position)
    {
        foreach (Vector3 recentPosition in recentSpawnPositions)
        {
            if (Vector3.Distance(position, recentPosition) < minSpawnDistanceBetweenShips)
            {
                return false; // Too close to a recently spawned ship
            }
        }
        return true;
    }
}
