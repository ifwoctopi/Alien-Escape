using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Deathfade : MonoBehaviour
{
    private Image image;
    public float fadeDuration = 2f;
    public float delayBeforeFade = 20f;

    void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(FadeAfterDelay());
    }

    IEnumerator FadeAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeFade); // Wait before fading

        float elapsedTime = 0f;
        Color originalColor = image.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0); // Fully transparent
    }
}
