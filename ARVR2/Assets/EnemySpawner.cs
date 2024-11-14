using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyShipPrefab;   // Prefab for the enemy ship
    public float spawnInterval = 5f;     // Time interval between spawns
    public float minSpawnZ = 10f;        // Minimum Z position for spawning in front of the player
    public float maxSpawnDistance = 100f; // Maximum distance for spawning enemies in the forward direction
    public Transform player;             // Reference to the player's transform (submarine)
    public float offsetRange = 10f;      // The range of offset for the ship to move towards
    public float spawnAngleRange = 90f;  // Half the spawn area angle, making it 180 degrees total

    private float spawnTimer;

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
        // Generate a random angle within the forward 180 degrees of the submarine's direction along the Y-axis
        float angle = Random.Range(-spawnAngleRange, spawnAngleRange); // Angle range is between -90 and 90 degrees

        // Generate a random distance within the specified range in front of the player
        float distance = Random.Range(minSpawnZ, maxSpawnDistance);

        // Calculate the spawn position based on the distance and angle in front of the player
        Vector3 spawnPosition = player.position + player.forward * distance;

        // Apply rotation around the Y-axis for random spread within the 180° arc
        spawnPosition = Quaternion.Euler(0, angle, 0) * (spawnPosition - player.position) + player.position;

        // Ensure that the Z position is at least the minimum spawn Z distance in front of the player
        spawnPosition.z = Mathf.Max(spawnPosition.z, player.position.z + minSpawnZ);

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
}
