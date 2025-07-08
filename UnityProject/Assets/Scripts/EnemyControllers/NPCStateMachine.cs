using UnityEngine;
using UnityEngine.AI;

public class NPCStateMachine : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;

    private NavMeshAgent _agent;
    private Animator _animator;

    [Header("Idle Settings")]
    private bool isIdle = true;
    private float idleTime = 0f;
    public float minIdleDuration = 2f;
    public float maxIdleDuration = 5f;

    [Header("Angry State Settings")]
    public float timeUntilAngry = 10f;
    public float angryDuration = 5f;
    private float angryTimer = 0f;
    private float angryEndTime = 0f;
    private bool isAngry = false;

    [Header("Enemy Spawn Settings")]
    public GameObject[] angryEnemyPrefabs;
    public int spawnAmount = 3;
    public Collider spawnAreaCollider;
    private float nextSpawnTime = 0f;
    public float spawnCooldown = 1.5f;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        SetNewIdleDuration();
        GoToNextPatrolPoint();
    }

    void Update()
    {
        if (isAngry)
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("Angry"))
            {
                if (Time.time >= nextSpawnTime)
                {
                    SpawnAngryEnemies();
                    nextSpawnTime = Time.time + spawnCooldown;
                }

                if (Time.time >= angryEndTime)
                {
                    isAngry = false;
                    angryTimer = 0f;
                    _animator.SetBool("isAngry", false);
                    _animator.SetBool("isIdle", true);
                    _agent.isStopped = false;
                    GoToNextPatrolPoint();
                }
            }
        }
        else
        {
            angryTimer += Time.deltaTime;

            if (angryTimer >= timeUntilAngry)
            {
                isAngry = true;
                _animator.SetBool("isAngry", true);
                _animator.SetBool("isIdle", false);
                _agent.isStopped = true;

                angryEndTime = Time.time + angryDuration;
                nextSpawnTime = Time.time;
            }
            else
            {
                PatrolLogic();
                UpdateAnimationStates();
            }
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
        Debug.Log("😡 Spawning angry enemies...");

        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject prefab = angryEnemyPrefabs[Random.Range(0, angryEnemyPrefabs.Length)];
            Vector3 spawnPos = GetRandomPointInBounds(spawnAreaCollider.bounds);
            Instantiate(prefab, spawnPos, Quaternion.identity);
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