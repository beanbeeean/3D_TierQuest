using UnityEngine;

public class Potion : MonoBehaviour
{
    private ItemSpawner _itemSpawner;

    [SerializeField] private int _healValue = 50;
    [SerializeField] private LayerMask _playerLayer;

    void Awake()
    {
        _itemSpawner = GetComponentInParent<ItemSpawner>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _playerLayer.value) == 0)
        {
            return;
        }

        PlayerStatus player = other.GetComponentInParent<PlayerStatus>();

        if (player != null)
        {
            player.Heal(_healValue);
        }

        if (_itemSpawner != null)
        {
            _itemSpawner.SpawnItem();
        }

        Destroy(gameObject);
    }
}