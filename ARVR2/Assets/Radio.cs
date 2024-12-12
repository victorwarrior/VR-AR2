using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Radio : MonoBehaviour
{
    public Slider slider;
    public Text progressText;
    public float timePerIncrease = 1f;


    public IEnumerator IncreaseSliderOverTime()
    {
        while (slider.value < slider.maxValue && GameManager.Instance.button_2 == 1 && SceneManager.GetActiveScene().name == "MainScene")
        {
            yield return new WaitForSeconds(timePerIncrease); 

            slider.value++; 
            progressText.text = "Progress: " + Mathf.FloorToInt(slider.value) + "%"; 
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
