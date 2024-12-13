using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uboat : MonoBehaviour
{
    public GameObject lightSource;
    public AudioSource radioAudio;
    public GameObject radar;
    public GameObject uBoat;

    public Radio radio;

    // Variables for movement
    private float currentSpeed = 0f;
    private float targetSpeed = 0f;
    private float speedSmoothTime = 0.5f; // Smooth transition time for speed
    private float velocity = 0f; // Used for smoothing

    public AudioSource engineAudio; 
    private float targetVolume = 0f;
    public float maxVolume = 1.0f; // Max volume of the sound
    public float fadeSpeed = 1.0f; // How fast the volume fades in/out



    private void Update()
    {
        if (GameManager.Instance.button_1 == 1)
        {
            TurnOffOnLight(1);
        }
        else
        {
            TurnOffOnLight(0);
        }

        if (GameManager.Instance.button_2 == 1)
        {
            TurnOffOnRadio(1);
        }
        else
        {
            TurnOffOnRadio(0);
        }

        if (GameManager.Instance.button_3 == 1)
        {
            TurnOffOnRadar(1);
        }
        else
        {
            TurnOffOnRadar(0);
        }

        UboatAudio();
        MoveFowardUboat(GameManager.Instance.pot_Speed_Value_Converted);
    }

    public void TurnOffOnLight(int state)
    {
        if (lightSource != null)
        {
            if (state == 1)
            {
                lightSource.SetActive(true);
            }
            else if (state == 0)
            {
                lightSource.SetActive(false);
            }
        }

    }

    public void TurnOffOnRadio(int state)
    {
        if (radioAudio != null)
        {
            if (state == 1)
            {
                if (!radioAudio.isPlaying) // Check if the audio is already playing
                {
                    radioAudio.Play();
                    radio.StartCoroutine(radio.IncreaseSliderOverTime());
                }
            }
            else if (state == 0)
            {
                if (radioAudio.isPlaying) 
                {
                    radioAudio.Pause();
                    radio.StopCoroutine(radio.IncreaseSliderOverTime()); 

                }
            }
        }
    }


    public void TurnOffOnRadar(int state)
    {
        if (radar != null)
        {
            if (state == 1)
            {
                radar.SetActive(true);
            }
            else if (state == 0)
            {
                radar.SetActive(false);
            }
        }

    }

    public void MoveFowardUboat(int speed)
    {
        // Calculate the target speed based on the converted pot value
        targetSpeed = Mathf.Lerp(0f, 10f, speed / 5f); // Map 0-5 to 0-10 (adjust as needed)

        // Smoothly transition to the target speed using SmoothDamp
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref velocity, speedSmoothTime);

        // Move the Uboat in the forward direction based on smoothed speed
        uBoat.transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }


    public void UboatAudio()
    {
        currentSpeed = GetSpeed(); // Get the current speed of the car

        float[] volumeLevels = { 0f, 0.2f, 0.4f, 0.6f, 0.8f, 1.0f };

        targetVolume = 0f;

        for (int i = 0; i < GameManager.Instance.potLvlTresholds.Length; i++)
        {
            if (currentSpeed < GameManager.Instance.potLvlTresholds[i])
            {
                targetVolume = volumeLevels[i]; 
                break; 
            }
        }

        if (targetVolume > 0 && !engineAudio.isPlaying)
        {
            engineAudio.Play();
        }
        else if (targetVolume <= 0 && engineAudio.isPlaying)
        {
            engineAudio.Stop();
        }

        engineAudio.volume = Mathf.MoveTowards(engineAudio.volume, targetVolume, fadeSpeed * Time.deltaTime);
    }

    private float GetSpeed()
    {
        return GameManager.Instance.pot_Speed_Value_Converted; 
    }

}

