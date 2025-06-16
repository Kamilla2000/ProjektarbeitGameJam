using UnityEngine;
using UnityEngine.AI;

public class VorlesungNavMeshAgentController : MonoBehaviour
{

    // zielpunkt
    public Transform TargetIndicator;

    // layer, die für Raycast interessant sind
    public LayerMask LayerMaskCustom;

    private NavMeshAgent _agent;
    //
    private Camera _rayCamera;

    private Animator _animator;
    private int _movementParameterHash;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        // Get Camera
        _rayCamera = Camera.main;

        _animator = GetComponent<Animator>();
        _movementParameterHash = Animator.StringToHash("speed");
    }

    // Update is called once per frame
    void Update()
    {
        //Strahl erzeugt und schicke es los
        Ray ray = _rayCamera.ScreenPointToRay(Input.mousePosition);

        // Maustaste gedrückt?
        if (Input.GetMouseButtonDown(0))
        {
            // falls ja, dann auf die position verschieben
            if (Physics.Raycast(ray, out RaycastHit rayCalsHit, float.MaxValue, LayerMaskCustom))
            {
                if (rayCalsHit.collider.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")))
                {
                    TargetIndicator.position = rayCalsHit.point;
                    _agent.SetDestination(TargetIndicator.position);
                }
            }
        }

        // Animation mit der Geschwindigkeit anpassen/ syncronisieren
        _animator.SetFloat(_movementParameterHash, _agent.velocity.magnitude);
    }
}
