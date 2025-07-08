using UnityEngine;
using UnityEngine.AI;

public class NPCStateMachine : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    private NavMeshAgent _agent;
    private Animator _animator;
    private Transform _player;

    [Header("Angry Settings")]
    public float timeUntilAngry = 10f;
    private float angryTimer = 0f;
    private bool isAngry = false;

    [Header("Angry Enemy Spawn Settings")]
    public GameObject[] angryEnemyPrefabs;
    public int spawnAmount = 3;
    public Collider spawnAreaCollider;

    [Header("Animation Bool States")]
    public bool isIdle = true;

    private float idleTime = 0f;
    public float minIdleDuration = 2f;
    public float maxIdleDuration = 5f;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;

        SetNewIdleDuration();
        GoToNextPatrolPoint();
    }

    void Update()
    {
        // Timer für Angry-Modus
        if (!isAngry)
        {
            angryTimer += Time.deltaTime;
            if (angryTimer >= timeUntilAngry)
            {
                BecomeAngry();
            }
        }

        // Patrouille, wenn nicht angry
        if (!isAngry)
        {
            PatrolLogic();
        }

        // Update Animation States
        _animator.SetBool("isIdle", isIdle);
        _animator.SetBool("isAngry", isAngry);
    }

    void PatrolLogic()
    {
        if (isIdle)
        {
            if (Time.time >= idleTime)
            {
                isIdle = false;
                GoToNextPatrolPoint();
            }
        }
        else
        {
            if (!_agent.pathPending && _agent.remainingDistance < 0.2f)
            {
                isIdle = true;
                _agent.isStopped = true;
                SetNewIdleDuration();
            }
        }
    }

    void SetNewIdleDuration()
    {
        idleTime = Time.time + Random.Range(minIdleDuration, maxIdleDuration);
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        _agent.destination = patrolPoints[currentPatrolIndex].position;
        _agent.isStopped = false;

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void BecomeAngry()
    {
        isAngry = true;
        isIdle = false;

        Debug.Log("😡 NPC ist jetzt ANGRY!");

        // Spawn Enemies innerhalb des Colliders
        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject enemyPrefab = angryEnemyPrefabs[Random.Range(0, angryEnemyPrefabs.Length)];
            Vector3 spawnPos = GetRandomPointInBounds(spawnAreaCollider.bounds);
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }

    Vector3 GetRandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            bounds.min.y,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}