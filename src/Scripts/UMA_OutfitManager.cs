using UnityEngine;
using UMA;
using UMA.CharacterSystem;

[System.Serializable]
public class Outfit
{
    public string outfitName;
    public UMAWardrobeRecipe[] recipes; // multiple recipes (top, pants, shoes, etc.)
}

public class UMA_OutfitManager : MonoBehaviour
{
    public DynamicCharacterAvatar avatar;
    public Outfit[] outfits; // list of all outfit styles

    public void ApplyOutfit(string outfitName)
    {
        if (avatar == null)
        {
            Debug.LogError("[UMA_OutfitManager] Avatar not assigned!");
            return;
        }

        // Clear ALL wardrobe slots before applying new outfit
        avatar.ClearSlots();

        // Find the outfit by name
        Outfit selectedOutfit = null;
        foreach (var outfit in outfits)
        {
            if (outfit.outfitName == outfitName)
            {
                selectedOutfit = outfit;
                break;
            }
        }

        if (selectedOutfit == null)
        {
            Debug.LogError("[UMA_OutfitManager] Outfit not found: " + outfitName);
            return;
        }

        // Apply all recipes in the outfit
        foreach (var recipe in selectedOutfit.recipes)
        {
            if (recipe != null)
            {
                avatar.SetSlot(recipe);
                Debug.Log("[UMA_OutfitManager] Added " + recipe.name);
            }
        }

        // Rebuild avatar
        avatar.BuildCharacter();
        Debug.Log("[UMA_OutfitManager] Applied outfit: " + outfitName);
    }
}
