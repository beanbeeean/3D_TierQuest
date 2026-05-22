using System.Collections;
using TMPro;
using UnityEngine;

public class ShopUIController : MonoBehaviour
{
    [SerializeField] private ShopItemData[] _shopItems;
    [SerializeField] private ShopSlotUI[] _slotUIs;
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private TextMeshProUGUI _popupText;
    [SerializeField] private float _popupTimer = 2f;
    [SerializeField] private ShopPopupData[] _popupDatas;

    private ShopItemData _currentItem;

    private bool _isOpened = false;

    private int _amount = 1;
    private int _totalPrice = 0;

    private Coroutine _popupCoroutine;

    void Start()
    {
        InitializeShop();
        InitalizeData();
        UpdateAmountText();

        _popupText.gameObject.SetActive(false);
    }

    public void ToggleShopUI()
    {
        _isOpened = !_isOpened;
        gameObject.SetActive(_isOpened);

        if (_isOpened)
        {
            InitalizeData();
            UpdateAmountText();
            ClearSelectedSlot();

            _inventoryUI.OpenInventory();
        }
        else
        {
            _inventoryUI.CloseInventory();
        }
    }

    private void InitializeShop()
    {
        for (int i = 0; i < _shopItems.Length; i++)
        {
            _slotUIs[i].Initialize(_shopItems[i], OnClickShopItem);
        }
    }

    private void OnClickShopItem(ShopItemData data)
    {
        if (_currentItem != null &&
            data.itemData.itemId == _currentItem.itemData.itemId)
        {
            InitalizeData();
            UpdateAmountText();
            ClearSelectedSlot();
            return;
        }

        _currentItem = data;
        _amount = 1;

        UpdateAmountText();

        int selectedIndex = GetShopItemIndex(data);
        SelectSlot(selectedIndex);
    }

    public void IncreaseAmount()
    {
        _amount++;
        UpdateAmountText();
    }

    public void DecreaseAmount()
    {
        if (_amount <= 1)
        {
            _amount = 1;
            return;
        }

        _amount--;
        UpdateAmountText();
    }

    public void BuyItem()
    {
        if (_currentItem == null)
        {
            ShowPopup(ShopPopupType.NoSelectedItem);
            return;
        }

        _totalPrice = _currentItem.price * _amount;

        if (!_inventory.CanAfford(_totalPrice))
        {
            ShowPopup(ShopPopupType.NotEnoughGold);
            return;
        }

        _inventory.SpendGold(_totalPrice);
        _inventory.AddItem(_currentItem.itemData, _amount);

        ShowPopup(ShopPopupType.PurchaseComplete);

        InitalizeData();
        UpdateAmountText();
        ClearSelectedSlot();
    }

    private void UpdateAmountText()
    {
        _amountText.text = _amount.ToString();
    }

    private void InitalizeData()
    {
        _currentItem = null;
        _amount = 1;
    }

    private void SelectSlot(int slotIndex)
    {
        ClearSelectedSlot();

        if (slotIndex < 0 || slotIndex >= _slotUIs.Length)
            return;

        _slotUIs[slotIndex].SetSelected(true);
    }

    private void ClearSelectedSlot()
    {
        for (int i = 0; i < _slotUIs.Length; i++)
        {
            _slotUIs[i].SetSelected(false);
        }
    }

    private int GetShopItemIndex(ShopItemData data)
    {
        for (int i = 0; i < _shopItems.Length; i++)
        {
            if (_shopItems[i].itemData.itemId == data.itemData.itemId)
            {
                return i;
            }
        }

        return -1;
    }

    private void ShowPopup(ShopPopupType type)
    {
        if (_popupCoroutine != null)
        {
            StopCoroutine(_popupCoroutine);
        }

        _popupCoroutine = StartCoroutine(PopupRoutine(type));
    }

    private IEnumerator PopupRoutine(ShopPopupType type)
    {
        _popupText.gameObject.SetActive(true);

        ShopPopupData popupData = GetPopupData(type);

        if (popupData != null)
        {
            _popupText.text = popupData.message;
            _popupText.color = popupData.color;
        }

        yield return new WaitForSeconds(_popupTimer);

        _popupText.gameObject.SetActive(false);

        _popupCoroutine = null;
    }

    private ShopPopupData GetPopupData(ShopPopupType type)
    {
        for (int i = 0; i < _popupDatas.Length; i++)
        {
            if (_popupDatas[i].type == type)
            {
                return _popupDatas[i];
            }
        }

        return null;
    }
}