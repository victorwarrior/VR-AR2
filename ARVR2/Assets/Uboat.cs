using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uboat : MonoBehaviour
{
    public GameObject lightSource;
    public GameObject radioAudio;
    public GameObject radar;
    public GameObject uBoat;


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

        MoveFowardUboat(GameManager.Instance.pot_Speed_Value);
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
                radioAudio.SetActive(true);
            }
            else if (state == 0)
            {
                radioAudio.SetActive(false);
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
        float moveSpeed = Mathf.Lerp(0f, 10f, speed / 1023f); // Normalize speed and scale it to a max value (e.g., 10 units per second)
        uBoat.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

}
