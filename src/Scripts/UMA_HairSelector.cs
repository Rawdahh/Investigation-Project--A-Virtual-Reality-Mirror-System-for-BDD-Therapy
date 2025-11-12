using UnityEngine;
using UMA.CharacterSystem;

public class UMA_HairSelector : MonoBehaviour
{
    public DynamicCharacterAvatar avatar; // UMA avatar

    public void SetHair(string hairRecipeName)
    {
        if (avatar == null) return;

        // Clear existing hair
        avatar.ClearSlot("Hair");

        // Add new hair
        avatar.SetSlot(hairRecipeName);

        // Rebuild the avatar with new hair
        avatar.BuildCharacter();
    }
}
