/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Collections;
using System;

/**
 * Sample for reading using polling by yourself. In case you are fond of that.
 */
public class SampleUserPolling_JustRead : MonoBehaviour
{
    public SerialController serialController;

    // Initialization
    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
	}

    // Executed each frame
    void Update()
    {
        string message = serialController.ReadSerialMessage();

        if (message == null)
            return;

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else
            Debug.Log(message);

 
            string[] parts = message.Split('/');

            int int1 = int.Parse(parts[0]);
            int int2 = int.Parse(parts[1]);
            int int3 = int.Parse(parts[2]);
            int int4 = int.Parse(parts[3]);
            int int5 = int.Parse(parts[4]);


            GameManager.Instance.pot_Speed_Value = int1;
            GameManager.Instance.soundLvlFromPlayer = int2;
            GameManager.Instance.button_1 = int3;
            GameManager.Instance.button_2 = int4;
            GameManager.Instance.button_3 = int5;





    }
}
