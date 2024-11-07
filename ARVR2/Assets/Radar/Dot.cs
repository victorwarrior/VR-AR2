using UnityEngine;
using UnityEngine.UI;

public class Dot : MonoBehaviour
{
    private RectTransform rectTransform;
    private Image dotImage; // To access the Image component for fading.
    private float fadeSpeed = 1f; // Speed at which the dot fades out.
    private float alphaValue = 1f; // Initial alpha value (fully visible).

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        dotImage = GetComponent<Image>(); // Get the Image component.
    }

    private void Update()
    {
        // Fade the dot by decreasing its alpha value.
        if (alphaValue > 0)
        {
            alphaValue -= fadeSpeed * Time.deltaTime;
            Color dotColor = dotImage.color;
            dotColor.a = alphaValue; // Apply the fade effect.
            dotImage.color = dotColor;
        }
        else
        {
            Destroy(gameObject); // Remove the dot once it's fully faded.
        }
    }

    // Initialize the dot with a position on the radar.
    public void Initialize(Vector3 radarPosition)
    {
        rectTransform.localPosition = radarPosition; // Set the position on the radar.
    }

    // Check if the dot has fully faded.
    public bool IsFaded()
    {
        return alphaValue <= 0;
    }

    // A method to manually control the fading (if needed).
    public void Fade()
    {
        alphaValue -= fadeSpeed * Time.deltaTime;
    }
}
