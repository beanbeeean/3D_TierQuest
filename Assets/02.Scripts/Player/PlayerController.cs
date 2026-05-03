using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rb;
    private PlayerInput _playerInput;
    private InputAction _moveAction;

    [SerializeField] private string moveActionName = "Move";
    
    public Vector2 MoveVector { get; private set; }
    [SerializeField] private float _walkSpeed = 3f;
    [SerializeField] private float _runSpeed = 10f;
    private float _appliedSpeed;

    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private bool _grounded = true;

    private int _floorLayer = 1 << 8;

    public float AppliedSpeed => _appliedSpeed; 
    public bool IsGrounded => _grounded;

    private Transform _cameraTransform;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = FindAction(moveActionName);
        _appliedSpeed = _walkSpeed;
        _cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        MoveVector = _moveAction != null ? _moveAction.ReadValue<Vector2>() : Vector2.zero;
        CheckGround();
    }

    void FixedUpdate()
    {
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
        if (_grounded)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _grounded = false;
        }
    }

    void OnRun(InputValue value)
    {
        if (value.isPressed)
        {
            _appliedSpeed = _runSpeed;
        }
        else
        {
            _appliedSpeed = _walkSpeed;
        }
    }
    
    void CheckGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 1f))
        {
            if (((1 << hit.collider.gameObject.layer) & _floorLayer) != 0)
            {
                Debug.Log("붙어있음");
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
