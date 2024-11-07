using UnityEngine;
using UnityEngine.UI;

public class Dot : MonoBehaviour
{
    private Image dotImage;
    private float fadeDuration = 3f; // Time to fade out (seconds)
    private float fadeTime = 0f;

    // Initialize the dot's position and set its color to fully visible
    public void Initialize(Vector3 position)
    {
        dotImage = GetComponent<Image>();
        transform.localPosition = position; // Set the position of the dot
        dotImage.color = new Color(dotImage.color.r, dotImage.color.g, dotImage.color.b, 1); // Fully visible initially
    }

    // Gradually fade out the dot
    public void Fade()
    {
        fadeTime += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, fadeTime / fadeDuration);
        dotImage.color = new Color(dotImage.color.r, dotImage.color.g, dotImage.color.b, alpha);
    }

    // Check if the dot is fully faded
    public bool IsFaded()
    {
        return dotImage.color.a <= 0f;
    }
}
