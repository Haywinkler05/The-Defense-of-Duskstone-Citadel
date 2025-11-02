using UnityEngine;
using UnityEngine.AI;

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
}
