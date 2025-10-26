using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightHandTrackingDriver : MonoBehaviour
{
    [Header("Source Hands (Grey Visualizer)")]
    [SerializeField] private Transform sourceLeftWrist;
    [SerializeField] private Transform sourceRightWrist;

    [Header("Target Hands (Knight)")]
    [SerializeField] private Transform targetLeftHand;
    [SerializeField] private Transform targetRightHand;

    [Header("Settings")]
    [SerializeField] private bool copyLeftHand = true;
    [SerializeField] private bool copyRightHand = true;
    [SerializeField] private bool showDebugInfo = false;

    // Bone name mappings - XR Hand to Mixamo
    private readonly string[][] fingerNames = new string[][]
    {
        // Thumb
        new string[] { "Thumb", "HandThumb" },
        // Index
        new string[] { "Index", "HandIndex" },
        // Middle
        new string[] { "Middle", "HandMiddle" },
        // Ring
        new string[] { "Ring", "HandRing" },
        // Pinky
        new string[] { "Little", "HandPinky" }
    };

    private readonly string[] segmentNames = new string[] { "Proximal", "Intermediate", "Distal", "Tip" };
    private readonly string[] mixamoSegments = new string[] { "1", "2", "3", "4" };

    void Start()
    {
        if (showDebugInfo)
        {
            Debug.Log("HandPoseCopier initialized");
            if (sourceLeftWrist != null) Debug.Log($"Source left wrist: {sourceLeftWrist.name}");
            if (targetLeftHand != null) Debug.Log($"Target left hand: {targetLeftHand.name}");
        }
    }

    void LateUpdate()
    {
        if (copyLeftHand && sourceLeftWrist != null && targetLeftHand != null)
        {
            CopyHandPose(sourceLeftWrist, targetLeftHand, "L_");
        }

        if (copyRightHand && sourceRightWrist != null && targetRightHand != null)
        {
            CopyHandPose(sourceRightWrist, targetRightHand, "R_");
        }
    }

    void CopyHandPose(Transform sourceWrist, Transform targetHand, string prefix)
    {
        // Copy each finger
        for (int finger = 0; finger < fingerNames.Length; finger++)
        {
            string xrFingerName = fingerNames[finger][0];
            string mixamoFingerName = fingerNames[finger][1];

            // Copy each segment of the finger
            for (int segment = 0; segment < segmentNames.Length; segment++)
            {
                // Build source bone name (e.g., "R_IndexProximal")
                string sourceBoneName = prefix + xrFingerName;
                if (segment == 0 && finger == 0)
                {
                    // Thumb metacarpal
                    sourceBoneName += "Metacarpal";
                }
                else if (segment == 0)
                {
                    // Other fingers metacarpal
                    sourceBoneName += "Metacarpal";
                }
                else
                {
                    sourceBoneName += segmentNames[segment];
                }

                // Build target bone name (e.g., "mixamorig:LeftHandIndex1")
                string targetBoneName = "mixamorig:" +
                    (prefix == "L_" ? "Left" : "Right") +
                    mixamoFingerName +
                    mixamoSegments[segment];

                // Find and copy rotation
                Transform sourceBone = FindChildRecursive(sourceWrist, sourceBoneName);
                Transform targetBone = FindChildRecursive(targetHand, targetBoneName);

                if (sourceBone != null && targetBone != null)
                {
                    targetBone.localRotation = sourceBone.localRotation;
                }
                else if (showDebugInfo && sourceBone == null)
                {
                    Debug.LogWarning($"Could not find source bone: {sourceBoneName}");
                }
                else if (showDebugInfo && targetBone == null)
                {
                    Debug.LogWarning($"Could not find target bone: {targetBoneName}");
                }
            }
        }
    }

    Transform FindChildRecursive(Transform parent, string name)
    {
        if (parent.name == name)
            return parent;

        foreach (Transform child in parent)
        {
            Transform result = FindChildRecursive(child, name);
            if (result != null)
                return result;
        }

        return null;
    }
}
