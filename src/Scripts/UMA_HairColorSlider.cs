using UnityEngine;
using UnityEngine.UI;
using UMA.CharacterSystem;

public class UMA_HairColorSlider : MonoBehaviour
{
    public DynamicCharacterAvatar avatar;  // drag UMA avatar
    public Slider slider;                  // drag the UI slider
    public string colourName = "Hair";      // UMA colour slot name
    public Gradient hairGradient;          // assign colorur in Inspector

    void Start()
    {
        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = 0.5f;
            slider.onValueChanged.AddListener(OnSliderChanged);
        }
    }

    void OnSliderChanged(float value)
    {
        if (avatar == null || hairGradient == null) return;

        Colour newColour = hairGradient.Evaluate(value);
        avatar.SetColor(colourName, newColour);
        avatar.BuildCharacter(true);
    }
}
