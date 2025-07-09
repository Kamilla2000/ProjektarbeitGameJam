using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator animator;

    private int isWalkingHash;
    private int isRunningHash;
    private int isJumpingHash;

    private Vector2 movementInput;
    private Vector3 currentMovement;
    private Vector3 appliedMovement;
    private bool isMovementPressed;
    private bool isRunPressed;
    private bool isJumpPressed;
    private bool isJumping = false;

    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runMultiplier = 1f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float jumpTimeout = 0.2f;

    private float gravity = -9.81f;
    private float groundedGravity = -0.05f;
    private float initialJumpVelocity;
    private float jumpCooldownTimer = 0f;

    private void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;

        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;

        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;

        initialJumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
    }

    private void onMovementInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        isMovementPressed = movementInput.x != 0 || movementInput.y != 0;
    }

    private void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    private void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    private void handleGravity()
    {
        if (characterController.isGrounded)
        {
            if (isJumping)
            {
                // Landung
                isJumping = false;
            }

            jumpCooldownTimer = 0f;
            currentMovement.y = groundedGravity;
            appliedMovement.y = groundedGravity;

            if (isJumpPressed && jumpCooldownTimer <= 0f)
            {
                isJumping = true;
                currentMovement.y = initialJumpVelocity;
                appliedMovement.y = initialJumpVelocity;
                jumpCooldownTimer = jumpTimeout;
            }
        }
        else
        {
            jumpCooldownTimer -= Time.deltaTime;

            // Schwerkraft wirkt in der Luft
            currentMovement.y += gravity * Time.deltaTime;
            appliedMovement.y = currentMovement.y;
        }
    }

    private void handleRotation()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDir = cameraForward * movementInput.y + cameraRight * movementInput.x;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }
    }

    private void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        // Walking
        if (isMovementPressed && !isWalking)
            animator.SetBool(isWalkingHash, true);
        else if (!isMovementPressed && isWalking)
            animator.SetBool(isWalkingHash, false);

        // Running
        if (isMovementPressed && isRunPressed && !isRunning)
            animator.SetBool(isRunningHash, true);
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
            animator.SetBool(isRunningHash, false);

        // Jumping Animation nur wenn wir wirklich springen
        animator.SetBool(isJumpingHash, isJumping);
    }

    private void Update()
    {
        handleGravity();
        handleRotation();
        handleAnimation();

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * movementInput.y + cameraRight * movementInput.x;
        currentMovement.x = moveDirection.x * (isRunPressed ? walkSpeed * runMultiplier : walkSpeed);
        currentMovement.z = moveDirection.z * (isRunPressed ? walkSpeed * runMultiplier : walkSpeed);

        appliedMovement.x = currentMovement.x;
        appliedMovement.z = currentMovement.z;

        characterController.Move(appliedMovement * Time.deltaTime);
    }

    private void OnEnable() => playerInput.CharacterControls.Enable();
    private void OnDisable() => playerInput.CharacterControls.Disable();

    public bool IsAudible => isMovementPressed || isRunPressed || isJumpPressed;
}