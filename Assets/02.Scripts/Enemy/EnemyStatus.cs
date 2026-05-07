using UnityEngine;

public class EnemyStatus : MonoBehaviour
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

    public void TakeDamage(int dmg)
    {
        _currentHP -= dmg;
        if (IsDead)
        {
            PlayDeathAnim();
            Destroy(gameObject);
        }

    }
    
    void PlayDeathAnim()
    {
        
    }
}
