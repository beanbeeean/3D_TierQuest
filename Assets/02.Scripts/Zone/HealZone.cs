using UnityEngine;

public class HealZone : MonoBehaviour
{
    [SerializeField] private int _healValue = 20;
    [SerializeField] private float _cooldownTimer = 1f;
    [SerializeField] private LayerMask _playerLayer;

    float _timer = 0f;

    void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & _playerLayer.value) == 0)
        {
            return;
        }

        _timer += Time.deltaTime;

        if (_timer >= _cooldownTimer)
        {
            PlayerStatus player = other.GetComponentInParent<PlayerStatus>();

            if (player != null)
            {
                player.Heal(_healValue);
            }

            _timer = 0f;
        }
    }


}
