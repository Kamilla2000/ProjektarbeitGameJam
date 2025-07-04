using UnityEngine;
using UnityEngine.AI;

public class BearPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;              // List of patrol points
    public float patrolWaitTime = 2f;             // Time to wait at each patrol point

    [Header("Chase & Attack Settings")]
    public float viewDistance = 10f;              // How far the bear can see the player
    public float losePlayerDistance = 15f;        // How far the player has to be for the bear to stop chasing
    public float viewAngle = 120f;                // Field of view (angle) for detecting player
    public float attackDistance = 2f;             // Distance to start attack
    public float attackCooldown = 2f;             // Time between attacks
    public int attackDamage = 10;                 // Damage dealt to player

    private int currentPatrolIndex;
    private float patrolTimer;
    private float attackTimer;
    private bool isChasing = false;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private PlayerHealthPA playerHealth;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealthPA>();
        }

        MoveToNextPatrolPoint(); // Start patrolling
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        attackTimer += Time.deltaTime;

        // Check if player is within field of view and range
        bool canSeePlayer = distanceToPlayer <= viewDistance && angleToPlayer <= viewAngle / 2f;

        // Start chasing if player is visible
        if (canSeePlayer)
        {
            isChasing = true;
        }

        // Stop chasing if player is too far
        if (distanceToPlayer > losePlayerDistance)
        {
            isChasing = false;
        }

        // If close enough, stop and attack
        if (distanceToPlayer <= attackDistance && isChasing)
        {
            agent.SetDestination(transform.position); // Stop moving
            animator.SetBool("isWalking", false);

            if (attackTimer >= attackCooldown)
            {
                AttackPlayer();
                attackTimer = 0f;
            }
        }
        // If chasing, move toward player
        else if (isChasing)
        {
            agent.SetDestination(player.position);
            animator.SetBool("isWalking", true);
        }
        // If not chasing, continue patrol
        else
        {
            Patrol();
        }

        // Update walking animation based on movement
        animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolWaitTime)
            {
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                MoveToNextPatrolPoint();
                patrolTimer = 0f;
            }
        }
    }

    private void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        animator.SetBool("isWalking", true);
    }

    private void AttackPlayer()
    {
        animator.SetTrigger("Attack");

        if (playerHealth != null)
        {
            playerHealth.TakeDamege(attackDamage);
        }
    }

    // Show vision range in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, losePlayerDistance);

        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewDistance);
    }
}
