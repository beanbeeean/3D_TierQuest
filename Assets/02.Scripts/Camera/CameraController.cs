using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target; 
    [SerializeField] private Vector3 _offset = new Vector3(0, 2, -6);
    [SerializeField] private float _sensitivity = 0.1f; 
    
    private float _xRotation;
    private float _yRotation;

    private Vector2 _mouseDelta;
    private bool _isRotating = false;

    void Start()
    {
        _xRotation = transform.eulerAngles.x;
        _yRotation = transform.eulerAngles.y;
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
            _xRotation = Mathf.Clamp(_xRotation, 10f, 80f);
        }
    }

    private void FollowTarget()
    {
        Quaternion rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        float distance = Mathf.Abs(_offset.z); 
        Vector3 dir = new Vector3(0, 0, -distance);
        transform.position = _target.position + (rotation * dir) + (Vector3.up * _offset.y);
        transform.LookAt(_target.position);
    }
}