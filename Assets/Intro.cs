using UnityEngine;
using System.Collections;

public class IntroSequence : MonoBehaviour
{
    [Header("🎙️ King Dialogue Settings")]
    public AudioSource kingVoice;          // AudioSource component for the King's voice
    public AudioClip introClip;            // The intro dialogue clip

    [Header("🚪 Door & Wave Settings")]
    public Animator doorAnimator;          // Animator controlling the castle doors
    public GameObject waveManager;         // The object that spawns the skeletons

    private bool sequenceStarted = false;

    void Start()
    {
        // Make sure skeleton waves are off at start
        if (waveManager != null)
        {
            waveManager.SetActive(false);
            Debug.Log("[IntroSequence] Wave manager disabled at start.");
        }
        else
        {
            Debug.LogWarning("[IntroSequence] Wave manager reference missing!");
        }

        // Start the cinematic intro sequence
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

        // ---- Play the King's dialogue ----
        kingVoice.clip = introClip;
        kingVoice.spatialBlend = 0f; // Force 2D for guaranteed audibility
        kingVoice.volume = 1f;
        kingVoice.Play();

        Debug.Log("[IntroSequence] Playing King dialogue: " + introClip.name +
                  " | Duration: " + kingVoice.clip.length + "s");

        // ---- Wait until the clip finishes completely ----
        while (kingVoice.isPlaying)
        {
            yield return null; // wait each frame until finished
        }

        Debug.Log("[IntroSequence] Dialogue finished. Opening castle gates...");

        // ---- Trigger the Door Animation ----
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

        // ---- Optional: Wait a few seconds before enabling the waves ----
        yield return new WaitForSeconds(2.5f);

        // ---- Enable the Skeleton Waves ----
        if (waveManager != null)
        {
            waveManager.SetActive(true);
            Debug.Log("[IntroSequence] Skeleton waves activated!");
        }

        Debug.Log("[IntroSequence] Intro sequence completed.");
    }
}


// ---- Animator Parameter Helper Extension ----
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
