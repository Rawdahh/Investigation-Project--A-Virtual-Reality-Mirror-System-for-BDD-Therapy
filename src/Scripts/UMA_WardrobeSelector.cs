using UnityEngine;
using UMA.CharacterSystem;

public class UMA_WardrobeSelector : MonoBehaviour
{
    public DynamicCharacterAvatar avatar; //UMA avatar here

    // This variable will "remember" the last recipe name that was set.

    public string lastSelectedOutfit { get; private set; }

    public void SetOutfit(string recipeName)
    {
        // When a new outfit is set, we store its name in our new variable.
        lastSelectedOutfit = recipeName;

        if (avatar == null) return;

        // Clear old outfit
        avatar.ClearSlot("Chest");
        avatar.ClearSlot("Legs");
        avatar.ClearSlot("Feet");

        // Add new outfit recipe
        avatar.SetSlot(recipeName);

        // Rebuild with new clothing
        avatar.BuildCharacter();
    }
}
