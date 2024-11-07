using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f;          // Speed of the enemy ship

    private Vector3 moveDirection;    // Direction the ship will move toward

    private void Update()
    {
        // Move the ship toward the player
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    // Set the movement direction for the ship
    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;
        // Face the ship in the movement direction
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
