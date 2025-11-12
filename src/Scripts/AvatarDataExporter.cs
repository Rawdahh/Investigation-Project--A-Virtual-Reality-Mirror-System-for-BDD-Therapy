////using UnityEngine;
////using System.Collections.Generic; // Required to use Lists
////using System.IO;
////using System;

////public class AvatarDataExporter : MonoBehaviour
////{
////    [Header("Sliders to Log")]
////    [Tooltip("Drag all the GameObjects that have a UMA_DNASlider script you want to log into this list.")]
////    public List<UMA_DNASlider> dnaSlidersToLog = new List<UMA_DNASlider>();

////    [Header("Optional Sliders")]
////    [Tooltip("Drag the GameObject with the UMASkinColorSlider script here, if you use it.")]
////    public UMASkinColorSlider skinColorSliderToLog;

////    // You can add a similar public variable for the Wardrobe Selector if you need it

////    // --- CHANGE THIS ---
////    [Tooltip("Drag the GameObject with the WardrobeMemory script here.")]
////    public WardrobeMemory wardrobeMemoryToLog; // Changed the type
////    // -------------------


////    public void ExportAvatarData()
////    {
////        string fileName = $"AvatarData_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
////        string filePath = Path.Combine(Application.persistentDataPath, fileName);

////        try
////        {
////            string fullReport = "--- AVATAR CUSTOMISATION FINALISED ---\n";

////            // --- THIS IS THE MODIFIED SECTION ---
////            // Loop through our specific list of DNA sliders
////            foreach (UMA_DNASlider dnaSlider in dnaSlidersToLog)
////            {
////                if (dnaSlider != null)
////                {
////                    // First, check the new flag.
////                    if (dnaSlider.hasBeenAdjusted)
////                    {
////                        // If it has been adjusted, log the final numerical value.
////                        fullReport += $"DNA Slider '{dnaSlider.dnaName}' final value: {dnaSlider.slider.value.ToString("F2")}\n";
////                    }
////                    else
////                    {
////                        // If it has NOT been adjusted, log the word "Default".
////                        fullReport += $"DNA Slider '{dnaSlider.dnaName}' final value: Default\n";
////                    }
////                }
////            }
////            // ------------------------------------

////            // Loop through our specific list instead of searching the whole scene
////            foreach (UMA_DNASlider dnaSlider in dnaSlidersToLog)
////            {
////                if (dnaSlider != null && dnaSlider.slider != null)
////                {
////                    fullReport += $"DNA Slider '{dnaSlider.dnaName}' final value: {dnaSlider.slider.value.ToString("F2")}\n";
////                }
////            }

////            // --- THIS SECTION IS UPDATED ---
////            if (wardrobeMemoryToLog != null)
////            {
////                if (!string.IsNullOrEmpty(wardrobeMemoryToLog.lastOutfitName)) // Changed the variable we read from
////                {
////                    fullReport += $"Wardrobe Choice final selection: '{wardrobeMemoryToLog.lastOutfitName}'\n"; // Changed the variable
////                }
////                else
////                {
////                    fullReport += "Wardrobe Choice final selection: 'Default (None)'\n";
////                }
////            }
////            // -----------------------------

////            fullReport += "--- End of Avatar Values ---";

////            File.WriteAllText(filePath, fullReport);

////            if (FileLogger.Instance != null)
////            {
////                FileLogger.Instance.Log($"Avatar data successfully exported to {fileName}");
////            }
////        }
////        catch (Exception e)
////        {
////            // Error handling
////            Debug.LogError($"Failed to export avatar data: {e.Message}");
////        }
////    }
////}

//using UnityEngine;
//using System.Collections.Generic; // Required to use Lists
//using System.IO;
//using System;

//public class AvatarDataExporter : MonoBehaviour
//{
//    [Header("Sliders to Log")]
//    [Tooltip("Drag all the GameObjects that have a UMA_DNASlider script you want to log into this list.")]
//    public List<UMA_DNASlider> dnaSlidersToLog = new List<UMA_DNASlider>();

//    [Header("Optional Components to Log")]
//    [Tooltip("Drag the GameObject with the UMASkinColorSlider script here, if you use it.")]
//    public UMASkinColorSlider skinColorSliderToLog;

//    [Tooltip("Drag the GameObject with the WardrobeMemory script here.")]
//    public WardrobeMemory wardrobeMemoryToLog;

//    public void ExportAvatarData()
//    {
//        string fileName = $"AvatarData_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
//        string filePath = Path.Combine(Application.persistentDataPath, fileName);

//        try
//        {
//            string fullReport = "--- AVATAR CUSTOMISATION FINALISED ---\n";

//            // --- 1. Log all the DNA Sliders ---
//            // This is the new "smart" loop that checks if a slider was adjusted.
//            foreach (UMA_DNASlider dnaSlider in dnaSlidersToLog)
//            {
//                if (dnaSlider != null)
//                {
//                    if (dnaSlider.hasBeenAdjusted)
//                    {
//                        // If adjusted, log the final numerical value.
//                        fullReport += $"DNA Slider '{dnaSlider.dnaName}' final value: {dnaSlider.slider.value.ToString("F2")}\n";
//                    }
//                    else
//                    {
//                        // If not adjusted, log the word "Default".
//                        fullReport += $"DNA Slider '{dnaSlider.dnaName}' final value: Default\n";
//                    }
//                }
//            }

//            // --- 2. Log the Skin Color Slider (if assigned) ---
//            // (You could apply the same "hasBeenAdjusted" logic to UMASkinColorSlider.cs if you wanted)
//            if (skinColorSliderToLog != null && skinColorSliderToLog.slider != null)
//            {
//                fullReport += $"Skin Color Slider final value: {skinColorSliderToLog.slider.value.ToString("F2")}\n";
//            }

//            // --- 3. Log the Wardrobe Choice (if assigned) ---
//            if (wardrobeMemoryToLog != null)
//            {
//                if (!string.IsNullOrEmpty(wardrobeMemoryToLog.lastOutfitName))
//                {
//                    fullReport += $"Wardrobe Choice final selection: '{wardrobeMemoryToLog.lastOutfitName}'\n";
//                }
//                else
//                {
//                    fullReport += "Wardrobe Choice final selection: 'Default (None)'\n";
//                }
//            }

//            // --- 4. Finalize the Report ---
//            fullReport += "--- End of Avatar Values ---";

//            File.AppendAllText(filePath, fullReport + Environment.NewLine); // We also add a new line for spacing

//            if (FileLogger.Instance != null)
//            {
//                FileLogger.Instance.Log($"Avatar data successfully exported to {fileName}");
//            }
//        }
//        catch (Exception e)
//        {
//            // Error handling
//            Debug.LogError($"Failed to export avatar data: {e.Message}");
//        }
//    }
//}


using UnityEngine;
using System.Collections.Generic; // Required to use Lists
using System.IO;
using System;

public class AvatarDataExporter : MonoBehaviour
{
    [Header("Sliders to Log")]
    [Tooltip("Drag all the GameObjects that have a UMA_DNASlider script you want to log into this list.")]
    public List<UMA_DNASlider> dnaSlidersToLog = new List<UMA_DNASlider>();

    [Header("Optional Components to Log")]
    [Tooltip("Drag the GameObject with the UMASkinColorSlider script here, if you use it.")]
    public UMASkinColorSlider skinColorSliderToLog;

    [Tooltip("Drag the GameObject with the WardrobeMemory script here.")]
    public WardrobeMemory wardrobeMemoryToLog;

    public void ExportAvatarData()
    {
        // --- CHANGE #1: Use a fixed filename instead of a timestamped one ---
        string fileName = "AvatarData.txt";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        // -------------------------------------------------------------------

        try
        {
            // The logic to build the report string is exactly the same as before.
            string fullReport = "--- AVATAR CUSTOMISATION FINALISED ---\n";

            foreach (UMA_DNASlider dnaSlider in dnaSlidersToLog)
            {
                if (dnaSlider != null)
                {
                    if (dnaSlider.hasBeenAdjusted)
                    {
                        fullReport += $"DNA Slider '{dnaSlider.dnaName}' final value: {dnaSlider.slider.value.ToString("F2")}\n";
                    }
                    else
                    {
                        fullReport += $"DNA Slider '{dnaSlider.dnaName}' final value: Default\n";
                    }
                }
            }

            if (skinColorSliderToLog != null && skinColorSliderToLog.slider != null)
            {
                fullReport += $"Skin Color Slider final value: {skinColorSliderToLog.slider.value.ToString("F2")}\n";
            }

            if (wardrobeMemoryToLog != null)
            {
                if (!string.IsNullOrEmpty(wardrobeMemoryToLog.lastOutfitName))
                {
                    fullReport += $"Wardrobe Choice final selection: '{wardrobeMemoryToLog.lastOutfitName}'\n";
                }
                else
                {
                    fullReport += "Wardrobe Choice final selection: 'Default (None)'\n";
                }
            }

            fullReport += "--- End of Avatar Values ---";

            // --- CHANGE #2: Use AppendAllText to add to the file ---
            File.AppendAllText(filePath, fullReport + Environment.NewLine + Environment.NewLine);
            // -------------------------------------------------------

            if (FileLogger.Instance != null)
            {
                // This message now correctly refers to the fixed filename.
                FileLogger.Instance.Log($"Avatar data successfully appended to {fileName}");
            }
        }
        catch (Exception e)
        {
            // Error handling
            Debug.LogError($"Failed to export avatar data: {e.Message}");
        }
    }
}