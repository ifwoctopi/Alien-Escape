using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class SceneLoader : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // Replace with your actual scene name
    }
}
