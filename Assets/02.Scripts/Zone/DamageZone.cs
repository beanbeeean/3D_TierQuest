using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField] private int _damageValue = 20;
    [SerializeField] private float _cooldownTimer = 3f;
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
                player.TakeDamage(_damageValue);
            }

            _timer = 0f;
        }
    }
}
