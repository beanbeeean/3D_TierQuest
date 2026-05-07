using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _anim;
    private PlayerController _controller;

    private readonly int _moveSpeedHash = Animator.StringToHash("MoveSpeed");
    private readonly int _isGroundedHash = Animator.StringToHash("IsGrounded");
    private readonly int _attackTriggerHash = Animator.StringToHash("AttackTrigger");

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _controller = GetComponent<PlayerController>();
    }


    void Update()
    {
        float currentSpeed = _controller.MoveVector.magnitude > 0 ? _controller.AppliedSpeed : 0;
        _anim.SetFloat(_moveSpeedHash, currentSpeed, 0.1f, Time.deltaTime);
        _anim.SetBool(_isGroundedHash, _controller.IsGrounded);
    }

    public void AttackAnimator()
    {
        _anim.SetTrigger(_attackTriggerHash);
    }


   
}
