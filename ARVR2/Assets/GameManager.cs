using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float lowestDistanceEnemy = float.PositiveInfinity;

    public int soundLvlFromPlayer;
    public Camera playerCameraGameobject;

    public int pot_Speed_Value;
    public int button_1;
    public int button_2;
    public int button_3;

    public int[] lvlTresholds = {100, 75,50,25,10};


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        if (scene.name == "MainScene")
        {
            playerCameraGameobject = FindFirstObjectByType<Camera>();
        }
    }

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

    private int previousButton1State;
    private int previousButton2State;
    private int previousButton3State;
    private bool DetectButtonStateChange()
    {
        if (button_1 != previousButton1State || button_2 != previousButton2State || button_3 != previousButton3State)
        {
            previousButton1State = button_1;
            previousButton2State = button_2;
            previousButton3State = button_3;

            return true; 
        }

        return false; 
    }


    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "LostScene")
        {
            if (DetectButtonStateChange() == true)
            {
                SceneManager.LoadScene("MainScene");
            }
        }

        if (SceneManager.GetActiveScene().name == "MainScene")
        {

            int lvlChoosen = 0;


            if (lowestDistanceEnemy >= lvlTresholds[0])
            {
                lvlChoosen = 5;
            }
            else if (lowestDistanceEnemy >= lvlTresholds[1])
            {
                lvlChoosen = 4;
            }
            else if (lowestDistanceEnemy >= lvlTresholds[2])
            {
                lvlChoosen = 3;
            }
            else if (lowestDistanceEnemy >= lvlTresholds[3])
            {
                lvlChoosen = 2;
            }
            else if (lowestDistanceEnemy <= lvlTresholds[4])
            {
                lvlChoosen = 1;
            }

            int totalSoundInfluence = soundLvlFromPlayer + button_1 + button_2 + button_3;


            switch (lvlChoosen)
            {
                case 1:
                    if (totalSoundInfluence >= 0 || pot_Speed_Value >= 50)
                    {
                        SceneManager.LoadScene("LostScene");
                    }
                    break;
                case 2:
                    if (totalSoundInfluence >= 1 || pot_Speed_Value >= 100)
                    {
                        SceneManager.LoadScene("LostScene");
                    }
                    break;
                case 3:
                    if (totalSoundInfluence >= 2 || pot_Speed_Value >= 250)
                    {
                        SceneManager.LoadScene("LostScene");
                    }
                    break;
                case 4:
                    if (totalSoundInfluence >= 3 || pot_Speed_Value >= 500)
                    {
                        SceneManager.LoadScene("LostScene");
                    }
                    break;
                case 5:
                    if (totalSoundInfluence >= 4 || pot_Speed_Value >= 1000)
                    {
                        SceneManager.LoadScene("LostScene");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
