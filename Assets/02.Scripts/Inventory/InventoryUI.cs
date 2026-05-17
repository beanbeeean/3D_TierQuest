using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private bool _isOpened = false;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private Button _closeBtn;

    [SerializeField] private PlayerController _controller;
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private InventorySlotUI[] _slotUIs;

    [SerializeField] private ItemDetailUIController _itemDetailUI;

    public bool IsOpened => _isOpened;

    private int _currentSlotIdx;
    private InventorySlot _currentSlot;

    void Awake()
    {
        _closeBtn.onClick.AddListener(ToggleInventory);

        for (int i = 0; i < _slotUIs.Length; i++)
        {
            _slotUIs[i].Initialize(i, OnSlotClicked);
        }

        _itemDetailUI.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        _controller.inventoryAction += ToggleInventory;
        _inventory.OnInventoryChanged += UpdateInventoryUI;
    }

    void OnDisable()
    {
        _controller.inventoryAction -= ToggleInventory;
        _inventory.OnInventoryChanged -= UpdateInventoryUI;
    }

    void ToggleInventory()
    {
        _isOpened = !_isOpened;
        _inventoryPanel.SetActive(_isOpened);

        if (_isOpened)
        {
            UpdateInventoryUI();
        }
        else
        {
            ResetSelectedSlot();
        }
    }

    void UpdateInventoryUI()
    {
        List<InventorySlot> slots = _inventory.Slots;

        for (int i = 0; i < _slotUIs.Length; i++)
        {
            if (i < slots.Count && slots[i].amount > 0)
            {
                _slotUIs[i].SetSlot(slots[i]);
            }
            else
            {
                _slotUIs[i].ClearSlot();
            }
        }

        if (_currentSlot == null || _currentSlot.amount <= 0 || !_inventory.Slots.Contains(_currentSlot))
        {
            ResetSelectedSlot();
        }
        else
        {
            _itemDetailUI.Initialize(_currentSlot, _inventory, _inventoryPanel);
        }
    }

    public void ResetSelectedSlot()
    {
        _currentSlotIdx = -1;
        _currentSlot = null;
    }

    private void OnSlotClicked(int slotIndex)
    {
        if (_currentSlotIdx == slotIndex)
        {
            ResetSelectedSlot();
            return;
        }

        List<InventorySlot> slots = _inventory.Slots;

        if (slotIndex >= slots.Count)
        {
            ResetSelectedSlot();
            return;
        }

        InventorySlot slot = slots[slotIndex];

        if (slot.amount <= 0)
        {
            ResetSelectedSlot();
            return;
        }

        ShowItemDetailUI(slotIndex, slot);
    }

    private void ShowItemDetailUI(int slotIndex, InventorySlot slot)
    {
        _currentSlotIdx = slotIndex;
        _currentSlot = slot;

        _itemDetailUI.gameObject.SetActive(true);
        _itemDetailUI.Initialize(slot, _inventory, _inventoryPanel);
        _inventoryPanel.SetActive(false);
    }
}