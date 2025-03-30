using UnityEngine;

public class CreditsScript : MonoBehaviour
{
    public float scrollSpeed = 35f; // Speed of scrolling
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Moves the credits upwards
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
    }
}
