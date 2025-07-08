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
    private bool hasSpawnedEnemies = false;

    [Header("Angry Enemy Spawn Settings")]
    public GameObject[] angryEnemyPrefabs;
    public int spawnAmount = 3;
    public Collider spawnAreaCollider;

    private bool isIdle = true;
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
                isAngry = true;
                isIdle = false; // nicht mehr idle!
                _animator.SetBool("isAngry", true);
                _animator.SetBool("isIdle", false);
            }
            else
            {
                PatrolLogic();
                UpdateAnimationStates();
            }
        }
        else
        {
            // Warten bis Animator wirklich im "Angry" State ist
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Angry") && !hasSpawnedEnemies)
            {
                SpawnAngryEnemies();
                hasSpawnedEnemies = true;
            }

            // Kein weiteres Patrouillieren im Angry-Modus
            _agent.isStopped = true;
        }
    }

    void UpdateAnimationStates()
    {
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

        _agent.isStopped = false;
        _agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    void SpawnAngryEnemies()
    {
        Debug.Log("😡 NPC ist jetzt ANGRY & spawnt Gegner!");

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