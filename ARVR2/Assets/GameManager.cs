using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float lowestDistanceEnemy = float.PositiveInfinity;

    public int soundLvlFromPlayer;
    public int pot_Speed_Value;
    public int button_1;
    public int button_2;
    public int button_3;



    private void Awake()
    {
        // Ensure singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
