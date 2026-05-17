using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private int _currentHP = 0;
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private float _currentSt = 0f;
    [SerializeField] private float _maxSt = 100f;
    [SerializeField] private float _rechargeStValue = 10f;

    private bool _isConsumingStamina;

    [SerializeField] private float _infiniteStaminaDuration = 30f;

    private bool _isInBuffZone;
    private bool _hasInfiniteStamina;
    private float _infiniteStaminaTimer;

    public bool HasInfiniteStamina => _hasInfiniteStamina;

    public int CurrentHP => _currentHP;
    public int MaxHP => _maxHP;
    public bool IsDead => _currentHP <= 0;
    public float CurrentSt => _currentSt;
    public float MaxSt => _maxSt;

    public Action<int> changeHpEvent;
    public Action<float> changeStaminaEvent;
    public Action<float> changeBuffTimerEvent;

    public Action hitEvent;
    public Action deathEvent;
    public Action restartEvent;


    void Awake()
    {
        ResetStatus();
    }

    void Update()
    {
        HandleInfiniteStaminaTimer();

        if (_isConsumingStamina) return;

        if (_currentSt >= _maxSt)
        {
            _currentSt = Mathf.Min(_currentSt, _maxSt);
            return;
        }

        RechargeStamina();
    }
    
    private void HandleInfiniteStaminaTimer()
    {
        if (!_hasInfiniteStamina) return;

        if (_isInBuffZone)
        {
            return;
        }

        _infiniteStaminaTimer -= Time.deltaTime;
        changeBuffTimerEvent?.Invoke(_infiniteStaminaTimer);

        if (_infiniteStaminaTimer <= 0f)
        {
            _hasInfiniteStamina = false;
            changeBuffTimerEvent?.Invoke(_infiniteStaminaTimer);
            _infiniteStaminaTimer = 0f;
        }
    }

    public void Heal(int hp)
    {
        _currentHP += hp;
        _currentHP = Mathf.Min(_currentHP, _maxHP);
        changeHpEvent?.Invoke(_currentHP);
    }

    public void TakeDamage(int dmg)
    {
        if (IsDead) return;

        _currentHP -= dmg;
        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);
        changeHpEvent?.Invoke(_currentHP);

        if (IsDead)
        {
            deathEvent?.Invoke();
            return;
        }
        hitEvent?.Invoke();
    }


    public void SetConsumingStamina(bool isConsuming)
    {
        _isConsumingStamina = isConsuming;
    }

    public void ConsumeStamina(float value)
    {
        if (_hasInfiniteStamina)
        {
            changeStaminaEvent?.Invoke(_currentSt);
            return;
        }

        _currentSt -= value;
        _currentSt = Mathf.Max(_currentSt, 0f);

        changeStaminaEvent?.Invoke(_currentSt);
    }

    public void RechargeStamina()
    {
        _currentSt += Time.deltaTime * _rechargeStValue;
        _currentSt = Mathf.Clamp(_currentSt, 0, _maxSt);
        changeStaminaEvent?.Invoke(_currentSt);
    }

    public void HealStamina(int value)
    {
        _currentSt += value;
        _currentSt = Mathf.Min(_currentSt, _maxSt);
        changeStaminaEvent?.Invoke(_currentSt);
    }
    
    public void EnterBuffZone()
    {
        _isInBuffZone = true;

        _currentSt = _maxSt;
        changeStaminaEvent?.Invoke(_currentSt);

        _hasInfiniteStamina = true;
        _infiniteStaminaTimer = _infiniteStaminaDuration;
    }

    public void ExitBuffZone()
    {
        _isInBuffZone = false;
        _infiniteStaminaTimer = _infiniteStaminaDuration;
    }

    public void ResetStatus()
    {
        _currentHP = _maxHP;
        _currentSt = _maxSt;

        _isConsumingStamina = false;
        _isInBuffZone = false;
        _hasInfiniteStamina = false;
        _infiniteStaminaTimer = 0f;
        restartEvent?.Invoke();
        changeHpEvent?.Invoke(_currentHP);
        changeStaminaEvent?.Invoke(_currentSt);
        changeBuffTimerEvent?.Invoke(_infiniteStaminaTimer);
    }

}
