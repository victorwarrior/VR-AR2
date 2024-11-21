using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;          // Speed of the enemy ship
    public float waterHeight = 0f;    // The height of the water surface

    private Vector3 moveDirection;    // Direction the ship will move toward

    private void Update()
    {
        // Ensure the ship stays at the water height
        Vector3 currentPosition = transform.position;
        currentPosition.y = waterHeight;  // Keep the ship on the water surface
        transform.position = currentPosition;

        // Move the ship toward the player (no change to the Y-coordinate)
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    // Set the movement direction for the ship
    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;

        // Ensure the ship faces the movement direction but keeps its Y-coordinate at the water level
        Vector3 directionWithoutY = new Vector3(direction.x, 0, direction.z); // Remove Y-component
        transform.rotation = Quaternion.LookRotation(directionWithoutY);
    }
}
