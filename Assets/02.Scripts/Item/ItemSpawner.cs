using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _potionPrefab;
    [SerializeField] private float _cooldownTimer = 5f;

    private GameObject _potion;
    private Coroutine _spawnCoroutine;

    void Start()
    {
        _potion = Instantiate(_potionPrefab, transform);
        _potion.SetActive(true);
    }

    public void InactiveItem()
    {
        _potion.SetActive(false);
    }

    public void SpawnItem()
    {
        if (_spawnCoroutine != null)
        {
            return;
        }

        _spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(_cooldownTimer);

        _potion.transform.localPosition = Vector3.zero;
        _potion.transform.localRotation = Quaternion.identity;
        _potion.SetActive(true);

        _spawnCoroutine = null;
    }
}