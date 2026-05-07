using UnityEngine;
using System.Collections.Generic;

public class Hitbox : MonoBehaviour
{
    private PlayerAttacking _attacking;

    [SerializeField] private LayerMask _enemyLayer;
    private readonly HashSet<EnemyStatus> _hitEnemies = new HashSet<EnemyStatus>();

    void Awake()
    {
        _attacking = GetComponentInParent<PlayerAttacking>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _enemyLayer.value) == 0)
        {
            return;
        }

        EnemyStatus enemy = other.GetComponentInParent<EnemyStatus>();

        if (enemy == null)
        {
            return;
        }

        if (_hitEnemies.Contains(enemy))
        {
            return;
        }

        _hitEnemies.Add(enemy);
        enemy.TakeDamage(_attacking.Damage);
    }

    public void ClearHitEnemies()
    {
        _hitEnemies.Clear();
    }
}