using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaveManager : MonoBehaviour
{
    [Header("⚔️ Wave Settings")]
    public GameObject skeletonPrefab;
    public Transform[] spawnPoints;
    public Transform playerTarget;
    public int baseEnemiesPerWave = 3;
    public int totalWaves = 3;          // 🔹 Built around 3 waves
    public float startDelay = 10f;

    [Header("🎵 Audio Settings (Optional)")]
    public AudioSource battleMusic;

    // 🔊 EVENT: Triggered when all waves are done
    public delegate void WaveEvent();
    public static event WaveEvent OnAllWavesComplete;

    private int currentWave = 0;
    private List<GameObject> activeSkeletons = new List<GameObject>();
    private bool allWavesStarted = false;

    void Start()
    {
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

        if (battleMusic != null && !battleMusic.isPlaying)
            battleMusic.Play();

        allWavesStarted = true;

        while (currentWave < totalWaves)
        {
            currentWave++;
            yield return StartCoroutine(SpawnWave(currentWave));

            // Wait for all skeletons in this wave to die
            yield return new WaitUntil(() => activeSkeletons.Count == 0);

            Debug.Log($"[WaveManager] ✅ Wave {currentWave} complete!");

            // If this was the final wave
            if (currentWave == totalWaves)
            {
                Debug.Log("[WaveManager] 🏁 Final wave cleared!");
                OnAllWavesComplete?.Invoke(); // 🔔 Send event after 3rd wave
            }

            yield return new WaitForSeconds(2f); // small gap between waves
        }
    }

    private IEnumerator SpawnWave(int waveNumber)
    {
        int enemiesThisWave = baseEnemiesPerWave * waveNumber;
        if (waveNumber == totalWaves)
            enemiesThisWave *= 2; // make final wave big

        Debug.Log($"[WaveManager] ⚔️ Spawning Wave {waveNumber}/{totalWaves} with {enemiesThisWave} skeletons");

        for (int i = 0; i < enemiesThisWave; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Vector3 offset = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
            Vector3 spawnPos = spawnPoint.position + offset;

            GameObject skeleton = Instantiate(skeletonPrefab, spawnPos, spawnPoint.rotation);
            activeSkeletons.Add(skeleton);

            SkeletonHit hit = skeleton.GetComponent<SkeletonHit>();
            if (hit == null) hit = skeleton.AddComponent<SkeletonHit>();
            StartCoroutine(RemoveOnDeath(skeleton));

            NavMeshAgent agent = skeleton.GetComponent<NavMeshAgent>() ?? skeleton.AddComponent<NavMeshAgent>();
            SkeletonFollow follow = skeleton.GetComponent<SkeletonFollow>() ?? skeleton.AddComponent<SkeletonFollow>();
            follow.target = playerTarget;

            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator RemoveOnDeath(GameObject skeleton)
    {
        // Wait until destroyed
        while (skeleton != null)
            yield return null;

        activeSkeletons.RemoveAll(s => s == null);

        // Debug message for tracking
        Debug.Log($"[WaveManager] ☠️ Skeleton destroyed. Remaining: {activeSkeletons.Count}");

        // If no skeletons remain AND we’re in the last wave → trigger victory (failsafe)
        if (allWavesStarted && currentWave == totalWaves && activeSkeletons.Count == 0)
        {
            Debug.Log("[WaveManager] 🏆 Last skeleton of last wave defeated!");
            OnAllWavesComplete?.Invoke();
        }
    }
}
