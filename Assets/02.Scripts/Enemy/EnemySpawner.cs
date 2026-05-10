using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyStatus _enemy;
    [SerializeField] private float _disableTimer = 2f;
    [SerializeField] private float _respawnTimer = 10f;

    
    void OnEnable()
    {
        _enemy.deathEvent += HandleEnemyDeath;
    }

    void OnDisable()
    {
        _enemy.deathEvent -= HandleEnemyDeath;
    }

    private void HandleEnemyDeath()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(_disableTimer);

        _enemy.gameObject.SetActive(false);

        yield return new WaitForSeconds(_respawnTimer);

        _enemy.ResetStatus();
        _enemy.gameObject.SetActive(true);
    }
}