using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private int _currentHP = 0;
    [SerializeField] private int _maxHP = 100;

    public int CurrentHP => _currentHP;
    public int MaxHP => _maxHP;
    public bool IsDead => _currentHP <= 0;

    void Awake()
    {
        _currentHP = _maxHP;
    }

    public void Heal(int hp)
    {
        _currentHP += hp;
        if(_currentHP > _maxHP)
        {
            _currentHP = _maxHP;
        }
    }
    
    public void TakeDamage(int dmg)
    {
        _currentHP -= dmg;
        if (IsDead)
        {
            // PlayerAnimator에서 죽는 애니메이션
            Destroy(gameObject);
        }

    }
}
