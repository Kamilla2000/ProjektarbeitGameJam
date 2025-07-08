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

    [Header("Movement Settings")]
    public float patrolWaitTime = 2f;
    public float patrolRadius = 10f;

    private float lastAttackTime;
    private float patrolTimer;
    private bool isDead = false;
    private Vector3 patrolTarget;

    private Animator animator;
    private NavMeshAgent agent;
    private Transform princess;

    private Eyes eyes;
    private Ears ears;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        eyes = GetComponent<Eyes>();
        ears = GetComponent<Ears>();

        if (healthBar != null)
        {
            healthBar.maxValue = HP;
            healthBar.value = HP;
        }

        GameObject princessObj = GameObject.FindGameObjectWithTag("Princess");
        if (princessObj != null)
            princess = princessObj.transform;

        SetNewPatrolDestination();
    }

    void Update()
    {
        if (isDead || princess == null) return;

        if (healthBar != null)
            healthBar.value = HP;

        float distance = Vector3.Distance(transform.position, princess.position);
        bool canSee = eyes != null && eyes.IsDetecting;
        bool canHear = ears != null && ears.IsDetecting;

        if (canSee || canHear)
        {
            agent.SetDestination(princess.position);
            animator.SetBool("isPatrolling", false);

            if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                animator.SetTrigger("attack");
                lastAttackTime = Time.time;

                PlayerHealthPA health = princess.GetComponent<PlayerHealthPA>();
                if (health != null)
                {
                    health.TakeDamege(attackDamage);
                }
            }
        }
        else
        {
            HandlePatrol();
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

        animator.SetBool("isPatrolling", true);
        agent.isStopped = false;
    }

    void SetNewPatrolDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius + transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit navHit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = navHit.position;
            agent.SetDestination(patrolTarget);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        HP -= amount;
        if (HP <= 0)
        {
            HP = 0;
            Die();
        }
        else
        {
            animator.SetTrigger("damage");
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("die");
        agent.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) Destroy(rb);

        Invoke(nameof(SpawnHeart), 2f);
    }

    void SpawnHeart()
    {
        if (heartPickupPrefab != null)
            Instantiate(heartPickupPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
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

    public bool IsDead() => isDead;
}
