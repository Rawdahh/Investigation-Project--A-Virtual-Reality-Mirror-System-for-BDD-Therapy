using UnityEngine;
using UMA;
using UMA.CharacterSystem;

public class UMA_OutfitStyle3 : MonoBehaviour
{
    public DynamicCharacterAvatar avatar;

    //wardrobe recipes for the outfit
    public UMAWardrobeRecipe topRecipe;
    public UMAWardrobeRecipe pantsRecipe;
    public UMAWardrobeRecipe shoesRecipe;

    public void ApplyProvocativeStyle()
    {
        if (avatar == null) return;

        if (topRecipe != null) avatar.SetSlot(topRecipe);
        if (pantsRecipe != null) avatar.SetSlot(pantsRecipe);
        if (shoesRecipe != null) avatar.SetSlot(shoesRecipe);

        avatar.BuildCharacter();
        Debug.Log("[UMA_OutfitStyle] Applied Provocative outfit!");
    }
}
