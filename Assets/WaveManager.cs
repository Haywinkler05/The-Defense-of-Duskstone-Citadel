using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaveManager : MonoBehaviour
{
    [Header("⚔️ Wave Settings")]
    public GameObject skeletonPrefab;       // Skeleton enemy prefab
    public Transform spawnPoint;            // Where they spawn (bridge entrance)
    public Transform playerTarget;          // OVR Rig or player
    public int baseEnemiesPerWave = 3;      // Wave 1 = 3, Wave 2 = 6, etc.
    public int maxWaves = 7;                // Total number of waves
    public float waveInterval = 15f;        // Time between waves
    public float startDelay = 30f;          // Delay before first wave

    private int waveCount = 0;
    private bool finished = false;

    void Start()
    {
        // Auto-assign the player target at runtime if not set in Inspector
        if (playerTarget == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTarget = playerObj.transform;
                Debug.Log("[WaveManager] Automatically found player target: " + playerTarget.name);
            }
            else
            {
                Debug.LogWarning("[WaveManager] No object with tag 'Player' found! Skeletons won't move.");
            }
        }

        // Start the timed wave spawner
        StartCoroutine(WaveSpawner());
    }

    private IEnumerator WaveSpawner()
    {
        // Wait for intro or cutscene before spawning enemies
        yield return new WaitForSeconds(startDelay);

        while (!finished)
        {
            SpawnWave();

            if (waveCount >= maxWaves)
            {
                finished = true;
                Debug.Log("[WaveManager] 🏰 All waves complete! The Citadel is safe!");
                yield break;
            }

            // Wait for the interval before the next wave
            yield return new WaitForSeconds(waveInterval);
        }
    }

    private void SpawnWave()
    {
        waveCount++;
        int enemiesThisWave = baseEnemiesPerWave * waveCount;
        Debug.Log($"[WaveManager] Spawning Wave {waveCount}/{maxWaves} ({enemiesThisWave} skeletons)");

        for (int i = 0; i < enemiesThisWave; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
            Vector3 spawnPos = spawnPoint.position + offset;

            // Create skeleton instance
            GameObject skeleton = Instantiate(skeletonPrefab, spawnPos, spawnPoint.rotation);

            // Ensure NavMeshAgent exists
            NavMeshAgent agent = skeleton.GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                agent = skeleton.AddComponent<NavMeshAgent>();
                agent.speed = 2f;
                agent.acceleration = 8f;
                agent.angularSpeed = 120f;
                agent.stoppingDistance = 1.5f;
            }

            // Add follow script if not present
            SkeletonFollow follow = skeleton.GetComponent<SkeletonFollow>();
            if (follow == null)
                follow = skeleton.AddComponent<SkeletonFollow>();

            // Assign player target
            follow.target = playerTarget;

            Debug.Log($"[WaveManager] Spawned skeleton {i + 1}/{enemiesThisWave} targeting {playerTarget.name}.");
        }
    }
}
