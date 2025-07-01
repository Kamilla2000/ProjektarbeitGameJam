using UnityEngine;

/// <summary>
/// Base for a third person character controller with jumping
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
    public float RotationSpeed = 720f;
    public float LocomotionParameterDamping = 0.1f;

    public float JumpForce = 5f;
    public float Gravity = -9.81f;

    private float _verticalVelocity;
    private bool _isGrounded;

    private CharacterController _characterController;
    private Transform _cameraTransform;
    private Animator _animator;

    private int _speedParameterHash;
    private int _isWalkingParameterHash;
    private int _jumpTriggerHash;

    public bool IsAudible { get; private set; }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();

        _cameraTransform = Camera.main.transform;

        _speedParameterHash = Animator.StringToHash("speed");
        _isWalkingParameterHash = Animator.StringToHash("isMoving");
        _jumpTriggerHash = Animator.StringToHash("JumpTrigger");
    }

    void Update()
    {
        // Read input
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        // Adjust movement based on camera orientation
        movementDirection = Quaternion.AngleAxis(_cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;

        bool shouldWalk = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float speed = shouldWalk ? inputMagnitude * 0.333f : inputMagnitude;

        _animator.SetBool(_isWalkingParameterHash, inputMagnitude > 0);
        _animator.SetFloat(_speedParameterHash, speed, LocomotionParameterDamping, Time.deltaTime);

        // Ground check
        _isGrounded = _characterController.isGrounded;
        if (_isGrounded && _verticalVelocity < 0)
            _verticalVelocity = -2f;

        // Jumping
        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            _verticalVelocity = JumpForce;
            _animator.SetTrigger(_jumpTriggerHash);
        }

        // Apply gravity
        _verticalVelocity += Gravity * Time.deltaTime;

        // Movement
        Vector3 move = movementDirection.normalized * speed;
        move.y = _verticalVelocity;

        _characterController.Move(move * Time.deltaTime);

        // Rotation
        if (movementDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);
        }

        // Character is audible, when moving fast
        IsAudible = speed >= 0.5f;
    }
}
