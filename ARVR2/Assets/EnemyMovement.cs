using UnityEngine;

//krigskib script der styger deres movement
public class EnemyMovement : MonoBehaviour
{
    //hastighed og lengde/dybte p� vandet
    public float speed = 5f;          
    public float waterHeight = 0f;    

    //spillerens position
    private Vector3 moveDirection;   

    private void Update()
    {
        //s� den forbliver p� vand overfladen
        Vector3 currentPosition = transform.position;
        currentPosition.y = waterHeight;  
        transform.position = currentPosition;

        // bev�ger sig mod spilleren
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    //en anden klasse kaldet EnemySpawner kalder funktion n�r et skib bliver spawned ind og setter dens kurs 
    public void SetDirection(Vector3 direction)
    {
        moveDirection = direction;

        // Ensure the ship faces the movement direction but keeps its Y-coordinate at the water level
        Vector3 directionWithoutY = new Vector3(direction.x, 0, direction.z); // Remove Y-component
        transform.rotation = Quaternion.LookRotation(directionWithoutY);
    }
}
