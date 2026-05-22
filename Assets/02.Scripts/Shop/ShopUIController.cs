using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIController : MonoBehaviour
{
    [SerializeField] private ShopItemData[] _shopItems;
    [SerializeField] private ShopSlotUI[] _slotUIs;
    [SerializeField] private PlayerInventory _inventory;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private InventoryUI _inventoryUI;
    [SerializeField] private TextMeshProUGUI _popupText;
    [SerializeField] private string _successMsg = "Purchase complete.";
    [SerializeField] private string _failMsg = "Not enough gold.";
    [SerializeField] private float _popupTimer = 2f;
    private ShopItemData _currentItem;
    private bool _isOpened = false;
    private bool _isSuccessPopup;
    private Coroutine _popupCoroutine;

    private int _amount = 1;
    private int _totalPrice = 0;

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
        if (_currentItem != null && data.itemData.itemId == _currentItem.itemData.itemId)
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
            Debug.Log("<color=red>구매할 아이템을 선택하세요.</color>");
            return;
        }

        _totalPrice = _currentItem.price * _amount;

        if (!_inventory.CanAfford(_totalPrice))
        {
            _isSuccessPopup = false;

            if (_popupCoroutine != null)
            {
                StopCoroutine(_popupCoroutine);
            }

            _popupCoroutine = StartCoroutine(PopupRoutine());

            return;
        }

        _inventory.SpendGold(_totalPrice);
        _inventory.AddItem(_currentItem.itemData, _amount);

        _isSuccessPopup = true;

        if (_popupCoroutine != null)
        {
            StopCoroutine(_popupCoroutine);
        }

        _popupCoroutine = StartCoroutine(PopupRoutine());


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
    
    private IEnumerator PopupRoutine()
    {
        _popupText.gameObject.SetActive(true);

        if (_isSuccessPopup)
        {
            _popupText.text = _successMsg;
            _popupText.color = Color.green;
        }
        else
        {
            _popupText.text = _failMsg;
            _popupText.color = Color.red;
        }

        yield return new WaitForSeconds(_popupTimer);

        _popupText.gameObject.SetActive(false);

        _popupCoroutine = null;
    }

}