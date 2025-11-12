using UnityEngine;

// This is the script that will gather and log all the final avatar data.
public class AvatarLogger : MonoBehaviour
{
    /// <summary>
    /// This is the public function we will call from our "Confirm" button.
    /// </summary>
    public void LogFinalAvatarValues()
    {
        // 1. Log a clear header to the file.
        FileLogger.Instance.Log("--- AVATAR CUSTOMISATION FINALISED ---");

        // 2. Find and log all DNA slider values.
        UMA_DNASlider[] dnaSliders = FindObjectsOfType<UMA_DNASlider>();
        foreach (UMA_DNASlider dnaSlider in dnaSliders)
        {
            string message = $"DNA Slider '{dnaSlider.dnaName}' final value: {dnaSlider.slider.value.ToString("F2")}";
            FileLogger.Instance.Log(message);
        }

        // 3. Find and log the skin color slider value.
        UMASkinColorSlider skinSlider = FindObjectOfType<UMASkinColorSlider>();
        if (skinSlider != null)
        {
            string message = $"Skin Color Slider final value: {skinSlider.slider.value.ToString("F2")}";
            FileLogger.Instance.Log(message);
        }
        else
        {
            FileLogger.Instance.Log("WARNING: UMASkinColorSlider was not found in the scene.");
        }

        // 4. Find and log the final wardrobe choice.
        UMA_WardrobeSelector wardrobeSelector = FindObjectOfType<UMA_WardrobeSelector>();
        if (wardrobeSelector != null)
        {
            // Check if an outfit was actually selected (it might be the default empty one)
            if (!string.IsNullOrEmpty(wardrobeSelector.lastSelectedOutfit))
            {
                string message = $"Wardrobe Choice final selection: '{wardrobeSelector.lastSelectedOutfit}'";
                FileLogger.Instance.Log(message);
            }
            else
            {
                FileLogger.Instance.Log("Wardrobe Choice final selection: 'Default (None)'");
            }
        }
        else
        {
            FileLogger.Instance.Log("WARNING: UMA_WardrobeSelector was not found in the scene.");
        }

        // 5. Log a footer to neatly close the data block.
        FileLogger.Instance.Log("--- End of Avatar Values ---");

    }
}