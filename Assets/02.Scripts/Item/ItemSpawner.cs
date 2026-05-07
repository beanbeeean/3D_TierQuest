using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _potionPrefab;
    [SerializeField] private float _cooldownTimer = 5f;

    void Start()
    {
        Instantiate(_potionPrefab, this.transform);
    }

    public void SpawnItem()
    {
        StartCoroutine(SpawnRoutine());
    }
    
    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(_cooldownTimer);

        Instantiate(_potionPrefab, this.transform);
    }

    
}
