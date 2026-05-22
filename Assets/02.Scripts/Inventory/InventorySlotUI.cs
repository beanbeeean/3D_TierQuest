using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _amount;
    [SerializeField] private Button _slotButton;

    private int _slotIndex;
    private Action<int> _clickAction;

    public void Initialize(int slotIndex, Action<int> clickAction)
    {
        _slotIndex = slotIndex;
        _clickAction = clickAction;

        _slotButton.onClick.RemoveListener(OnClickSlot);
        _slotButton.onClick.AddListener(OnClickSlot);
    }

    public void SetSlot(InventorySlot slot)
    {
        _icon.sprite = slot.itemData.icon;
        _icon.gameObject.SetActive(true);

        _amount.text = slot.amount.ToString();
        _amount.gameObject.SetActive(true);
    }

    public void ClearSlot()
    {
        _icon.sprite = null;
        _icon.gameObject.SetActive(false);

        _amount.text = "";
        _amount.gameObject.SetActive(false);
    }

    private void OnClickSlot()
    {
        _clickAction?.Invoke(_slotIndex);
    }
}