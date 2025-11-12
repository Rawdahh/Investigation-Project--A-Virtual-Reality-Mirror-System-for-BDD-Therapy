using UnityEngine;
using UnityEngine.UI;
using UMA;
using UMA.CharacterSystem;

public class UMA_DNASlider : MonoBehaviour
{
    public DynamicCharacterAvatar avatar;
    public string dnaName = "height";
    public Slider slider;

    // track if the user has ever moved this slider.
    public bool hasBeenAdjusted { get; private set; }


    void OnEnable()
    {
        if (slider != null)
            slider.onValueChanged.AddListener(OnSliderChanged);

        if (avatar != null)
            avatar.CharacterCreated.AddListener(OnAvatarCreated);  // wait for UMA to build
    }

    void OnDisable()
    {
        if (slider != null)
            slider.onValueChanged.RemoveListener(OnSliderChanged);

        if (avatar != null)
            avatar.CharacterCreated.RemoveListener(OnAvatarCreated);
    }

    void OnAvatarCreated(UMAData data)
    {
        var dna = avatar.GetDNA();
        if (dna != null && dna.ContainsKey(dnaName))
        {
            slider.SetValueWithoutNotify(dna[dnaName].Value); // sync slider to DNA
        }
    }

    void OnSliderChanged(float value)
    {
        hasBeenAdjusted = true;

        Debug.Log("Slider for '" + dnaName + "' changed to: " + value);

        if (avatar == null) return;

        var dna = avatar.GetDNA();
        if (dna != null && dna.ContainsKey(dnaName))
        {
            // Set the DNA value
            dna[dnaName].Set(value);

            //dna[dnaName].Value = value;
            //avatar.BuildCharacter(); // rebuild with updated DNA
        }
    }
}
