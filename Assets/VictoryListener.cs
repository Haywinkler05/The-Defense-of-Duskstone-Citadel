using UnityEngine;

public class VictoryListener : MonoBehaviour
{
    [Header("🎵 Audio References")]
    public AudioSource bgMusic;
    public AudioSource yayyySound;

    private void OnEnable()
    {
        WaveManager.OnAllWavesComplete += HandleWavesComplete;
    }

    private void OnDisable()
    {
        WaveManager.OnAllWavesComplete -= HandleWavesComplete;
    }

    private void HandleWavesComplete()
    {
        Debug.Log("[VictoryListener] 🏁 Received event: All waves complete!");

        if (bgMusic != null)
        {
            bgMusic.Stop();
            Debug.Log("[VictoryListener] ⏹️ Background music stopped.");
        }

        if (yayyySound != null)
        {
            yayyySound.Play();
            Debug.Log("[VictoryListener] 🎉 YAYYY sound played!");
        }
    }
}
