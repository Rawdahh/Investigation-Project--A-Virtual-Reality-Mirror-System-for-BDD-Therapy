using UnityEngine;
using UMA;
using UMA.CharacterSystem;

public class UMAAnimatorAssigner : MonoBehaviour
{
    [Tooltip("Drag the DynamicCharacterAvatar (PlayerAvatar) here")]
    public DynamicCharacterAvatar avatar;

    [Tooltip("Drag your AnimatorController (e.g. Locomotion.controller) here")]
    public RuntimeAnimatorController animatorController;

    void OnEnable()
    {
        if (avatar != null)
        {
            avatar.CharacterCreated.AddListener(OnAvatarCreated);
            avatar.CharacterUpdated.AddListener(OnAvatarUpdated);
        }
    }

    void OnDisable()
    {
        if (avatar != null)
        {
            avatar.CharacterCreated.RemoveListener(OnAvatarCreated);
            avatar.CharacterUpdated.RemoveListener(OnAvatarUpdated);
        }
    }

    void OnAvatarCreated(UMAData data)
    {
        AssignAnimator();
    }

    void OnAvatarUpdated(UMAData data)
    {
        // sometimes Animator is recreated/changed on updates so we make sure it's assigned
        AssignAnimator();
    }

    void AssignAnimator()
    {
        if (avatar == null)
        {
            Debug.LogWarning("UMAAnimatorAssigner: avatar is null.");
            return;
        }

        // Find an Animator on the avatar or its children
        Animator anim = avatar.GetComponentInChildren<Animator>(includeInactive: true);

        if (anim == null)
        {
            // If none found, add one to the avatar root
            anim = avatar.gameObject.AddComponent<Animator>();
            Debug.Log("UMAAnimatorAssigner: Added Animator component to avatar root.");
        }

        if (animatorController != null)
        {
            if (anim.runtimeAnimatorController != animatorController)
            {
                anim.runtimeAnimatorController = animatorController;
                Debug.Log("UMAAnimatorAssigner: Assigned RuntimeAnimatorController to Animator.");
            }
        }
        else
        {
            Debug.LogWarning("UMAAnimatorAssigner: animatorController not assigned in inspector.");
        }
    }
}
