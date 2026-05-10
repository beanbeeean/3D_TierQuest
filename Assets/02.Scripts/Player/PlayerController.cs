using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private PlayerStatus _status;

    [SerializeField] private string moveActionName = "Move";
    
    public Vector2 MoveVector { get; private set; }
    [SerializeField] private float _walkSpeed = 3f;
    [SerializeField] private float _runSpeed = 10f;
    [SerializeField] private float _runStaminaValue = 2f;
    private bool _isRunPressed;
    private bool _isExhausted;

    private float _appliedSpeed;

    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _jumpStaminaValue = 10f;
    [SerializeField] private bool _grounded = true;
    

    private int _floorLayer = 1 << 8;

    public float AppliedSpeed => _appliedSpeed; 
    public bool IsGrounded => _grounded;

    private Transform _cameraTransform;
    public Action attackAction;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = FindAction(moveActionName);
        _appliedSpeed = _walkSpeed;
        _cameraTransform = Camera.main.transform;
        _status = GetComponent<PlayerStatus>();
    }

    private void Update()
    {
        MoveVector = _moveAction != null ? _moveAction.ReadValue<Vector2>() : Vector2.zero;
        CheckGround();
    }

    void FixedUpdate()
    {
        HandleRunStamina();
        HandleMove();
    }

    void HandleMove()
    {
        Vector3 camForward = _cameraTransform.forward;
        Vector3 camRight = _cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        Vector3 moveDir = (camForward.normalized * MoveVector.y) + (camRight.normalized * MoveVector.x);

        _rb.linearVelocity = new Vector3(
            moveDir.normalized.x * _appliedSpeed,
            _rb.linearVelocity.y,
            moveDir.normalized.z * _appliedSpeed
        );

        if (moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir.normalized), 0.2f);
        }
    }
    

    void OnJump()
    {
        if (_grounded && _status.CurrentSt >= _jumpStaminaValue)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _grounded = false;
            _status.ConsumeStamina(_jumpStaminaValue);
        }
    }

    void OnRun(InputValue value)
    {
        _isRunPressed = value.isPressed;

        if (!_isRunPressed)
        {
            _isExhausted = false;
        }
    }

    void OnAttack()
    {
        attackAction?.Invoke();
    }

    void HandleRunStamina()
    {
        if (_status.CurrentSt <= 0f)
        {
            _isExhausted = true;
        }
        
        bool canRun = _isRunPressed && !_isExhausted && MoveVector != Vector2.zero && _status.CurrentSt > 0f;

        if (canRun)
        {
            _appliedSpeed = _runSpeed;
            _status.SetConsumingStamina(true);
            _status.ConsumeStamina(_runStaminaValue * Time.fixedDeltaTime);
        }
        else
        {
            _appliedSpeed = _walkSpeed;
            _status.SetConsumingStamina(false);
        }

        
    }
    
    void CheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2f))
        {
            if (((1 << hit.collider.gameObject.layer) & _floorLayer) != 0)
            {
                _grounded = true;
                return;
            }
        }
        _grounded = false;
    }

    private InputAction FindAction(string actionName)
    {
        if (string.IsNullOrWhiteSpace(actionName)) return null;
        return _playerInput.actions.FindAction(actionName, false);
    }
    

}
