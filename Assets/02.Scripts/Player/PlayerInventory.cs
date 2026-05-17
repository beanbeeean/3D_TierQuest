using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private PlayerGoldUIController _goldUI;
    private List<InventorySlot> _slots = new List<InventorySlot>();

    public List<InventorySlot> Slots => _slots;
    
    public event Action OnInventoryChanged;

    private PlayerStatus _status;
    
    private int _playerGold = 0;
    void Awake()
    {
        _status = GetComponent<PlayerStatus>();
    }

    public void AddItem(ItemData itemData, int amount)
    {
        for (int i = 0; i < _slots.Count; i++)
        {
            if (_slots[i].itemData.itemId == itemData.itemId)
            {
                _slots[i].amount += amount;

                Debug.Log($"{itemData.itemName} 획득 / 현재 개수: {_slots[i].amount}");
                OnInventoryChanged?.Invoke();
                return;
            }
        }

        InventorySlot newSlot = new InventorySlot
        {
            itemData = itemData,
            amount = amount
        };

        _slots.Add(newSlot);

        Debug.Log($"{itemData.itemName} 획득 / 총 슬롯 수: {_slots.Count}");

        OnInventoryChanged?.Invoke();
    }

    public void UseItem(InventorySlot slot)
    {
        ItemType currentType = slot.itemData.type;

        switch (currentType)
        {
            case ItemType.HP_Potion:
                if(_status.CurrentHP >= _status.MaxHP)
                {
                    Debug.Log($"<color=red>HP가 이미 최대치입니다.</color>");
                    break;
                }
                slot.amount--;
                _status.Heal(slot.itemData.value);
                break;
            case ItemType.ST_Potion:
                if(_status.CurrentSt >= _status.MaxSt)
                {
                    Debug.Log($"<color=red>Stamina가 이미 최대치입니다.</color>");
                    break;
                }
                slot.amount--;
                _status.HealStamina(slot.itemData.value);
                break;
            case ItemType.Currency:
                slot.amount--;
                _playerGold += slot.itemData.value;
                Debug.Log($"gold : {_playerGold}");
                _goldUI.UpdateGoldHUD(_playerGold);
                break;
            default:
                break;
        }

        if (slot.amount <= 0)
        {
            _slots.Remove(slot);
        }

        OnInventoryChanged?.Invoke();
    }
}