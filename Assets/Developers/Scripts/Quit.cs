using UnityEngine;

public class QuitGame : MonoBehaviour
{
    // This function will be called when the player clicks the quit button
    public void Quit()
    {
        // If we are running in the Unity Editor, stop playing the scene
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Otherwise, quit the application
        Application.Quit();
#endif
    }
}
