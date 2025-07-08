using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolAndChase : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;

    [Header("Combat Settings")]
    public float attackRange = 2.5f;
    public float attackDamage = 10f;
    public float attackCooldown = 2f;

    [Header("Senses")]
    public Eyes eyes;
    public Ears ears;

    private int currentPatrolIndex = 0;
    private float patrolTimer = 0f;
    private float lastAttackTime = 0f;
    private bool isChasing = false;
    private bool isDying = false;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform princess;
    private EnemyChasingDieAndDamage healthSystem;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<EnemyChasingDieAndDamage>();
        princess = GameObject.FindGameObjectWithTag("Princess")?.transform;

        MoveToNextPatrolPoint();
    }

    void Update()
    {
        if (healthSystem != null && healthSystem.IsDead())
        {
            if (!isDying)
            {
                isDying = true;
                agent.isStopped = true;
                animator.SetBool("isPatrolling", false);
                animator.SetBool("isChasing", false);
                animator.SetTrigger("die");
                StartCoroutine(DestroyAfterDeath());
            }
            return;
        }

        if (princess == null) return;

        float distanceToPrincess = Vector3.Distance(transform.position, princess.position);

        if ((eyes != null && eyes.IsDetecting) || (ears != null && ears.IsDetecting))
        {
            isChasing = true;
            agent.isStopped = false;
            agent.SetDestination(princess.position);

            animator.SetBool("isChasing", true);
            animator.SetBool("isPatrolling", false);

            if (distanceToPrincess <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;

                if (princess.TryGetComponent(out PlayerHealthPA health))
                {
                    health.TakeDamege(attackDamage);
                }
            }
        }
        else
        {
            isChasing = false;
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        float distanceToPoint = Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position);

        if (distanceToPoint <= 0.5f)
        {
            agent.isStopped = true;
            animator.SetBool("isPatrolling", false);
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolWaitTime)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                MoveToNextPatrolPoint();
                patrolTimer = 0f;
            }
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            animator.SetBool("isPatrolling", true);
            animator.SetBool("isChasing", false);
        }
    }

    void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    private System.Collections.IEnumerator DestroyAfterDeath()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}