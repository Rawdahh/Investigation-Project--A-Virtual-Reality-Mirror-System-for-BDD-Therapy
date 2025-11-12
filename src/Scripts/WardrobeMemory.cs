using UnityEngine;

// Remember the last outfit that was chosen.

public class WardrobeMemory : MonoBehaviour

{
    // Our logger will read this variable to get the final choice.
    public string lastOutfitName { get; private set; }

    //UI buttons will call this function.
    public void SetLastOutfitName(string outfitName)

    {

        lastOutfitName = outfitName;

        Debug.Log($"Wardrobe choice remembered: {outfitName}");

    }

}