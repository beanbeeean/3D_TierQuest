using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGoldUIController : MonoBehaviour
{
    [SerializeField] private PlayerInventory _playerInventory;
    [SerializeField] private TextMeshProUGUI _goldText;

    public void UpdateGoldHUD(int _value)
    {
        _goldText.text = String.Format($"Gold : {_value}G");
    }
}
