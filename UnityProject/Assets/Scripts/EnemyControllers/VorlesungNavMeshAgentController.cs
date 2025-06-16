using UnityEngine;
using UnityEngine.AI;

public class VorlesungNavMeshAgentController : MonoBehaviour
{
    public float detectionRadius = 10f; // Erkennungsradius

    private NavMeshAgent _agent;
    private Animator _animator;
    private int _movementParameterHash;

    private Transform _playerTransform;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _movementParameterHash = Animator.StringToHash("speed");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player mit Tag 'Player' nicht gefunden.");
        }
    }

    void Update()
    {
        if (_playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            _agent.SetDestination(_playerTransform.position);
        }
        else
        {
            _agent.ResetPath(); // stoppt die Bewegung
        }

        _animator.SetFloat(_movementParameterHash, _agent.velocity.magnitude);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

}