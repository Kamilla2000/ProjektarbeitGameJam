using UnityEngine;
using UnityEngine.AI;

public class NPCStateMachine : BaseStateMachine
{
    // Position des Spielers
    public Vector3 PlayerPosition { get => _player.position; }
    public bool CanSeePlayer { get => _eyes.IsDetecting; }
    public bool CanHearPlayer { get => _ears.IsDetecting; }

    // States des NPC
    public NPCIdleState IdleState;
    public NPCAttackState AttackState;     // Neu: AttackState
    public NPCPatrolState PatrolState;

    // NavMeshAgent & Animator
    private NavMeshAgent _agent;
    private Animator _animator;

    // Sinne des NPC
    private Eyes _eyes;
    private Ears _ears;

    // Spieler-Transform
    private Transform _player;

    // Ursprüngliche Geschwindigkeit des NavMeshAgent
    private float _initalAgentSpeed;

    // Hash für Animator Parameter
    private int _speedParameterHash;

#if UNITY_EDITOR
    void OnDrawGizmosSelected() => WaypointGizmos.DrawWayPoints(PatrolState.Waypoints);
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

        // Beispiel: Automatischer Wechsel zu AttackState, wenn NPC den Spieler sehen kann
        if (CanSeePlayer && !(CurrentState is NPCAttackState))
        {
            SwitchToState(AttackState);
        }
    }

    public void SetDestionation(Vector3 position) => _agent.SetDestination(position);

    public void SetSpeedMultiplier(float multiplier) => _agent.speed = multiplier * _initalAgentSpeed;

    public void AttackPlayer()
    {
        _animator.SetTrigger("Attack");
        Debug.Log("NPC greift den Spieler an!");
        // Hier kannst du weitere Logik für Schaden, Effekte etc. hinzufügen
    }
}