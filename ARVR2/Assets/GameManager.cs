using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [HideInInspector]
    public Uboat uboat;

    public float lowestDistanceEnemy = float.PositiveInfinity;

    public int soundLvlFromPlayer;
    public Camera playerCameraGameobject;

    public int pot_Speed_Value;
    public int button_1;
    public int button_2;
    public int button_3;

    public int[] lvlTresholds = { 100, 75, 50, 25, 10 };
    public int lvlWinRange = 100;

    public AudioSource backgroundMusic;

    private bool sceneChangeRequested = false;

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
        // Reset the scene change flag when a new scene is loaded
        if (scene.name == "LostScene" || scene.name == "WinScene")
        {
            sceneChangeRequested = false;
        }

        if (scene.name == "MainScene")
        {
            uboat = FindFirstObjectByType<Uboat>();
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

    public void ChangeScene()
    {
        if (DetectButtonStateChange())
        {
            // Allow the scene change only if we're in a "Win" or "Lost" scene
            if (SceneManager.GetActiveScene().name == "LostScene" || SceneManager.GetActiveScene().name == "WinScene")
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    public void CheckIfLoss()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            int lvlChoosen = 0;

            if (lowestDistanceEnemy >= lvlTresholds[0]) lvlChoosen = 5;
            else if (lowestDistanceEnemy >= lvlTresholds[1]) lvlChoosen = 4;
            else if (lowestDistanceEnemy >= lvlTresholds[2]) lvlChoosen = 3;
            else if (lowestDistanceEnemy >= lvlTresholds[3]) lvlChoosen = 2;
            else if (lowestDistanceEnemy <= lvlTresholds[4]) lvlChoosen = 1;

            int totalSoundInfluence = soundLvlFromPlayer + button_1 + button_2 + button_3;

            switch (lvlChoosen)
            {
                case 1:
                    if (totalSoundInfluence >= 0 || pot_Speed_Value >= 50)
                    {
                        LoadLoastScene();
                    }
                    break;
                case 2:
                    if (totalSoundInfluence >= 1 || pot_Speed_Value >= 100)
                    {
                        LoadLoastScene();
                    }
                    break;
                case 3:
                    if (totalSoundInfluence >= 2 || pot_Speed_Value >= 250)
                    {
                        LoadLoastScene();
                    }
                    break;
                case 4:
                    if (totalSoundInfluence >= 3 || pot_Speed_Value >= 500)
                    {
                        LoadLoastScene();
                    }
                    break;
                case 5:
                    if (totalSoundInfluence >= 4 || pot_Speed_Value >= 1000)
                    {
                        LoadLoastScene();
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void LoadLoastScene()
    {
        if (SceneManager.GetActiveScene().name != "LostScene")
        {
            SceneManager.LoadScene("LostScene");
        }
    }

    public void CheckIfWin()
    {
        if (uboat.gameObject.transform.position.z >= lvlWinRange && uboat.radio.slider.value == uboat.radio.slider.maxValue && !sceneChangeRequested)
        {
            sceneChangeRequested = true; // Prevent multiple scene loads
            SceneManager.LoadScene("WinScene");
        }
    }

    public void BackgroundMusic()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.loop = true;
            if (!backgroundMusic.isPlaying)
            {
                backgroundMusic.Play();
            }
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            CheckIfLoss();
            CheckIfWin();
        }

        ChangeScene();

        BackgroundMusic();
    }
}
