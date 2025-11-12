using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // public function that our UI Button can see and call.
    // uses a string for the name of the scene to load.
    public void LoadSceneByName(string sceneName)
    {
        // Print a message to the console for debugging
        Debug.Log("Loading scene: " + sceneName);

        // core command that loads the new scene.
        SceneManager.LoadScene(sceneName);
    }

    // "Next" button.
    public void LoadNextScene()
    {
      
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        Debug.Log("Current Scene Index: " + currentSceneIndex);
        Debug.Log("Next Scene Index: " + (currentSceneIndex + 1));
        SceneManager.LoadScene(currentSceneIndex + 1);
  
}
}