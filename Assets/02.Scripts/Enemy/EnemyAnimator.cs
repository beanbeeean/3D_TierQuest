using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{

    private EnemyStatus _enemyStatus;
    private Animator _anim;

    private readonly int _isDeadHash = Animator.StringToHash("IsDead");
    private readonly int _hitTriggerHash = Animator.StringToHash("HitTrigger");


    void Awake()
    {
        _anim = GetComponent<Animator>();
        _enemyStatus = GetComponent<EnemyStatus>();
    }

    void OnEnable()
    {
        _enemyStatus.hitEvent += HitAnimator;
        _enemyStatus.deathEvent += DeathAnimator;
    }

    void OnDisable()
    {
        _enemyStatus.hitEvent -= HitAnimator;
        _enemyStatus.deathEvent -= DeathAnimator;
    }

    void HitAnimator()
    {
        _anim.SetTrigger(_hitTriggerHash);
    }
    
    void DeathAnimator()
    {
        _anim.SetBool(_isDeadHash, true);
    }
}
