using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private ItemData _itemData;

    private ItemSpawner _spawner;

    public void Initialize(ItemSpawner spawner)
    {
        _spawner = spawner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _playerLayer.value) == 0)
            return;

        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory == null)
            return;

        inventory.AddItem(_itemData, 1);

        _spawner.SpawnItem();
        _spawner.InactiveItem();
    }
}