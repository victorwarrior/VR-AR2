using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uboat : MonoBehaviour
{
    //lys refrence
    public GameObject lightSource;
    //radio refrence
    public AudioSource radioAudio;
    //radar refrence
    public GameObject radar;
    //radar ubåd 
    public GameObject uBoat;

    //radio refrence
    public Radio radio;

    //variable for movement
    private float currentSpeed = 0f;
    private float targetSpeed = 0f;
    private float speedSmoothTime = 0.5f; 
    private float velocity = 0f; 

    //Lyd til moterlarm
    public AudioSource engineAudio; 
    private float targetVolume = 0f;
    public float maxVolume = 1.0f; 
    public float fadeSpeed = 1.0f;



    private void Update()
    {
        //tænder og slukker lys
        if (GameManager.Instance.button_1 == 1)
        {
            TurnOffOnLight(1);
        }
        else
        {
            TurnOffOnLight(0);
        }

        //tænder og slukker radio
        if (GameManager.Instance.button_2 == 1)
        {
            TurnOffOnRadio(1);
        }
        else
        {
            TurnOffOnRadio(0);
        }

        //tænder og slukker radar
        if (GameManager.Instance.button_3 == 1)
        {
            TurnOffOnRadar(1);
        }
        else
        {
            TurnOffOnRadar(0);
        }

        //lyd til ubåd
        UboatAudio();
        //ubåd frem
        MoveFowardUboat(GameManager.Instance.pot_Speed_Value_Converted);
    }

    //funktion til at tænde og slukke lys
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

    //funktion til at tænde og slukke radio
    public void TurnOffOnRadio(int state)
    {
        if (radioAudio != null)
        {
            if (state == 1)
            {
                if (!radioAudio.isPlaying) //check vis lyden spiller
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
        targetSpeed = Mathf.Lerp(0f, 10f, speed / 5f); // Map 0-5 to 0-10 (adjust as needed)
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref velocity, speedSmoothTime);
        uBoat.transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }


    public void UboatAudio()
    {
        currentSpeed = GetSpeed(); 

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

