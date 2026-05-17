using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _cooldownTimer = 5f;

    private GameObject _item;
    private Coroutine _spawnCoroutine;

    void Start()
    {
        _item = Instantiate(_prefab, transform);
        _item.SetActive(true);

        Item item = _item.GetComponentInChildren<Item>();
        item.Initialize(this);
    }

    public void InactiveItem()
    {
        _item.SetActive(false);
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

        _item.transform.localPosition = Vector3.zero;
        _item.transform.localRotation = Quaternion.identity;
        _item.SetActive(true);

        _spawnCoroutine = null;
    }
}