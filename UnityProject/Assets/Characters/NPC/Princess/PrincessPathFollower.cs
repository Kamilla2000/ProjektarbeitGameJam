using UnityEngine;
using UnityEngine.AI;

public class PrincessPathFollower : MonoBehaviour
{
    public Transform[] pathPoints;
    public float stopDistance = 0.3f;

    private NavMeshAgent agent;
    private Animator animator;
    private PlayerHealthPA health;
    private int currentTargetIndex = 0;
    private bool hasStartedWalking = false;
    private float delayBeforeWalking = 10f;
    private float timer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health = GetComponent<PlayerHealthPA>();

        if (animator != null)
        {
            animator.SetBool("isWalking", false);
        }

        agent.enabled = false;
    }

    void Update()
    {
        if (!hasStartedWalking)
        {
            timer += Time.deltaTime;
            if (timer >= delayBeforeWalking)
            {
                StartWalking();
            }
            return;
        }

         
        if (health != null && health.isKissed)
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
            return;
        }

         
        if (agent.isStopped && health != null && !health.isKissed)
        {
            agent.isStopped = false;
            animator.SetBool("isWalking", true);
        }

        if (!agent.pathPending && agent.remainingDistance < stopDistance)
        {
            currentTargetIndex++;
            if (currentTargetIndex < pathPoints.Length)
            {
                agent.SetDestination(pathPoints[currentTargetIndex].position);
            }
            else
            {
                agent.ResetPath();
                animator.SetBool("isWalking", false);
            }
        }
    }

    void StartWalking()
    {
        hasStartedWalking = true;
        agent.enabled = true;

        if (pathPoints.Length > 0)
        {
            agent.SetDestination(pathPoints[currentTargetIndex].position);
        }

        animator.SetBool("isWalking", true);
    }
}
