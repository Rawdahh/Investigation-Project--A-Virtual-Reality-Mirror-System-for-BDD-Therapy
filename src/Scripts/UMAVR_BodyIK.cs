using UMA;
using UMA.CharacterSystem;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class UMAVR_BodyIK : MonoBehaviour
{
    [Header("VR Targets (Your Hardware)")]
    public Transform headTarget; // VR Camera
    public Transform leftHandTarget; // LeftHand Controller
    public Transform rightHandTarget; // RightHand Controller

    [Header("IK Constraints (From the IK_Rig)")]
    public OverrideTransform headIK;
    public TwoBoneIKConstraint leftHandIK;
    public TwoBoneIKConstraint rightHandIK;

    // We will get this component automatically
    private RigBuilder rigBuilder;

    void Start()
    {
        // Get the Rig Builder component to use later
        rigBuilder = GetComponent<RigBuilder>();

        // Find the UMA Avatar component
        var dca = GetComponent<DynamicCharacterAvatar>();
        if (dca != null)
        {
            // Subscribe to the event that fires ONLY when the character is fully built
            dca.CharacterCreated.AddListener(OnCharacterCreated);
        }
    }

    // This function runs automatically after the UMA skeleton is created
    private void OnCharacterCreated(UMA.UMAData umaData)
    {
        Debug.Log("UMA Character Created. Linking VR IK...");

        //Find the Bones from the new skeleton by name
        Transform headBone = umaData.skeleton.GetBoneGameObject(UMASkeleton.StringToHash("Head")).transform;
        Transform leftHandBone = umaData.skeleton.GetBoneGameObject(UMASkeleton.StringToHash("LeftHand")).transform;
        Transform leftLowerArmBone = umaData.skeleton.GetBoneGameObject(UMASkeleton.StringToHash("LeftForeArm")).transform;
        Transform leftUpperArmBone = umaData.skeleton.GetBoneGameObject(UMASkeleton.StringToHash("LeftUpperArm")).transform;
        Transform rightHandBone = umaData.skeleton.GetBoneGameObject(UMASkeleton.StringToHash("RightHand")).transform;
        Transform rightLowerArmBone = umaData.skeleton.GetBoneGameObject(UMASkeleton.StringToHash("RightForeArm")).transform;
        Transform rightUpperArmBone = umaData.skeleton.GetBoneGameObject(UMASkeleton.StringToHash("RightUpperArm")).transform;

        // Assign Bones and Targets to the IK Constraints

        // Head
        headIK.data.constrainedObject = headBone;
        headIK.data.sourceObject = headTarget;
        headIK.data.space = OverrideTransformData.Space.World;

        // Left Hand
        leftHandIK.data.root = leftUpperArmBone;   // Shoulder
        leftHandIK.data.mid = leftLowerArmBone;    // Elbow
        leftHandIK.data.tip = leftHandBone;      // Hand
        leftHandIK.data.target = leftHandTarget; // Tell the hand to follow the controller

        // Right Hand
        rightHandIK.data.root = rightUpperArmBone;
        rightHandIK.data.mid = rightLowerArmBone;
        rightHandIK.data.tip = rightHandBone;
        rightHandIK.data.target = rightHandTarget;

        // Final Step: Rebuild the rig with the new assignments
        rigBuilder.Build();

        Debug.Log("VR IK Linking Complete!");
    }
}