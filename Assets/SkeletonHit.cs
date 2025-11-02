using UnityEngine;

public class SkeletonHit : MonoBehaviour
{
    [Header("Settings")]
    public string swordTag = "Sword";  // The tag of your sword collider
    public GameObject deathEffect;     // Optional particle effect on death (e.g., dust or bones)
    public AudioClip deathSound;       // Optional death sound
    private AudioSource audioSource;

    private bool isDead = false;

    void Start()
    {
        // Add AudioSource automatically if not present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag(swordTag))
        {
            Debug.Log($"[{name}] hit by sword and destroyed!");
            isDead = true;

            // Optional VFX
            if (deathEffect != null)
                Instantiate(deathEffect, transform.position, Quaternion.identity);

            // Optional SFX
            if (deathSound != null)
                audioSource.PlayOneShot(deathSound);

            // Delay destruction slightly if sound is playing
            float delay = (deathSound != null) ? deathSound.length * 0.8f : 0f;
            Destroy(gameObject, delay);
        }
    }
}
