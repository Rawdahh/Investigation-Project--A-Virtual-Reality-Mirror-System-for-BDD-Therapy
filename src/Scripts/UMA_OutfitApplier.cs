using UnityEngine;
using UMA;
using UMA.CharacterSystem;

public class UMA_OutfitApplier : MonoBehaviour
{
    public DynamicCharacterAvatar avatar;
    public UMAWardrobeRecipe recipeAsset;

    public void ApplyRecipeByAsset()
    {
        if (avatar == null)
        {
            Debug.LogError("[UMA_OutfitApplier] Avatar is NULL.");
            return;
        }

        if (recipeAsset == null)
        {
            Debug.LogError("[UMA_OutfitApplier] recipeAsset is NULL.");
            return;
        }

        // UMA 2 DCS way:
        avatar.ClearSlot(recipeAsset.wardrobeSlot); // clear any previous item in this slot
        avatar.SetSlot(recipeAsset);                // apply the recipe

        avatar.BuildCharacter(); // force rebuild
        Debug.Log("[UMA_OutfitApplier] Applied wardrobe: " + recipeAsset.name);
    }
}
