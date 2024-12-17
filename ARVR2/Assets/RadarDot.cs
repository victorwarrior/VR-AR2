using UnityEngine;
using UnityEngine.UI;

//klasse til at visulisere en prik på raderen hvor der er en båd
public class RadarDot : MonoBehaviour
{
    //billed til radar dot
    private Image dotImage;
    //Bruges til fade effect
    private bool isFading = false;
    public float fadeDuration = 1.0f; // Duration of the fade effect (in seconds)
    private float currentFadeTime;

    //tager dens image
    void Awake()
    {
        dotImage = GetComponent<Image>();
        if (dotImage == null)
            Debug.LogError("RadarDot requires an Image component.");
    }

    //laver dot
    public void InitializeDot()
    {
        SetAlpha(0f); // Initialize with alpha 0 so it's invisible
    }

    //Den er blevet ramt af swiper og skal få farve og fade
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

    //den begynder at fade
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

    //den bliver synlig
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
