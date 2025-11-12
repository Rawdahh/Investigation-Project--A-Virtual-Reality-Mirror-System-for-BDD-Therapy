using UnityEngine;
using UnityEngine.UI; // Required for accessing UI components like the Button

public class GameStartManager : MonoBehaviour
{
    [Header("Avatars to Teleport")]
    [Tooltip("The main VR player rig that needs to be moved.")]
    public Transform firstPersonAvatar;

    [Tooltip("The customized character that the player has created.")]
    public Transform playerAvatar;

    [Header("Destination Points")]
    [Tooltip("The location where the VR player will be teleported.")]
    public Transform firstPersonAvatarSpawnPoint;

    [Tooltip("The location where the customized character will be teleported.")]
    public Transform playerAvatarSpawnPoint;

    [Header("UI Elements")]
    [Tooltip("The entire UI Canvas for the customization screen, which will be hidden.")]
    public GameObject customizationUI;

    // This is the public function that our button will call.
    public void StartGameSequence()
    {
        // Safety Checks
        if (firstPersonAvatar == null || playerAvatar == null || firstPersonAvatarSpawnPoint == null || playerAvatarSpawnPoint == null)
        {
            Debug.LogError("GAME START ERROR: One or more of the required Transforms is not assigned in the Inspector!");
            return; // Stop the function here to prevent errors.
        }

        // Step 1: Hide the Customization UI
        if (customizationUI != null)
        {
            customizationUI.SetActive(false);
        }

        // Step 2: Teleport the Avatars
        // Move the FirstPersonAvatar (the player) to its spawn point.
        firstPersonAvatar.position = firstPersonAvatarSpawnPoint.position;
        firstPersonAvatar.rotation = firstPersonAvatarSpawnPoint.rotation;

        // Move the PlayerAvatar (the customized character) to its spawn point.
        playerAvatar.position = playerAvatarSpawnPoint.position;
        playerAvatar.rotation = playerAvatarSpawnPoint.rotation;

        // Step 3: Confirmation
        Debug.Log("Teleport sequence complete! Player and Avatar have been moved.");
    }
}