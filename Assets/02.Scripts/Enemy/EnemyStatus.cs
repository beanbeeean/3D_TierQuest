using System;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField] private int _currentHP = 0;
    [SerializeField] private int _maxHP = 100;

    public Action hitEvent;
    public Action deathEvent;
    public Action<int> changeHpEvent;

    public int CurrentHP => _currentHP;
    public int MaxHP => _maxHP;
    public bool IsDead => _currentHP <= 0;

    void Awake()
    {
        ResetStatus();
    }

    public void ResetStatus()
    {
        _currentHP = _maxHP;
        changeHpEvent?.Invoke(_currentHP);
    }

    public void TakeDamage(int dmg)
    {
        _currentHP -= dmg;
        _currentHP = Mathf.Max(_currentHP, 0);

        hitEvent?.Invoke();
        changeHpEvent?.Invoke(_currentHP);

        if (IsDead)
        {
            deathEvent?.Invoke();
        }
    }
}