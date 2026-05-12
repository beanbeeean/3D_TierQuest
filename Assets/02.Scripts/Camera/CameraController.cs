using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target; 
    [SerializeField] private Vector3 _offset = new Vector3(0, 2, -6);
    [SerializeField] private float _sensitivity = 0.1f;
    [SerializeField] private float _minRotationX = 0f;
    [SerializeField] private float _maxRotationX = 60f;
    
    private float _xRotation;
    private float _yRotation;

    private Vector2 _mouseDelta;
    private bool _isRotating = false;

    private float _initialXRotation;
    private float _initialYRotation;
    private Vector3 _initialOffset;

    void Start()
    {
        _xRotation = transform.eulerAngles.x;
        _yRotation = transform.eulerAngles.y;

        _initialXRotation = _xRotation;
        _initialYRotation = _yRotation;
        _initialOffset = _offset;
    }

    void LateUpdate()
    {
        if (_target == null) return;

        HandleInput();
        FollowTarget();
        
    }

    private void HandleInput()
    {
        _isRotating = Mouse.current.rightButton.isPressed;

        if (_isRotating)
        {
            _mouseDelta = Mouse.current.delta.ReadValue();
            _yRotation += _mouseDelta.x * _sensitivity;
            _xRotation -= _mouseDelta.y * _sensitivity;
            _xRotation = Mathf.Clamp(_xRotation, _minRotationX, _maxRotationX);
        }
    }

    private void FollowTarget()
    {
        Quaternion rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        Vector3 rotatedOffset = rotation * _offset;

        transform.position = _target.position + rotatedOffset;

        transform.LookAt(_target.position);
    }

    public void ResetCamera()
    {
        _xRotation = _initialXRotation;
        _yRotation = _initialYRotation;
        _offset = _initialOffset;

        FollowTarget();
    }
}