using UnityEngine;
using System.Collections;

public class IntroSequence : MonoBehaviour
{
    [Header("King Dialogue Settings")]
    public AudioSource kingVoice;          // AudioSource for the King's voice
    public AudioClip introClip;            // The dialogue clip
    public float dialogueDelay = 2f;       // Delay before the King starts speaking

    [Header("Door Settings")]
    public Animator doorAnimator;          // Animator controlling the castle doors

    private bool sequenceStarted = false;

    void Start()
    {
        StartCoroutine(BeginIntro());
    }

    private IEnumerator BeginIntro()
    {
        if (sequenceStarted) yield break;
        sequenceStarted = true;

        Debug.Log("[IntroSequence] Beginning intro sequence...");

        if (kingVoice == null || introClip == null)
        {
            Debug.LogError("[IntroSequence] Missing King voice or intro clip!");
            yield break;
        }

        // --- Wait before the King starts speaking ---
        Debug.Log($"[IntroSequence] Waiting {dialogueDelay:F1}s before dialogue starts...");
        yield return new WaitForSeconds(dialogueDelay);

        // --- Configure immersive 3D audio ---
        kingVoice.clip = introClip;
        kingVoice.spatialBlend = 1f;                     // Full 3D sound
        kingVoice.rolloffMode = AudioRolloffMode.Logarithmic;
        kingVoice.minDistance = 5f;
        kingVoice.maxDistance = 40f;
        kingVoice.dopplerLevel = 0f;
        kingVoice.Play();

        Debug.Log($"[IntroSequence] Playing 3D King dialogue: {introClip.name} | Duration: {kingVoice.clip.length:F1}s");

        // Wait until the clip finishes
        yield return new WaitWhile(() => kingVoice.isPlaying);

        Debug.Log("[IntroSequence] Dialogue finished. Opening castle gates...");

        // --- Trigger the Door Animation ---
        if (doorAnimator != null)
        {
            if (doorAnimator.HasParameterOfType("Open", AnimatorControllerParameterType.Trigger))
            {
                doorAnimator.SetTrigger("Open");
                Debug.Log("[IntroSequence] Door 'Open' trigger sent successfully.");
            }
            else
            {
                Debug.LogWarning("[IntroSequence] Animator missing 'Open' trigger parameter!");
            }
        }
        else
        {
            Debug.LogError("[IntroSequence] Door Animator reference missing!");
        }

        Debug.Log("[IntroSequence] Intro sequence completed.");
    }
}

public static class AnimatorExtensions
{
    public static bool HasParameterOfType(this Animator self, string name, AnimatorControllerParameterType type)
    {
        foreach (var param in self.parameters)
        {
            if (param.type == type && param.name == name)
                return true;
        }
        return false;
    }
}
