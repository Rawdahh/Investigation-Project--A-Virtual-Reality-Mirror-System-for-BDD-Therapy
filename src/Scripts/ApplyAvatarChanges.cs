using UnityEngine;
using UMA.CharacterSystem;

public class ApplyAvatarChanges : MonoBehaviour
{
    // We will assign our PlayerAvatar to this in the Inspector.
    public DynamicCharacterAvatar avatar;

    // This is the function our button will call.
    public void ApplyChanges()
    {
        if (avatar != null)
        {
            Debug.Log("Apply button clicked. Rebuilding character now.");
            avatar.BuildCharacter();
        }
        else
        {
            Debug.LogError("Avatar is not assigned on the ApplyAvatarChanges script!");
        }
    }
}