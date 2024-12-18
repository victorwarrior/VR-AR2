using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;

public class GameManager : MonoBehaviour
{
    //refrence til gamemanager til singelton
    public static GameManager Instance { get; private set; }
    
    //refrence til andre scripts uboat og spawner af enemy
    [HideInInspector]
    public Uboat uboat;
    public EnemySpawner spawner;

    //den nærmeste enemy som stater på infinity som er længst væk
    public float lowestDistanceEnemy = float.PositiveInfinity;

    //lyd fra sensor mikrofon
    public int soundLvlFromPlayer;
    //bliver ikke brugt men ref til cam
    public Camera playerCameraGameobject;
    //pot value til hastighed
    public int pot_Speed_Value;
    //converter værdi til mindre stadier
    public int pot_Speed_Value_Converted;
    //ref til speed slider
    private Slider speedSlider;
    //ref til slider text
    private Text speedSliderText;

    //knappe værdier
    public int button_1;
    public int button_2;
    public int button_3;
    //værdi fra talkingpeople
    public int talkingPeople;

    //hvor langt man skal være fra nærmeste ship for at skifte lvls
    public int[] lvlTresholds = { 100, 75, 50, 25, 10 };
    //de forskelige lvls på pot hastighed
    public int[] potLvlTresholds = { 1, 2, 3, 4, 5};
    //hvor langt man skal sejle for at vinde
    public int lvlWinRange = 100;

    //bagrundslyd
    public AudioSource backgroundMusic;

    //når man skal skifte scene
    private bool sceneChangeRequested = false;

    //gemmer scenen
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    //fjerner scenen
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //når scenen loader
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset scene
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
        //singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //gemer sidste værdi til at se om man vil restart 
    private int previousButton1State;
    private int previousButton2State;
    private int previousButton3State;

    //når man trykker på en random knap restarter scenen
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

    //skifter scene tilbage til main 
    public void ChangeScene()
    {
        if (DetectButtonStateChange())
        {
            if (SceneManager.GetActiveScene().name == "LostScene" || SceneManager.GetActiveScene().name == "WinScene")
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    //checker om man har tabt
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

    //loader loss scene
    public void LoadLoastScene()
    {
        if (SceneManager.GetActiveScene().name != "LostScene")
        {
            SceneManager.LoadScene("LostScene");
        }
    }

    //check om man har vundet
    public void CheckIfWin()
    {
        if (uboat.gameObject.transform.position.z >= lvlWinRange && uboat.radio.slider.value == uboat.radio.slider.maxValue && !sceneChangeRequested)
        {
            sceneChangeRequested = true; // forhindre flere scene loads
            SceneManager.LoadScene("WinScene");
        }
    }

    //bagrunds musik
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

    //setter hastighed slider
   public void SetSpeedSlider()
{
    speedSlider.value = pot_Speed_Value_Converted;
    speedSliderText.text = "Speed: " + speedSlider.value;

    Color sliderColor = GetSpeedColor(pot_Speed_Value_Converted);
    speedSlider.fillRect.GetComponent<Image>().color = sliderColor;
}

    //setter farven baseret på hastighed
private Color GetSpeedColor(int speedValue)
{
    if (speedValue >= potLvlTresholds[4])
    {
        return Color.red; // High
    }
    else if (speedValue >= potLvlTresholds[3])
    {
        return new Color(1f, 0.647f, 0f);
    }
    else if (speedValue >= potLvlTresholds[2])
    {
        return Color.yellow; // Medium
    }
    else if (speedValue >= potLvlTresholds[1])
    {
        return Color.green; 
    }
    else
    {
        return Color.cyan; //low
    }
}
    //ændre pot værdi til 6 stadier
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

    //køre updates på om har tabt, vundet, ændre pot værdi og set speed slider
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            CheckIfLoss();
            CheckIfWin();
            ConvertPotValue();
            SetSpeedSlider();

        }
        //change scene og bagrundsmusik
        ChangeScene();
        BackgroundMusic();
    }
}
