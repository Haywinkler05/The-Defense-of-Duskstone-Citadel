using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRHandKinmatics : MonoBehaviour
{
    [Header("XR Tracking")]
    [SerializeField] private Transform xrCamera;
    [SerializeField] private Transform xrLeftHand;
    [SerializeField] private Transform xrRightHand;

    [Header("IK Targets")]
    [SerializeField] private Transform headTarget;
    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private Transform rightHandTarget;

    [Header("Offsets (Optional)")]
    [SerializeField] private Vector3 headPositionOffset = Vector3.zero;
    [SerializeField] private Vector3 handPositionOffset = Vector3.zero;


    void Start()
    {
        // Force enable XR hand objects
        if (xrLeftHand != null) xrLeftHand.gameObject.SetActive(true);
        if (xrRightHand != null) xrRightHand.gameObject.SetActive(true);
    }
    private void LateUpdate()
    {
        if (xrCamera != null && headTarget != null)
        {
            headTarget.position = xrCamera.position + headPositionOffset;
            headTarget.rotation = xrCamera.rotation;
        }

        // Update left hand target
        if (xrLeftHand != null && leftHandTarget != null)
        {
            leftHandTarget.position = xrLeftHand.position + handPositionOffset;
            leftHandTarget.rotation = xrLeftHand.rotation;
        }

        // Update right hand target
        if (xrRightHand != null && rightHandTarget != null)
        {
            rightHandTarget.position = xrRightHand.position + handPositionOffset;
            rightHandTarget.rotation = xrRightHand.rotation;
        }
    }
}
