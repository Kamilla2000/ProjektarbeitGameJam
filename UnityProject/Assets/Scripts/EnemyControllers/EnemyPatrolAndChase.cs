using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolAndChase : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;        // Points to patrol between
    public float patrolWaitTime = 2f;       // Wait time at each patrol point

    [Header("Combat Settings")]
    public float attackRange = 2.5f;        // Distance at which enemy can attack
    public float attackDamage = 10f;        // Damage dealt to player
    public float attackCooldown = 2f;       // Time between attacks

    [Header("Senses")]
    public Eyes eyes;                       // Vision detection script
    public Ears ears;                       // Hearing detection script

    private int currentPatrolIndex = 0;     // Index of the current patrol point
    private float patrolTimer = 0f;         // Timer to wait at patrol point
    private float lastAttackTime = 0f;      // Time of last attack
    private bool isChasing = false;         // Is enemy chasing the player
    private bool isDying = false;           // Is enemy currently dying

    private NavMeshAgent agent;             // NavMeshAgent for movement
    private Animator animator;              // Animator to play animations
    private Transform player;               // Reference to the player
    private EnemyChasingDieAndDamage healthSystem; // Enemy's health script

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        healthSystem = GetComponent<EnemyChasingDieAndDamage>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        MoveToNextPatrolPoint();
    }

    void Update()
    {
        // Stop all behavior if dead
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

        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Detect player through vision or hearing
        if ((eyes != null && eyes.IsDetecting) || (ears != null && ears.IsDetecting))
        {
            isChasing = true;
            agent.isStopped = false;
            agent.SetDestination(player.position);

            animator.SetBool("isChasing", true);
            animator.SetBool("isPatrolling", false);

            // Attack if in range and cooldown passed
            if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                lastAttackTime = Time.time;

                if (player.TryGetComponent(out PlayerHealthPA health))
                {
                    health.TakeDamege(attackDamage);
                }
            }
        }
        else
        {
            // Return to patrolling if player is not detected
            isChasing = false;
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        float distanceToPoint = Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position);

        // Wait at patrol point
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
            // Walk toward patrol point
            agent.isStopped = false;
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
            animator.SetBool("isPatrolling", true);
            animator.SetBool("isChasing", false);
        }
    }

    // Moves to the next patrol destination
    void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    // Destroys enemy after death animation is finished
    private System.Collections.IEnumerator DestroyAfterDeath()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
