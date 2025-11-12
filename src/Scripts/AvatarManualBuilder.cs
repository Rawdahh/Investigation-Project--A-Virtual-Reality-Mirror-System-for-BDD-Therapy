using UnityEngine;
using UMA.CharacterSystem;

public class AvatarManualBuilder : MonoBehaviour
{
    public DynamicCharacterAvatar avatar;

    public void RebuildAvatar()
    {
        if (avatar != null)
        {
            Debug.Log("Manual build button pressed. Rebuilding character...");
            avatar.BuildCharacter();
        }
        else
        {
            Debug.LogError("Avatar not assigned to the Manual Builder script!");
        }
    }
}