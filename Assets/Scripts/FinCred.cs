using UnityEngine;
using UnityEngine.UI;

public class ScrollCredits : MonoBehaviour
{
    public float speed = 50f; // Adjust as needed

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
