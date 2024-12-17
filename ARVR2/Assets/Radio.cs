using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Klasse til radio
public class Radio : MonoBehaviour
{
    //visuelle objecter der bruges til at visualisere i scenen
    public Slider slider;
    public Text progressText;

    //Hvor lang tid skal der gå mellem IncreaseSliderOverTime kaldes
    public float timePerIncrease = 1f;


    //funktion der får slider til at stige i value
    public IEnumerator IncreaseSliderOverTime()
    {
        while (slider.value < slider.maxValue && GameManager.Instance.button_2 == 1 && SceneManager.GetActiveScene().name == "MainScene")
        {
            yield return new WaitForSeconds(timePerIncrease); 

            slider.value++; 
            progressText.text = "Progress: " + Mathf.FloorToInt(slider.value) + "%"; 
        }
    }
}
