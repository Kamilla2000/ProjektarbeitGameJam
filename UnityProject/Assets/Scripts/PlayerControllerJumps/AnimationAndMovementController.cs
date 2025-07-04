using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class AnimationAndMovementController : MonoBehaviour
{
    // Declare reference variables
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    // Variables to store optimized setter/getter IDs for Animator parameters
    int isWalkingHash;
    int isRunningHash;

    // Variables to store player input values
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    Vector3 appliedMovement;
    bool isMovementPressed;
    bool isRunPressed;

    // Constants
    float rotationFactorPerFrame = 15f;
    int zero = 0;

    // Gravity variables
    float gravity = -9.8f;
    float groundedGravity = -0.05f;

    [SerializeField] float walkSpeed = 2.0f;       // Editable in Inspector
    [SerializeField] float runMultiplier = 5.0f;   // Editable in Inspector

    // Jumping variables
    bool isJumpPressed = false;
    float initialJumpVelocity;
    float maxJumpHeight = 4.0f;
    float maxJumpTime = 0.75f;
    bool isJumping = false;
    int isJumpingHash;
    int jumpCountHash;
    bool isJumpAnimating = false;
    int jumpCount = 0;
    Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> jumpGravities = new Dictionary<int, float>();
    Coroutine currentJumpResetRoutine = null;

    // Task tracking
    private TaskManager taskManager;
    private HashSet<KeyCode> keysPressed = new HashSet<KeyCode>();
    private bool dialogueFinished = false;

    private AudioManager audioManager;

    void Awake()
    {
        //Audio Manager suchen
        audioManager = FindFirstObjectByType<AudioManager>();

        // Initialize PlayerInput and get required components
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        taskManager = FindAnyObjectByType<TaskManager>();

        // Cache Animator parameter hashes for performance
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        jumpCountHash = Animator.StringToHash("jumpCount");

        // Subscribe to PlayerInput events
        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;

        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;

        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;

        setUpJumpVariables();
    }

    // Call this when dialogue is finished to allow player control
    public void SetDialogueFinished()
    {
        dialogueFinished = true;
    }

    // Setup initial jump velocities and gravity values for multi-jump system
    void setUpJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;

        float secondJumpGravity = (-2 * (maxJumpHeight + 2)) / Mathf.Pow(timeToApex * 1.25f, 2);
        float secondJumpInitialVelocity = (2 * (maxJumpHeight + 2)) / (timeToApex * 1.25f);

        float thirdJumpGravity = (-2 * (maxJumpHeight + 4)) / Mathf.Pow(timeToApex * 1.5f, 2);
        float thirdJumpInitialVelocity = (2 * (maxJumpHeight + 4)) / (timeToApex * 1.5f);

        initialJumpVelocities.Add(1, initialJumpVelocity);
        initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        jumpGravities.Add(0, gravity);
        jumpGravities.Add(1, gravity);
        jumpGravities.Add(2, secondJumpGravity);
        jumpGravities.Add(3, thirdJumpGravity);
    }

    // Handle jump logic including multi-jump
    void handleJump()
    {
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            if (jumpCount > 3 && currentJumpResetRoutine != null)
            {
                StopCoroutine(currentJumpResetRoutine);
            }

            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            jumpCount += 1;

            animator.SetInteger(jumpCountHash, jumpCount);
            currentMovement.y = initialJumpVelocities[jumpCount] * 0.5f;
            appliedMovement.y = initialJumpVelocities[jumpCount] * 0.5f;

            // === Track jump task for taskManager if dialogue finished ===
            if (dialogueFinished)
                taskManager?.RegisterJump();

            //Audio
            audioManager?.Play("Jump");
        }
        else if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    // Coroutine to reset jump count after delay
    IEnumerator jumpResetRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        jumpCount = 0;
    }

    // InputSystem callback for jump input
    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    // InputSystem callback for run input
    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    // InputSystem callback for movement input
    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;

        // Get camera directions and flatten Y axis
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        // Calculate move direction relative to camera
        Vector3 moveDirection = camForward * currentMovementInput.y + camRight * currentMovementInput.x;

        // Set movement vectors scaled by speed multipliers
        currentMovement = moveDirection * walkSpeed;
        currentRunMovement = moveDirection * runMultiplier;

        // Prevent movement tracking during dialogue
        if (!dialogueFinished) return;

        // Track pressed keys for task tracking
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) keysPressed.Add(KeyCode.W);
            if (Keyboard.current.aKey.isPressed) keysPressed.Add(KeyCode.A);
            if (Keyboard.current.sKey.isPressed) keysPressed.Add(KeyCode.S);
            if (Keyboard.current.dKey.isPressed) keysPressed.Add(KeyCode.D);

            // === Track move task ===
            if (keysPressed.Contains(KeyCode.W) && keysPressed.Contains(KeyCode.A)
                && keysPressed.Contains(KeyCode.S) && keysPressed.Contains(KeyCode.D))
            {
                taskManager?.RegisterMove();
            }

            // === Track run task ===
            if (isRunPressed && isMovementPressed)
            {
                taskManager?.RegisterRun();
            }
        }
    }

    // Handle character rotation smoothly towards movement direction
    void handleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    // Handle setting animator parameters based on movement states
    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);

            //Audio
            audioManager?.Play("Walk");
        }
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);

            //Audio
            audioManager?.Play("Run");
        }
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }

    // Handle gravity application for jump and fall mechanics
    void handleGravity()
    {
        bool isFalling = currentMovement.y < 0.0f || !isJumpPressed;
        float fallMultiplier = 2.0f;

        if (characterController.isGrounded)
        {
            if (isJumpAnimating)
            {
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
                currentJumpResetRoutine = StartCoroutine(jumpResetRoutine());
                if (jumpCount == 3)
                {
                    jumpCount = 0;
                    animator.SetInteger(jumpCountHash, jumpCount);
                }
            }

            currentMovement.y = groundedGravity;
            appliedMovement.y = groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (jumpGravities[jumpCount] * fallMultiplier * Time.deltaTime);
            appliedMovement.y = Mathf.Max((previousYVelocity + currentMovement.y) * 0.5f, -20.0f);
        }
        else
        {
            float previousYVelocity = currentMovement.y;
            currentMovement.y = currentMovement.y + (jumpGravities[jumpCount] * Time.deltaTime);
            appliedMovement.y = (previousYVelocity + currentMovement.y) * 0.5f;
        }
    }

    // Main Update loop
    void Update()
    {
        handleRotation();
        handleAnimation();

        // Apply running or walking speed to movement
        if (isRunPressed)
        {
            appliedMovement.x = currentRunMovement.x;
            appliedMovement.z = currentRunMovement.z;
        }
        else
        {
            appliedMovement.x = currentMovement.x;
            appliedMovement.z = currentMovement.z;
        }

        // Move character controller
        characterController.Move(appliedMovement * Time.deltaTime);

        // Handle gravity and jumping
        handleGravity();
        handleJump();
    }

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
