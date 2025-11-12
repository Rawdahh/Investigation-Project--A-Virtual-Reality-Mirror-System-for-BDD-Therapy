using UnityEngine;
using UnityEngine.UI;
using UMA.CharacterSystem;

public class UMASkinColorSlider : MonoBehaviour
{
    public DynamicCharacterAvatar avatar;
    public Slider slider;
    public Gradient skinGradient;

    void Start()
    {
        if (slider != null)
            slider.onValueChanged.AddListener(OnSliderChanged);

        // Initialise with current skin tone if available
        ApplySkin(slider.value);
    }

    void OnSliderChanged(float value)
    {
        ApplySkin(value);
    }

    void ApplySkin(float value)
    {
        if (avatar == null) return;

        Colour newColour = skinGradient.Evaluate(value); // pick from gradient
        avatar.SetColor("Skin", newColour);
        avatar.BuildCharacter();
    }
}
