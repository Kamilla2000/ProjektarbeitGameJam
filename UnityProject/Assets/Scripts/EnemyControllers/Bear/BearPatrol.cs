using UnityEngine;
using UnityEngine.AI;

public class BearPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;              // Array of patrol points to walk between
    public float patrolWaitTime = 2f;             // Time to wait at each patrol point

    [Header("Chase & Attack Settings")]
    public float viewDistance = 10f;              // How far the bear can see the player
    public float losePlayerDistance = 15f;        // Distance at which the bear stops chasing the player
    public float viewAngle = 120f;                // Field of view angle for detecting the player
    public float attackDistance = 2f;             // Distance at which the bear starts attacking
    public float attackCooldown = 2f;             // Time between attacks
    public int attackDamage = 10;                 // Damage dealt per attack

    private int currentPatrolIndex;
    private float patrolTimer;
    private float attackTimer;
    private bool isChasing = false;

    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private PlayerHealthPA playerHealth;

    private bool isDead = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();          // Get NavMeshAgent component for movement
        animator = GetComponent<Animator>();            // Get Animator component for animations
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Find player by tag

        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealthPA>();       // Get player health component
        }

        MoveToNextPatrolPoint();                          // Begin patrolling
    }

    private void Update()
    {
        if (player == null) return;

        // Calculate distance and direction to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        attackTimer += Time.deltaTime;

        // Check if player is within sight distance and inside field of view angle
        bool canSeePlayer = distanceToPlayer <= viewDistance && angleToPlayer <= viewAngle / 2f;

        // Start chasing player if visible
        if (canSeePlayer)
        {
            isChasing = true;
        }

        // Stop chasing if player is too far
        if (distanceToPlayer > losePlayerDistance)
        {
            isChasing = false;
        }

        // Attack if player is within attack range and bear is chasing
        if (distanceToPlayer <= attackDistance && isChasing)
        {
            agent.SetDestination(transform.position); // Stop moving to attack
            animator.SetBool("isWalking", false);

            if (attackTimer >= attackCooldown)
            {
                AttackPlayer();
                attackTimer = 0f;
            }
        }
        // Continue chasing player if still chasing
        else if (isChasing)
        {
            agent.SetDestination(player.position);
            animator.SetBool("isWalking", true);
        }
        // Otherwise, continue patrolling
        else
        {
            Patrol();
        }

        // Update walking animation based on movement velocity
        animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        // Wait at patrol point, then move to next
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
        animator.SetTrigger("Attack");     // Trigger attack animation

        if (playerHealth != null)
        {
            playerHealth.TakeDamege(attackDamage);  // Apply damage to player
        }
    }

    // Draw vision cones in the editor to visualize detection ranges
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);        // View distance radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, losePlayerDistance);  // Lose player distance radius

        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewDistance);
    }

    // Health and damage handling
    public float health = 100f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return; // Prevent multiple calls
        isDead = true;

        animator.SetBool("isDead", true);  // Trigger death animation

        agent.isStopped = true; // Stop NavMeshAgent movement
        agent.enabled = false;  // Optionally disable agent after death

        // Optionally disable other components like colliders, attacks, etc.

        // Destroy game object after animation length (example: 3 seconds)
        Destroy(gameObject, 3f);
    }
}
