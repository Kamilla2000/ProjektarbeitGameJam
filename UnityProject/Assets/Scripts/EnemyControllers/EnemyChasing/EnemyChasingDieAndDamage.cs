using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyChasingDieAndDamage : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int HP = 100;
    public Slider healthBar;

    [Header("Death & Pickup")]
    [SerializeField] private GameObject heartPickupPrefab;

    [Header("Attack Settings")]
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public int attackDamage = 10;
    private float lastAttackTime;

    [Header("Movement")]
    public float stoppingDistance = 1.5f;

    private Animator animator;
    private NavMeshAgent agent;
    private bool isDead = false;
    private Transform princessTarget;

    [Header("Patrolling")]
    public float patrolWaitTime = 2f;
    public float patrolRadius = 10f;
    private float patrolTimer;
    private Vector3 patrolTarget;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        healthBar.maxValue = HP;
        healthBar.value = HP;
        agent.stoppingDistance = stoppingDistance;

        GameObject princess = GameObject.FindGameObjectWithTag("Princess");
        if (princess != null)
        {
            princessTarget = princess.transform;
        }
        else
        {
            Debug.LogError("⚠ Kein Objekt mit Tag 'Princess' gefunden.");
        }

        SetNewPatrolDestination();
    }

    void Update()
    {
        healthBar.value = HP;
        if (isDead || princessTarget == null) return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Chase"))
        {
            agent.isStopped = false;
            agent.SetDestination(princessTarget.position);
        }
        else if (stateInfo.IsName("Patrol"))
        {
            HandlePatrol();
        }
        else
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        float distance = Vector3.Distance(transform.position, princessTarget.position);
        if (stateInfo.IsName("Attack") && distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetTrigger("attack");
            lastAttackTime = Time.time;

            PlayerHealthPA princessHealth = princessTarget.GetComponent<PlayerHealthPA>();
            if (princessHealth != null)
            {
                princessHealth.TakeDamege(attackDamage);
            }
        }
    }

    void HandlePatrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolWaitTime)
            {
                SetNewPatrolDestination();
                patrolTimer = 0f;
            }
        }
        else
        {
            patrolTimer = 0f;
        }

        agent.isStopped = false;
    }

    void SetNewPatrolDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = navHit.position;
            agent.SetDestination(patrolTarget);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (isDead) return;

        if (other.CompareTag("Rain"))
        {
            TakeDamage(HP);
        }
        else
        {
            TakeDamage(20);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        HP -= damageAmount;

        if (HP <= 0)
        {
            HP = 0;
            isDead = true;

            animator.SetTrigger("die");

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null) Destroy(rb);

            agent.enabled = false;
            Invoke(nameof(SpawnHeart), 2f);
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }

    private void SpawnHeart()
    {
        if (heartPickupPrefab != null)
        {
            Instantiate(heartPickupPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return isDead;
    }
}