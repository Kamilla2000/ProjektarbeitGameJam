using UnityEngine;
using UnityEngine.AI;

public class NPCStateMachine : MonoBehaviour
{
    [Header("Patrol")]
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    [Header("Idle Settings")]
    public float idleMinTime = 1f;
    public float idleMaxTime = 3f;
    private float idleTimer = 0f;
    private bool isIdle = false;

    [Header("Angry Settings")]
    public float timeUntilAngry = 10f;
    public float angryDuration = 5f;
    public GameObject[] angryPrefabsToSpawn;
    public Transform[] angrySpawnPoints;
    private float angryTimer = 0f;
    private float angryActiveTimer = 0f;
    private bool isAngry = false;

    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GoToNextWaypoint();
    }

    void Update()
    {
        // ---- Handle Angry Timer ----
        if (!isAngry)
        {
            angryTimer += Time.deltaTime;
            if (angryTimer >= timeUntilAngry)
            {
                EnterAngryState();
            }
        }
        else
        {
            angryActiveTimer += Time.deltaTime;
            if (angryActiveTimer >= angryDuration)
            {
                ExitAngryState();
            }
        }

        // ---- Handle Idle Logic ----
        if (isIdle)
        {
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0f)
            {
                isIdle = false;
                animator.SetBool("isIdle", false);
                GoToNextWaypoint();
            }
        }
        else
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                StartIdle();
            }
        }
    }

    private void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    private void StartIdle()
    {
        isIdle = true;
        idleTimer = Random.Range(idleMinTime, idleMaxTime);
        animator.SetBool("isIdle", true);
        agent.SetDestination(transform.position); // stop moving
    }

    private void EnterAngryState()
    {
        isAngry = true;
        angryActiveTimer = 0f;
        animator.SetBool("isAngry", true);
        agent.speed *= 2f;

        Debug.Log("NPC is ANGRY! 😡");

        // Spawn Prefabs
        for (int i = 0; i < angryPrefabsToSpawn.Length && i < angrySpawnPoints.Length; i++)
        {
            Instantiate(angryPrefabsToSpawn[i], angrySpawnPoints[i].position, Quaternion.identity);
        }
    }

    private void ExitAngryState()
    {
        isAngry = false;
        angryTimer = 0f;
        angryActiveTimer = 0f;
        animator.SetBool("isAngry", false);
        agent.speed /= 2f;

        Debug.Log("NPC calmed down 🧘");
    }
}