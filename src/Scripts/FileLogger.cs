using UnityEngine;
using System.IO; // Required for file operations
using System;   // Required for timestamps

public class FileLogger : MonoBehaviour
{
    // A "static instance" makes it accessible from any other script
    // without needing a direct reference.
    public static FileLogger Instance;

    private string logFilePath;

    void Awake()
    {
        // Ensure there is only one instance of this logger
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the logger active between scenes

            // Define the path for our log file. Application.persistentDataPath is a safe,
            // writable folder on any device, including the Quest 2.
            logFilePath = Path.Combine(Application.persistentDataPath, "MyGameSessionLog.txt");

            // Write a header to the file to mark the start of a new session
            Log("--- NEW SESSION STARTED ---");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Writes a message to the MyGameSessionLog.txt file.
    /// </summary>
    /// <param name="message">The text you want to save.</param>
    public void Log(string message)
    {
        // Get the current time for a timestamp
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        string logEntry = $"[{timestamp}] {message}";

        try
        {
            // Append the text to the file, creating it if it doesn't exist
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }
        catch (Exception e)
        {
            // If something goes wrong, log an error to the Unity console
            Debug.LogError($"Failed to write to log file: {e.Message}");
        }
    }
}