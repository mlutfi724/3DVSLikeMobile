using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    // declare reference variables

    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator animator;

    // variables to store optimized setter/getter paramater IDs

    private int isWalkingHash;
    private int isRunningHash;
    private int isJumpingHash;
    private int jumpCountHash;

    // variable to store player input values

    private Vector2 currentMovementInput;
    private Vector3 currentWalkMovement;
    private Vector3 currentRunMovement;
    private Vector3 appliedMovement;

    private bool isMovementPressed;
    private bool isRunPressed;

    // Constants
    private float rotationFactorPerFrame = 15.0f;

    private float runSpeed = 3f;
    private float gravity = -9.8f;
    private float groundedGravity = -0.05f;
    private int zero = 0;

    // Jumping variables
    private bool isJumpPressed = false;

    private float initialJumpVelocity;
    private float maxJumpHeight = 2.0f;
    private float maxJumpTime = 0.75f;
    private bool isJumping = false;
    private bool isJumpAnimating = false;
    private int jumpCount = 0;
    private Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
    private Dictionary<int, float> jumpGravities = new Dictionary<int, float>();
    private Coroutine currentJumpResetRoutine = null;

    // Awake is called earlier than Start in Unity's event life cycle
    private void Awake()
    {
        // initially set reference variables
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        jumpCountHash = Animator.StringToHash("jumpCount");

        // set the player input callbacks
        playerInput.CharacterControls.Walk.started += OnMovementInput;
        playerInput.CharacterControls.Walk.canceled += OnMovementInput;
        playerInput.CharacterControls.Walk.performed += OnMovementInput;

        playerInput.CharacterControls.Run.started += OnRun;
        playerInput.CharacterControls.Run.canceled += OnRun;

        playerInput.CharacterControls.Jump.started += OnJump;
        playerInput.CharacterControls.Jump.canceled += OnJump;

        SetupJumpVariables();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleRotation();
        HandleAnimation();

        if (isRunPressed)
        {
            appliedMovement.x = currentRunMovement.x;
            appliedMovement.z = currentRunMovement.z;
        }
        else
        {
            appliedMovement.x = currentWalkMovement.x;
            appliedMovement.z = currentWalkMovement.z;
        }

        characterController.Move(appliedMovement * Time.deltaTime);

        HandleGravity();
        HandleJump();
    }

    private void OnEnable()
    {
        // enable the character controls action map
        playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        // disable the character controls action map
        playerInput.CharacterControls.Disable();
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentWalkMovement.x = currentMovementInput.x;
        currentWalkMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x * runSpeed;
        currentRunMovement.z = currentMovementInput.y * runSpeed;
        isMovementPressed = currentMovementInput.x != zero || currentMovementInput.y != zero;
    }

    private void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    private void SetupJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        float secondJumpGravity = (-2 * (maxJumpHeight + 2)) / Mathf.Pow((timeToApex * 1.25f), 2);
        float secondJumpInitialVelocity = (2 * (maxJumpHeight + 2)) / (timeToApex * 1.25f);
        float thirdJumpGravity = (-2 * (maxJumpHeight + 4)) / Mathf.Pow((timeToApex * 1.5f), 2);
        float thirdJumpInitialVelocity = (2 * (maxJumpHeight + 4)) / (timeToApex * 1.5f);

        initialJumpVelocities.Add(1, initialJumpVelocity);
        initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        jumpGravities.Add(0, gravity);
        jumpGravities.Add(1, gravity);
        jumpGravities.Add(2, secondJumpGravity);
        jumpGravities.Add(3, thirdJumpGravity);
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;
        // the change position our character should point to
        positionToLookAt.x = currentWalkMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentWalkMovement.z;
        // the current rotation of our character
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            // create a new rotation based on where the player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    private void HandleJump()
    {
        if (!isJumping && characterController.isGrounded && isJumpPressed)
        {
            if (jumpCount < 3 && currentJumpResetRoutine != null)
            {
                StopCoroutine(currentJumpResetRoutine);
            }
            animator.SetBool(isJumpingHash, true);
            isJumpAnimating = true;
            isJumping = true;
            jumpCount += 1;
            animator.SetInteger(jumpCountHash, jumpCount);
            currentWalkMovement.y = initialJumpVelocities[jumpCount];
            appliedMovement.y = initialJumpVelocities[jumpCount];
        }
        else if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    private IEnumerator JumpResetRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        jumpCount = 0;
    }

    private void HandleGravity()
    {
        bool isFalling = currentWalkMovement.y <= 0.0f || !isJumpPressed;
        float fallGravityMultiplier = 2.0f;
        // apply proper gravity depending on if the character is grounded or not
        if (characterController.isGrounded)
        {
            if (isJumpAnimating)
            {
                animator.SetBool(isJumpingHash, false);
                isJumpAnimating = false;
                currentJumpResetRoutine = StartCoroutine(JumpResetRoutine());
                if (jumpCount == 3)
                {
                    jumpCount = 0;
                    animator.SetInteger(jumpCountHash, jumpCount);
                }
            }
            currentWalkMovement.y = groundedGravity;
            appliedMovement.y = groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = currentWalkMovement.y;
            currentWalkMovement.y = currentWalkMovement.y + (jumpGravities[jumpCount] * fallGravityMultiplier * Time.deltaTime);
            appliedMovement.y = Mathf.Max((previousYVelocity + currentWalkMovement.y) * 0.5f, -20.0f);
        }
        else
        {
            float previousYVelocity = currentWalkMovement.y;
            currentWalkMovement.y = currentWalkMovement.y + (jumpGravities[jumpCount] * Time.deltaTime);
            appliedMovement.y = (previousYVelocity + currentWalkMovement.y) * 0.5f;
            //currentWalkMovement.y += gravity * Time.deltaTime;
            //currentRunMovement.y += gravity * Time.deltaTime;
        }
    }

    private void HandleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        // Start walking if movement pressed is true and not already walking
        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        // stop walking if isMovementPressed is false and not already walking
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }
        // run if movement and run pressed are true and not currently running
        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        // stop running if movement or run pressed are false and currently running
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }
}