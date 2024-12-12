using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [HideInInspector]
    public Uboat uboat;
    public EnemySpawner spawner;

    public float lowestDistanceEnemy = float.PositiveInfinity;

    public int soundLvlFromPlayer;
    public Camera playerCameraGameobject;

    public int pot_Speed_Value;
    public int pot_Speed_Value_Converted;
    private Slider speedSlider;
    private Text speedSliderText;

    public int button_1;
    public int button_2;
    public int button_3;
    public int talkingPeople;

    public int[] lvlTresholds = { 100, 75, 50, 25, 10 };
    public int[] potLvlTresholds = { 1, 2, 3, 4, 5};
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
            spawner.player = uboat.transform;
            speedSlider = GameObject.Find("SpeedSlider").GetComponent<Slider>();
            speedSliderText = GameObject.Find("SpeedSliderText").GetComponent<Text>();
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

            int totalSoundInfluence = soundLvlFromPlayer + button_1 + button_2 + button_3 + talkingPeople;

            switch (lvlChoosen)
            {
                case 1:
                    if (totalSoundInfluence >= 1 || pot_Speed_Value_Converted >= potLvlTresholds[0])
                    {
                        LoadLoastScene();
                    }
                    break;
                case 2:
                    if (totalSoundInfluence >= 2 || pot_Speed_Value_Converted >= potLvlTresholds[1])
                    {
                        LoadLoastScene();
                    }
                    break;
                case 3:
                    if (totalSoundInfluence >= 3 || pot_Speed_Value_Converted >= potLvlTresholds[2])
                    {
                        LoadLoastScene();
                    }
                    break;
                case 4:
                    if (totalSoundInfluence >= 4 || pot_Speed_Value_Converted >= potLvlTresholds[3])
                    {
                        LoadLoastScene();
                    }
                    break;
                case 5:
                    if (totalSoundInfluence >= 5 || pot_Speed_Value_Converted >= potLvlTresholds[4])
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

   public void SetSpeedSlider()
{
    speedSlider.value = pot_Speed_Value_Converted;
    speedSliderText.text = "Speed: " + speedSlider.value;

    Color sliderColor = GetSpeedColor(pot_Speed_Value_Converted);
    speedSlider.fillRect.GetComponent<Image>().color = sliderColor;
}

private Color GetSpeedColor(int speedValue)
{
    if (speedValue >= potLvlTresholds[4])
    {
        return Color.red; // High speed
    }
    else if (speedValue >= potLvlTresholds[3])
    {
        return new Color(1f, 0.647f, 0f);//orange
    }
    else if (speedValue >= potLvlTresholds[2])
    {
        return Color.yellow; // Medium speed
    }
    else if (speedValue >= potLvlTresholds[1])
    {
        return Color.green; // Low speed
    }
    else
    {
        return Color.cyan; // Very low speed
    }
}
    public void ConvertPotValue()
    {
        pot_Speed_Value_Converted = (pot_Speed_Value * 6) / 1023;
        Debug.Log("Pot Speed Value Section: " + pot_Speed_Value_Converted);


        if (pot_Speed_Value_Converted == 0)
        {

        }
        else if (pot_Speed_Value_Converted == 1)
        {

        }
        else if (pot_Speed_Value_Converted == 2)
        {

        }
        else if (pot_Speed_Value_Converted == 3)
        {

        }
        else if (pot_Speed_Value_Converted == 4)
        {

        }
        else if (pot_Speed_Value_Converted == 5)
        {

        }
    }


    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            CheckIfLoss();
            CheckIfWin();
            ConvertPotValue();
            SetSpeedSlider();

        }

        ChangeScene();
        BackgroundMusic();
    }
}
