using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _anim;
    private PlayerController _controller;
    private PlayerAttacking _attacking;
    private PlayerStatus _status;

    private readonly int _moveSpeedHash = Animator.StringToHash("MoveSpeed");
    private readonly int _isGroundedHash = Animator.StringToHash("IsGrounded");
    private readonly int _attackTriggerHash = Animator.StringToHash("AttackTrigger");
    private readonly int _isDeadHash = Animator.StringToHash("IsDead");
    private readonly int _hitTriggerHash = Animator.StringToHash("HitTrigger");

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _controller = GetComponent<PlayerController>();
        _attacking = GetComponent<PlayerAttacking>();
        _status = GetComponent<PlayerStatus>();
    }


    void Update()
    {
        float currentSpeed = _controller.MoveVector.magnitude > 0 ? _controller.AppliedSpeed : 0;
        _anim.SetFloat(_moveSpeedHash, currentSpeed, 0.1f, Time.deltaTime);
        _anim.SetBool(_isGroundedHash, _controller.IsGrounded);
    }

    void OnEnable()
    {
        _attacking.attackEvent += AttackAnimator;
        _status.hitEvent += HitAnimator;
        _status.deathEvent += DeathAnimator;
        _status.restartEvent += RestartAnimator;
    }

    void OnDisable()
    {
        _attacking.attackEvent -= AttackAnimator;
        _status.hitEvent -= HitAnimator;
        _status.deathEvent -= DeathAnimator;
        _status.restartEvent -= RestartAnimator;
    }

    void AttackAnimator()
    {
        _anim.SetTrigger(_attackTriggerHash);
    }

    void HitAnimator()
    {
        _anim.SetTrigger(_hitTriggerHash);
    }

    void DeathAnimator()
    {
        _anim.SetBool(_isDeadHash, true);
    }
    
    void RestartAnimator()
    {
        _anim.SetBool(_isDeadHash, false);
    }


   
}
