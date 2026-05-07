using System.Collections;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    private PlayerController _controller;
    private PlayerAnimator _animator;
    private Hitbox _hitbox;

    [SerializeField] private GameObject _hitBoxObject;
    [SerializeField] private float _hitBoxActiveTime = 0.2f;
    [SerializeField] private float _cooldownTimer = 3f;
    [SerializeField] private int _damage = 10;

    private bool _isCooldown = false;

    public int Damage => _damage;
    public bool IsCooldown => _isCooldown;

    void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _animator = GetComponent<PlayerAnimator>();
        _hitbox = _hitBoxObject.GetComponent<Hitbox>();

        if (_hitBoxObject != null)
        {
            _hitBoxObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        _controller.attackAction += MeleeAttack;
    }

    void OnDisable()
    {
        _controller.attackAction -= MeleeAttack;
    }

    private void MeleeAttack()
    {
        if (_isCooldown) return;

        _animator.AttackAnimator();
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        _isCooldown = true;
        _hitbox.ClearHitEnemies();
        _hitBoxObject.SetActive(true);

        yield return new WaitForSeconds(_hitBoxActiveTime);

        _hitBoxObject.SetActive(false);

        yield return new WaitForSeconds(_cooldownTimer);

        _isCooldown = false;
    }
}