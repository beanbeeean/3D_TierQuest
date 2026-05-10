using UnityEngine;

public class BuffZone : MonoBehaviour
{
    [SerializeField] private LayerMask _playerLayer;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _playerLayer.value) == 0)
        {
            return;
        }

        PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();

        if (playerStatus == null)
        {
            return;
        }

        playerStatus.EnterBuffZone();
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & _playerLayer.value) == 0)
        {
            return;
        }

        PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();

        if (playerStatus == null)
        {
            return;
        }

        playerStatus.ExitBuffZone();
    }
}