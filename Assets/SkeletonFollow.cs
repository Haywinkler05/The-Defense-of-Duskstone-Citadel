using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class SkeletonFollow : MonoBehaviour
{
    public Transform target;
    public float attackRange = 2f;
    public float attackCooldown = 2f;

    private NavMeshAgent agent;
    private Animator animator;
    private float lastAttackTime;
    private bool hasDoneFinalJump = false; // prevents repeated jump attacks

    void OnEnable()
    {
        WaveManager.activeEnemies++;
    }

    void OnDestroy()
    {
        WaveManager.activeEnemies = Mathf.Max(0, WaveManager.activeEnemies - 1);
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
        }
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        // Follow player until in range
        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsAttacking", false);
        }
        else
        {
            agent.isStopped = true;
            animator.SetBool("IsWalking", false);

            // 🔹 If this is the final skeleton — perform special attack once
            if (WaveManager.activeEnemies <= 1 && !hasDoneFinalJump)
            {
                StartCoroutine(FinalJumpAttack());
                hasDoneFinalJump = true;
                return;
            }

            // 🔹 Otherwise regular melee attack
            if (Time.time > lastAttackTime + attackCooldown)
            {
                animator.SetBool("IsAttacking", true);
                lastAttackTime = Time.time;
            }
            else
            {
                animator.SetBool("IsAttacking", false);
            }
        }
    }

    private IEnumerator FinalJumpAttack()
    {
        // Trigger jump animation
        animator.ResetTrigger("JumpAttack");
        animator.SetTrigger("JumpAttack");
        Debug.Log("[SkeletonFollow] Performing FINAL Jump Attack!");

        // Optional: slow motion cinematic
        Time.timeScale = 0.4f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Wait in real time so slow motion doesn’t affect it
        yield return new WaitForSecondsRealtime(2f);

        // Restore time
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // Prevent AI from attacking again
        animator.SetBool("IsAttacking", false);
    }
}
