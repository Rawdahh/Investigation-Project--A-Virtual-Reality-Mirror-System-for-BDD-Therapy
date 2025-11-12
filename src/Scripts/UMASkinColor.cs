using UnityEngine;
using UMA.CharacterSystem;

public class UMASkinColor : MonoBehaviour
{
    public DynamicCharacterAvatar avatar;

    public void SetLightSkin()
    {
        ApplySkin(new Color(1.0f, 0.8f, 0.6f)); // light beige
        FileLogger.Instance.Log("SUCCESS: SetLightSkin() method was called.");
    }

    public void SetMediumSkin()
    {
        ApplySkin(new Color(0.75f, 0.55f, 0.4f)); // medium brown
        FileLogger.Instance.Log("SUCCESS: SetMediumSkin() method was called.");
    }

    public void SetDarkSkin()
    {
        ApplySkin(new Color(0.4f, 0.25f, 0.15f)); // dark brown
        FileLogger.Instance.Log("SUCCESS: SetDarkSkin() method was called.");
    }

    private void ApplySkin(Color color)
    {
        if (avatar == null) return;
        avatar.SetColor("Skin", color);
        avatar.BuildCharacter();
    }
}
