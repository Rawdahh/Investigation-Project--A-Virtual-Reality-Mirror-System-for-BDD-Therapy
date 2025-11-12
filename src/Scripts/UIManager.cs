using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Public methods to be called by UI Buttons
    public void StartGame()
    {
        Debug.Log("Starting Game...");
        SceneManager.LoadScene("FirstPerson"); // Load game scene 
    }

    public void OpenOptions()
    {
        Debug.Log("Opening Options...");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting Game...");
        Application.Quit(); // Quits the application (only works in a build, not in Editor)
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stops playing in the Editor
        #endif
    }
}