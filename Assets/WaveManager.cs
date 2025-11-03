using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public GameObject skeletonPrefab;
    public Transform[] spawnPoints;           // 🔹 Multiple spawn points
    public Transform playerTarget;
    public int baseEnemiesPerWave = 3;
    public int maxWaves = 7;
    public float startDelay = 10f;

    [Header("Audio Settings")]
    public AudioSource battleMusic;           // 🔹 Assign in Inspector
    public float finalWaveJumpTime = 60f;     // 🔹 Time (in seconds) to jump to

    private int waveCount = 0;
    private bool spawning = false;
    private List<GameObject> activeSkeletons = new List<GameObject>();

    void Start()
    {
        // Auto-find player if not assigned
        if (playerTarget == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                playerTarget = playerObj.transform;
            else
                Debug.LogWarning("[WaveManager] No Player tagged object found!");
        }

        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
        yield return new WaitForSeconds(startDelay);

        while (waveCount < maxWaves)
        {
            // If this is the final wave, jump audio before spawning
            if (waveCount == maxWaves - 1 && battleMusic != null)
            {
                Debug.Log("[WaveManager] 🎶 Final wave approaching — jumping music!");
                if (!battleMusic.isPlaying)
                    battleMusic.Play();

                battleMusic.time = finalWaveJumpTime;
            }

            yield return StartCoroutine(SpawnWave());

            // Wait until all skeletons from current wave are dead
            yield return new WaitUntil(() => activeSkeletons.Count == 0);

            waveCount++;
        }

        Debug.Log("[WaveManager] 🏰 All waves complete! The Citadel stands strong!");
    }

    private IEnumerator SpawnWave()
    {
        spawning = true;

        // Calculate enemy count (2x if last wave)
        int enemiesThisWave = baseEnemiesPerWave * (waveCount + 1);
        if (waveCount == maxWaves - 1)
            enemiesThisWave *= 2;

        Debug.Log($"[WaveManager] ⚔️ Spawning Wave {waveCount + 1}/{maxWaves} ({enemiesThisWave} skeletons)");

        for (int i = 0; i < enemiesThisWave; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Vector3 offset = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
            Vector3 spawnPos = spawnPoint.position + offset;

            GameObject skeleton = Instantiate(skeletonPrefab, spawnPos, spawnPoint.rotation);

            // Track the skeleton
            activeSkeletons.Add(skeleton);

            // Cleanup on death
            SkeletonHit hit = skeleton.GetComponent<SkeletonHit>();
            if (hit == null) hit = skeleton.AddComponent<SkeletonHit>();
            StartCoroutine(RemoveOnDeath(skeleton));

            // Ensure AI movement
            NavMeshAgent agent = skeleton.GetComponent<NavMeshAgent>();
            if (agent == null)
                agent = skeleton.AddComponent<NavMeshAgent>();

            SkeletonFollow follow = skeleton.GetComponent<SkeletonFollow>();
            if (follow == null)
                follow = skeleton.AddComponent<SkeletonFollow>();

            follow.target = playerTarget;

            yield return new WaitForSeconds(0.25f); // Small delay between spawns
        }

        spawning = false;
    }

    private IEnumerator RemoveOnDeath(GameObject skeleton)
    {
        // Wait until destroyed by SkeletonHit
        while (skeleton != null)
            yield return null;

        activeSkeletons.RemoveAll(s => s == null);
    }
}
