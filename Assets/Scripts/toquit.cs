using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void toquit()
    {
        Debug.Log("Quit button pressed!");

        // Quit the game when built
        Application.Quit();

        // Exit play mode in the Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
