using UnityEngine;

public class MicDetect : MonoBehaviour
{
    public int micLoudnessInt;
    public float sensitivity = 50f; // Sensitivity adjustment
    private string micDevice;
    private AudioClip micClip;
    private int sampleWindow = 256; // Number of samples to take for analysis

    void Start()
    {
        // Get the default microphone device
        if (Microphone.devices.Length > 0)
        {
            micDevice = Microphone.devices[0]; // Use the first available microphone
            StartMic();
        }
        else
        {
            Debug.LogError("No microphone detected.");
        }
    }

    void Update()
    {
        micLoudnessInt = GetMicLoudnessAsInt();
    }

    void StartMic()
    {
        // Start recording from the microphone
        micClip = Microphone.Start(micDevice, true, 1, AudioSettings.outputSampleRate);
        while (!(Microphone.GetPosition(micDevice) > 0)) { } // Wait until recording starts
    }

    // Function to get microphone loudness
    int GetMicLoudnessAsInt()
    {
        if (micClip == null || !Microphone.IsRecording(micDevice))
        {
            Debug.LogError("Mic not initialized or not recording.");
            return 0;
        }

        int micPosition = Microphone.GetPosition(micDevice);
        if (micPosition < sampleWindow)
        {
            return 0; // Wait for more data before processing
        }

        // Adjust sample window if necessary (to prevent out of bounds)
        int startSample = micPosition - sampleWindow;
        if (startSample < 0) startSample = 0;

        // Create a buffer to hold the audio samples
        float[] samples = new float[sampleWindow];
        micClip.GetData(samples, startSample);

        // Calculate RMS (Root Mean Square) which is a good measure of loudness
        float sum = 0;
        for (int i = 0; i < sampleWindow; i++)
        {
            sum += samples[i] * samples[i];
        }
        float rmsValue = Mathf.Sqrt(sum / sampleWindow);

        // Apply a sensitivity multiplier to boost quiet sounds
        rmsValue *= sensitivity;

        // Convert the RMS value to decibels (a logarithmic scale)
        float decibels = 20 * Mathf.Log10(rmsValue / 0.1f);

        // If the decibel value is negative (for quiet sounds), we treat it as 0 to avoid clipping
        decibels = Mathf.Max(0, decibels);

        // Map decibel value to a more flexible integer range (for example, 0 to 100)
        int loudness = Mathf.Clamp((int)decibels, 0, 100);

        return loudness;
    }

    void OnDisable()
    {
        StopMic();
    }

    void StopMic()
    {
        Microphone.End(micDevice);
    }
}
