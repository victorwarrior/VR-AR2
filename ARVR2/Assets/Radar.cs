using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

//Bruges til radar
public class Radar : MonoBehaviour
{
    //spillerens position
    public Transform player;
    //hvor langt raderen kan se
    public float radarRange = 100.0f;
    //Baggrunden af raderen st�relse
    public RectTransform radarPanel;
    //prefab til radar dot
    public GameObject radarDotPrefab;
    //scala p� radar
    public float radarScale = 1.0f;
    //sweeper til radar det bruges til at synlig�re dots
    public RectTransform sweeper;
    //hastighed p� sweeper
    public float sweepSpeed = 60.0f;
    //sweeper billed
    public Image sweeperImage;

    //Dictionary til at holde styr p� dots
    private Dictionary<GameObject, RadarDot> enemyDots = new Dictionary<GameObject, RadarDot>();

    void Update()
    {
        //F�r alle modstandere med tag Enemy
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Opdater eller opret dots for hver fjende
        GameManager.Instance.lowestDistanceEnemy = float.PositiveInfinity;

        foreach (GameObject enemy in enemies)
        {
            // Beregn afstanden til fjenden
            float distanceToEnemy = Vector3.Distance(player.position, enemy.transform.position);

            // Spring fjender over, der er uden for radarr�kkevidden
            if (distanceToEnemy > radarRange)
            {
                if (enemyDots.ContainsKey(enemy))
                {
                    Destroy(enemyDots[enemy].gameObject);
                    enemyDots.Remove(enemy);
                }
                continue;
            }

            // Opret en ny dot, hvis der ikke allerede findes en for denne fjende
            if (!enemyDots.ContainsKey(enemy))
            {
                GameObject radarDot = Instantiate(radarDotPrefab, radarPanel);
                RadarDot dot = radarDot.GetComponent<RadarDot>();
                dot.InitializeDot();
                enemyDots[enemy] = dot;
            }

            // Beregn den relative position af fjenden i forhold til spilleren
            Vector3 relativePos = enemy.transform.position - player.position;
            Vector3 radarPos = new Vector3(relativePos.x, 0, relativePos.z) * radarScale;

            // Afrund og m�l afstandens l�ngde
            float distance = Mathf.RoundToInt(radarPos.magnitude);

            // Opdater den korteste fjendeafstand i GameManager
            if (distance < GameManager.Instance.lowestDistanceEnemy) {
                GameManager.Instance.lowestDistanceEnemy = distance;
            }

            // Roter fjendens position i forhold til spillerens rotation
            radarPos = Quaternion.Euler(0, -player.eulerAngles.y, 0) * radarPos;

            // Opdater radarprikkens position
            enemyDots[enemy].GetComponent<RectTransform>().anchoredPosition = new Vector2(radarPos.x, radarPos.z);
        }

        // Roter radarskanneren
        RotateSweeper();
    }

    void RotateSweeper()
    {
        // Rot�r "sweeper"-billedet
        sweeperImage.transform.Rotate(0, 0, sweepSpeed * Time.deltaTime);

        // Just�r gennemsigtigheden af "sweeper"-billedet
        Color newColor = sweeperImage.color;
        newColor.a = Mathf.PingPong(Time.time * 0.5f, 0.5f) + 0.5f;
        sweeperImage.color = newColor;
    }
}
