using UnityEngine;
using UMA;
using UMA.CharacterSystem;

public class UMADNADebugger : MonoBehaviour
{
    public DynamicCharacterAvatar avatar;

    void OnEnable()
    {
        if (avatar != null)
        {
            avatar.CharacterCreated.AddListener(OnAvatarCreated);
        }
    }

    void OnDisable()
    {
        if (avatar != null)
        {
            avatar.CharacterCreated.RemoveListener(OnAvatarCreated);
        }
    }

    void OnAvatarCreated(UMAData data)
    {
        var dna = avatar.GetDNA();
        if (dna != null)
        {
            Debug.Log("---- UMA DNA Keys ----");
            foreach (var key in dna.Keys)
            {
                Debug.Log(key + " = " + dna[key].Value);
            }
            Debug.Log("----------------------");
        }
        else
        {
            Debug.LogWarning("No DNA found on avatar!");
        }
    }
}
