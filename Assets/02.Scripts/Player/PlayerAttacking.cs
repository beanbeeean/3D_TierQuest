using System;
using System.Collections;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    private PlayerController _controller;
    private PlayerStatus _status;
    private Hitbox _hitbox;

    [SerializeField] private GameObject _hitBoxObject;
    [SerializeField] private float _hitBoxActiveTime = 0.2f;
    [SerializeField] private float _cooldownTimer = 3f;
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackStamina = 10f;

    private bool _isCooldown = false;

    public int Damage => _damage;
    public bool IsCooldown => _isCooldown;

    public Action attackEvent;

    void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _hitbox = _hitBoxObject.GetComponent<Hitbox>();
        _status = GetComponent<PlayerStatus>();

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
        if (_status.CurrentSt <= _attackStamina) return;
        if (_isCooldown) return;

        attackEvent?.Invoke();
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        _isCooldown = true;
        _hitbox.ClearHitEnemies();
        _hitBoxObject.SetActive(true);
        _status.ConsumeStamina(_attackStamina);

        yield return new WaitForSeconds(_hitBoxActiveTime);

        _hitBoxObject.SetActive(false);

        yield return new WaitForSeconds(_cooldownTimer);

        _isCooldown = false;
    }
}