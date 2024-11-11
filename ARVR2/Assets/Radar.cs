using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radar : MonoBehaviour
{
    public Transform player;
    public float radarRange = 100.0f;
    public RectTransform radarPanel;
    public GameObject radarDotPrefab;
    public float radarScale = 1.0f;
    public RectTransform sweeper;
    public float sweepSpeed = 60.0f;
    public Image sweeperImage;

    private Dictionary<GameObject, RadarDot> enemyDots = new Dictionary<GameObject, RadarDot>();

    void Update()
    {
        // Get all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Update or create radar dots for each enemy
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(player.position, enemy.transform.position);

            // Skip enemies that are out of radar range
            if (distanceToEnemy > radarRange)
            {
                if (enemyDots.ContainsKey(enemy))
                {
                    Destroy(enemyDots[enemy].gameObject);
                    enemyDots.Remove(enemy);
                }
                continue;
            }

            if (!enemyDots.ContainsKey(enemy))
            {
                GameObject radarDot = Instantiate(radarDotPrefab, radarPanel);
                RadarDot dot = radarDot.GetComponent<RadarDot>();
                dot.InitializeDot();
                enemyDots[enemy] = dot;
            }

            Vector3 relativePos = enemy.transform.position - player.position;
            Vector3 radarPos = new Vector3(relativePos.x, 0, relativePos.z) * radarScale;
            radarPos = Quaternion.Euler(0, -player.eulerAngles.y, 0) * radarPos;
            enemyDots[enemy].GetComponent<RectTransform>().anchoredPosition = new Vector2(radarPos.x, radarPos.z);
        }

        // Rotate the sweeper
        RotateSweeper();
    }

    void RotateSweeper()
    {
        sweeperImage.transform.Rotate(0, 0, sweepSpeed * Time.deltaTime);
        Color newColor = sweeperImage.color;
        newColor.a = Mathf.PingPong(Time.time * 0.5f, 0.5f) + 0.5f;
        sweeperImage.color = newColor;
    }
}
