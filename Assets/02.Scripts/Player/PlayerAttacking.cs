using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    private PlayerController _controller;
    [SerializeField] private GameObject _meleePoint;
    private bool _isCooldown = false;
    [SerializeField] private float _cooldownTimer = 3f;
    private float _timer = 0f;

    void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        _controller.attackAction += MeleeAttack;
    }

    void OnDisable()
    {
        _controller.attackAction -= MeleeAttack;
    }

    void MeleeAttack()
    {
        if (_isCooldown) return;
        StartCoroutine(AttackRoutine());
    }
    
    IEnumerator AttackRoutine()
    {
        _isCooldown = true;
        // 데미지 주는거 불러와야됨 (Enemy 만들어서)
        
        while (_timer < _cooldownTimer)
        {
            _timer += Time.deltaTime;
            yield return null;
        }

        _isCooldown = false;
    }
}
