using System.Collections.Generic;
using UnityEngine;

public class HealZone : MonoBehaviour
{
    [SerializeField] private int _healValue = 20;
    [SerializeField] private float _cooldownTimer = 1f;
    [SerializeField] private LayerMask _playerLayer;

    private float _timer = 0f;
    private HashSet<PlayerStatus> _targets = new HashSet<PlayerStatus>();

    void Update()
    {
        if (_targets.Count <= 0)
        {
            _timer = 0f;
            return;
        }

        _timer += Time.deltaTime;

        if (_timer < _cooldownTimer)
            return;

        foreach (PlayerStatus target in _targets)
        {
            if (target != null)
            {
                target.Heal(_healValue);
            }
        }

        _timer = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _playerLayer.value) == 0)
            return;

        PlayerStatus player = other.GetComponentInParent<PlayerStatus>();

        if (player == null)
            return;

        _targets.Add(player);
    }

    void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & _playerLayer.value) == 0)
            return;

        PlayerStatus player = other.GetComponentInParent<PlayerStatus>();

        if (player == null)
            return;

        _targets.Remove(player);
    }
}