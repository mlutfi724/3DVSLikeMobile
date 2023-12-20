using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// This class is context for the state machine
public class PlayerStateMachine : MonoBehaviour
{
    // declare reference variables

    private CharacterController _characterController;
    private Animator _animator;
    private PlayerInput _playerInput; // note player input class must be generated from new input system in inspector

    // variables to store optimized setter/getter paramater IDs

    private int _isWalkingHash;
    private int _isRunningHash;
    private int _isJumpingHash;
    private int _jumpCountHash;
    private int _isFallingHash;

    // variable to store player input values

    private Vector2 _lastMovementInput;
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private Vector3 _currentRunMovement;
    private Vector3 _appliedMovement;
    private Vector3 _cameraRelativeMovement;

    private bool _isMovementPressed;
    private bool _isRunPressed;

    // Constants

    private float _rotationFactorPerFrame = 15.0f;
    [SerializeField] private float _runMultiplier = 3f;
    [SerializeField] private float _moveSpeed = 3f;
    private int _zero = 0;

    // gravity variables
    [SerializeField] private float _gravity = -9.8f;

    // Jumping variables
    private bool _isJumpPressed = false;

    private float _initialJumpVelocity;
    private float _maxJumpHeight = 2.0f;
    private float _maxJumpTime = 0.75f;
    private bool _isJumping = false;
    private bool _requireNewJumpPress = false;
    private int _jumpCount = 0;
    private Dictionary<int, float> _initialJumpVelocities = new Dictionary<int, float>();
    private Dictionary<int, float> _jumpGravities = new Dictionary<int, float>();
    private Coroutine _currentJumpResetRoutine = null;

    // state variables

    private PlayerBaseState _currentState;
    private PlayerStateFactory _states;

    // getters and setters
    public PlayerBaseState CurrentState
    { get { return _currentState; } set { _currentState = value; } }

    public Animator Animator
    { get { return _animator; } }

    public CharacterController CharacterController
    { get { return _characterController; } }

    public Coroutine CurrentJumpResetRoutine
    { get { return _currentJumpResetRoutine; } set { _currentJumpResetRoutine = value; } }

    public Dictionary<int, float> InitialJumpVelocities
    { get { return _initialJumpVelocities; } }

    public Dictionary<int, float> JumpGravities
    { get { return _jumpGravities; } }

    public int JumpCount
    { get { return _jumpCount; } set { _jumpCount = value; } }

    public int IsWalkingHash
    { get { return _isWalkingHash; } }

    public int IsRunningHash
    { get { return _isRunningHash; } }

    public int IsFallingHash
    { get { return _isFallingHash; } }

    public int IsJumpingHash
    { get { return _isJumpingHash; } }

    public int JumpCountHash
    { get { return _jumpCountHash; } }

    public bool IsMovementPressed
    { get { return _isMovementPressed; } }

    public bool IsRunPressed
    { get { return _isRunPressed; } }

    public bool RequireNewJumpPress
    { get { return _requireNewJumpPress; } set { _requireNewJumpPress = value; } }

    public bool IsJumping
    { set { _isJumping = value; } }

    public bool IsJumpPressed
    { get { return _isJumpPressed; } }

    public float Gravity
    { get { return _gravity; } }

    public float CurrentMovementY
    { get { return _currentMovement.y; } set { _currentMovement.y = value; } }

    public float AppliedMovementY
    { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }

    public float AppliedMovementX
    { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }

    public float AppliedMovementZ
    { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }

    public float RunMultiplier
    { get { return _runMultiplier; } }

    public float MoveSpeed
    { get { return _moveSpeed; } }

    public Vector2 LastMovementInput
    { get { return _lastMovementInput; } }

    public Vector2 CurrentMovementInput
    { get { return _currentMovementInput; } }

    // Awake is called earlier than Start in Unity's event life cycle
    private void Awake()
    {
        // initially set reference variables
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        // setup state
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

        // set the parameter hash references
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isRunningHash = Animator.StringToHash("isRunning");
        _isFallingHash = Animator.StringToHash("isFalling");
        _isJumpingHash = Animator.StringToHash("isJumping");
        _jumpCountHash = Animator.StringToHash("jumpCount");

        // set the player input callbacks
        _playerInput.CharacterControls.Walk.started += OnMovementInput;
        _playerInput.CharacterControls.Walk.canceled += OnMovementInput;
        _playerInput.CharacterControls.Walk.performed += OnMovementInput;

        _playerInput.CharacterControls.Run.started += OnRun;
        _playerInput.CharacterControls.Run.canceled += OnRun;

        _playerInput.CharacterControls.Jump.started += OnJump;
        _playerInput.CharacterControls.Jump.canceled += OnJump;

        SetupJumpVariables();
    }

    private void SetupJumpVariables()
    {
        float timeToApex = _maxJumpTime / 2;
        float initialGravity = (-2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
        float secondJumpGravity = (-2 * (_maxJumpHeight + 2)) / Mathf.Pow((timeToApex * 1.25f), 2);
        float secondJumpInitialVelocity = (2 * (_maxJumpHeight + 2)) / (timeToApex * 1.25f);
        float thirdJumpGravity = (-2 * (_maxJumpHeight + 4)) / Mathf.Pow((timeToApex * 1.5f), 2);
        float thirdJumpInitialVelocity = (2 * (_maxJumpHeight + 4)) / (timeToApex * 1.5f);

        _initialJumpVelocities.Add(1, _initialJumpVelocity);
        _initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        _initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        _jumpGravities.Add(0, initialGravity);
        _jumpGravities.Add(1, initialGravity);
        _jumpGravities.Add(2, secondJumpGravity);
        _jumpGravities.Add(3, thirdJumpGravity);
    }

    private void OnEnable()
    {
        // enable the character controls action map
        _playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        // disable the character controls action map
        _playerInput.CharacterControls.Disable();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _characterController.Move(_appliedMovement * Time.deltaTime);
        _lastMovementInput = new Vector2(0f, 1f); // shoot the projectile upward at the start of the game
    }

    // Update is called once per frame
    private void Update()
    {
        HandleRotation();
        CalculateLastMovementVector();
        _currentState.UpdateStates();

        _cameraRelativeMovement = ConvertToCameraSpace(_appliedMovement);
        _characterController.Move(_cameraRelativeMovement * Time.deltaTime);
    }

    private Vector3 ConvertToCameraSpace(Vector3 vectorToRotate)
    {
        // store the Y value of the original vector to rotate
        float currentYValue = vectorToRotate.y;

        // get the forward and right directional vectors of the camera
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        // remove the Y values to ignore upward/downward camera angles
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        // re normalize both vectors so they each have a magnitude of 1
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        // rotate the x and z vectorToRotate values to camera space
        Vector3 cameraForwardZProduct = vectorToRotate.z * cameraForward;
        Vector3 cameraRightXProduct = vectorToRotate.x * cameraRight;

        // the sum of both products is the vector3 in camera space
        Vector3 vectorRotatedToCameraSpace = cameraForwardZProduct + cameraRightXProduct;
        vectorRotatedToCameraSpace.y = currentYValue;
        return vectorRotatedToCameraSpace;
    }

    private void HandleRotation()
    {
        Vector3 positionToLookAt;
        // the change position our character should point to
        positionToLookAt.x = _cameraRelativeMovement.x;
        positionToLookAt.y = _zero;
        positionToLookAt.z = _cameraRelativeMovement.z;
        // the current rotation of our character
        Quaternion currentRotation = transform.rotation;

        if (_isMovementPressed)
        {
            // create a new rotation based on where the player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
        }
    }

    // callback handler function to set the player input values
    private void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
        _currentRunMovement.x = _currentMovementInput.x * _runMultiplier;
        _currentRunMovement.z = _currentMovementInput.y * _runMultiplier;
        _isMovementPressed = _currentMovementInput.x != _zero || _currentMovementInput.y != _zero;
    }

    private Vector2 CalculateLastMovementVector()
    {
        if (_isMovementPressed)
        {
            _lastMovementInput = _currentMovementInput;
        }
        return _lastMovementInput;
    }

    // callback handler function for jump buttons
    private void OnRun(InputAction.CallbackContext context)
    {
        _isRunPressed = context.ReadValueAsButton();
    }

    // callback handler function for run buttons
    private void OnJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
        _requireNewJumpPress = false;
    }
}