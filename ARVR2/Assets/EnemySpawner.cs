using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//bruges til at spawne fjentlige skibe
public class EnemySpawner : MonoBehaviour
{
    //Prefab til skibet der skal spawnes
    public GameObject enemyShipPrefab;   
    //interval mellem spawns
    public float spawnInterval = 5f;     
    //Hvor langt fra spilleren den minimum skal spawn
    public float minSpawnZ = 10f;
    //Hvor langt fra spilleren den maximum skal spawn
    public float maxSpawnDistance = 100f; 
    //reference til spillerens position
    public Transform player;    
    //hvor langt offset der skal v�re til spilleren
    public float offsetRange = 10f;     
    //N�r man tager ub�dens front vil de kune spawne med 90 grader til venstre og h�jre
    public float spawnAngleRange = 90f;  
    //Hvad h�jden er p� vandet i spillet
    public float waterHeight = 0f;      
    //hvad er minimum distancen fra skibe der er spawnet
    public float minSpawnDistanceBetweenShips = 20f; 


    private float spawnTimer;
    //Gemmer de seneste spawnede skibe
    private List<Vector3> recentSpawnPositions = new List<Vector3>(); 

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            //spawn timer
            spawnTimer = spawnInterval;

        }

    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "MainScene")
        {
            spawnTimer -= Time.deltaTime;

            //Den spawner og reseter
            // Spawn a new enemy ship when the timer reaches zero
            if (spawnTimer <= 0f)
            {
                SpawnEnemyShip();
                spawnTimer = spawnInterval; // Reset timer
            }
        }

    }


    //spawner skibet
    private void SpawnEnemyShip()
    {
        Vector3 spawnPosition;
        int attempts = 0;

        //pr�ver at finde et sted at spawn
        do
        {
            // Generer en tilf�ldig vinkel inden for de forreste 180 grader
            float angle = Random.Range(-spawnAngleRange, spawnAngleRange);

            // Generer en tilf�ldig afstand inden for det angivne interval foran spilleren
            float distance = Random.Range(minSpawnZ, maxSpawnDistance);

            //regner spawn�positionen baseret p� afstanden og vinklen foran spilleren
            spawnPosition = player.position + player.forward * distance;

            // Anvend rotation omkring Y-aksen for at sprede spawns tilf�ldigt i en 180� bue
            spawnPosition = Quaternion.Euler(0, angle, 0) * (spawnPosition - player.position) + player.position;

            // S�t spawn positionens Y-koordinat til vandoverfladens h�jde
            spawnPosition.y = waterHeight;

            // Gentag, indtil en gyldig spawn-position er fundet, eller maksimum antal fors�g er n�et
            attempts++;
        } while (!IsValidSpawnPosition(spawnPosition) && attempts < 10);

        // Tjek om vi har overskredet maksimum fors�g
        if (attempts >= 10)
        {
            Debug.LogWarning("Failed to find a valid spawn position after 10 attempts.");
            return;
        }

        // Tilf�j den nye spawn-position til listen over nylige positioner
        recentSpawnPositions.Add(spawnPosition);
        // Behold kun de sidste 10 positioner
        if (recentSpawnPositions.Count > 10)
        {
            recentSpawnPositions.RemoveAt(0);
        }

        // Tilf�j en tilf�ldig forskydning til spilleren
        Vector3 offset = new Vector3(Random.Range(-offsetRange, offsetRange), 0, Random.Range(-offsetRange, offsetRange));

        // Beregn den position med forskydningen
        Vector3 targetPosition = player.position + offset;

        // Instantiate fjendes med spawn position
        GameObject enemyShip = Instantiate(enemyShipPrefab, spawnPosition, Quaternion.identity);

        // S�t skibets bev�gelsesretning mod spilleren
        Vector3 moveDirection = (targetPosition - spawnPosition).normalized;
        enemyShip.GetComponent<EnemyMovement>().SetDirection(moveDirection);
    }

    // Tjek om en given spawn-position er gyldig
    private bool IsValidSpawnPosition(Vector3 position)
    {
        // Gennemg� nylige positioner og tjek om den er for t�t p� et tidligere spawn
        foreach (Vector3 recentPosition in recentSpawnPositions)
        {

            if (Vector3.Distance(position, recentPosition) < minSpawnDistanceBetweenShips)
            {
                return false; // For t�t p� et nyligt spawnet skib
            }
        }
        // Positionen er gyldig
        return true;
    }
}
