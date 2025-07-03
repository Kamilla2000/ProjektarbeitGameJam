using UnityEngine;
using UnityEngine.AI;

public class NPCStateMachine : BaseStateMachine
{
    public Vector3 PlayerPosition => _player.position;

    [Header("Sicht- und Hörsinn")]
    public bool CanSeePlayer => _eyes.IsDetecting;
    public bool CanHearPlayer => _ears.IsDetecting;

    [Header("Distanz Einstellungen")]
    public float chaseDistance = 10f;
    public float attackDistance = 2f;

    [Header("NPC States")]
    public NPCIdleState IdleState;
    public NPCPatrolState PatrolState;
    public NPCAttackState AttackState;
    public NPCChaseState ChaseState;  // NEU: ChaseState

    private NavMeshAgent _agent;
    private Animator _animator;
    private Eyes _eyes;
    private Ears _ears;
    private Transform _player;

    private float _initalAgentSpeed;
    private int _speedParameterHash;

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        WaypointGizmos.DrawWayPoints(PatrolState.Waypoints);

        if (_player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDistance);
        }
    }
#endif

    public override void Initialize()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _eyes = GetComponentInChildren<Eyes>();
        _ears = GetComponentInChildren<Ears>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;

        _initalAgentSpeed = _agent.speed;
        _speedParameterHash = Animator.StringToHash("speed");

        CurrentState = IdleState;
        CurrentState.OnEnterState(this);
    }

    public override void Tick()
    {
        _animator.SetFloat(_speedParameterHash, _agent.velocity.magnitude);

        float distanceToPlayer = Vector3.Distance(transform.position, PlayerPosition);

        if (distanceToPlayer <= attackDistance)
        {
            if (!(CurrentState is NPCAttackState))
                SwitchToState(AttackState);
        }
       /*else if (distanceToPlayer <= chaseDistance)
        {
            if (!(CurrentState is NPCChaseState))
                SwitchToState(ChaseState);
        }*/
        else
        {
            // Nur zurück zur Patrouille, wenn man aktuell jagt oder angreift
            if (CurrentState is NPCChaseState || CurrentState is NPCAttackState)
                SwitchToState(PatrolState);
        }
    }

    public void SetDestionation(Vector3 position) => _agent.SetDestination(position);

    public void SetSpeedMultiplier(float multiplier) => _agent.speed = multiplier * _initalAgentSpeed;

    public void AttackPlayer()
    {
        _animator.SetTrigger("Attack");
        Debug.Log("NPC greift den Spieler an!");
        // Schaden, Partikeleffekte etc.
    }
}