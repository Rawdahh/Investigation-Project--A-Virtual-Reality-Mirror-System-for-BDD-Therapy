using UnityEngine;

public class DialogueTimingManager : MonoBehaviour
{
    // This makes the script a Singleton, so we can access it from anywhere.
    public static DialogueTimingManager Instance { get; private set; }

    private float startTime;
    private bool isTiming = false;

    // This runs before the Start method.
    void Awake()
    {
        // Singleton pattern: Ensure only one instance exists.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy this new one if an instance already exists.
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy this object when loading a new scene.
        }
    }

    /// <summary>
    /// Starts the dialogue decision timer.
    /// </summary>
    public void StartDialogueTimer()
    {
        if (isTiming)
        {
            Debug.LogWarning("Timer was already started. Restarting now.");
        }

        startTime = Time.time; // Record the exact time the game has been running.
        isTiming = true;
        Debug.Log("Dialogue timer STARTED at: " + startTime);
    }

    /// <summary>
    /// Stops the timer and logs the result to the console.
    /// </summary>
    /// <param name="choiceDescription">A string describing the choice that was made.</param>
    public void StopDialogueTimer(string choiceDescription)
    {
        if (!isTiming)
        {
            Debug.LogWarning("StopDialogueTimer was called, but the timer was not running.");
            return; // Exit the function if we weren't timing.
        }

        float endTime = Time.time;
        float duration = endTime - startTime;
        isTiming = false;

        // Log the result to the Unity Console.
        Debug.Log($"DECISION MADE: Player chose '{choiceDescription}'. Time taken: {duration.ToString("F2")} seconds.");

        // For a real game, you would send this data to an analytics service here.
    }
}