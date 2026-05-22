using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private Button _slotButton;
    [SerializeField] private Image _slotBackground;
    [SerializeField] private Color _normalColor = Color.white;
    [SerializeField] private Color _selectedColor = Color.green;

    private ShopItemData _data;
    private Action<ShopItemData> _clickAction;

    public void Initialize(ShopItemData data, Action<ShopItemData> clickAction)
    {
        _data = data;
        _clickAction = clickAction;

        _icon.sprite = data.itemData.icon;
        _priceText.text = $"{data.price}G";

        _slotButton.onClick.RemoveAllListeners();
        _slotButton.onClick.AddListener(OnClickSlot);
    }

    private void OnClickSlot()
    {
        _clickAction?.Invoke(_data);
    }

    public void SetSelected(bool isSelected)
    {
        if (isSelected)
        {
            _slotBackground.color = _selectedColor;
        }
        else
        {
            _slotBackground.color = _normalColor;
        }
    }
}