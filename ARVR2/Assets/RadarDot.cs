using UnityEngine;
using UnityEngine.UI;

public class RadarDot : MonoBehaviour
{
    private Image dotImage;
    private bool isFading = false;
    public float fadeDuration = 1.0f; // Duration of the fade effect (in seconds)
    private float currentFadeTime;

    void Awake()
    {
        dotImage = GetComponent<Image>();
        if (dotImage == null)
            Debug.LogError("RadarDot requires an Image component.");
    }

    public void InitializeDot()
    {
        SetAlpha(0f); // Initialize with alpha 0 so it's invisible
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sweeper"))
        {
            // Reset the alpha to 1 (fully visible) and start fading
            SetAlpha(1.0f);
            currentFadeTime = 0f;
            isFading = true;
        }
    }

    void Update()
    {
        if (isFading)
        {
            // Increment fade time
            currentFadeTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1.0f, 0.0f, currentFadeTime / fadeDuration);

            // Apply the alpha to the dot
            SetAlpha(alpha);

            // Stop fading when fully transparent
            if (alpha <= 0.0f)
            {
                isFading = false;
            }
        }
    }

    private void SetAlpha(float alpha)
    {
        if (dotImage != null)
        {
            Color color = dotImage.color;
            color.a = alpha;
            dotImage.color = color;
        }
    }
}
