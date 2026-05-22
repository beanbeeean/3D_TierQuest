using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDetailUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _amount;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private Image _icon;
    [SerializeField] private Button _useButton;
    [SerializeField] private Button _closeBtn;

    private InventorySlot _current;
    private PlayerInventory _inventory;
    private GameObject _inv;

    void Awake()
    {
        _useButton.onClick.AddListener(ClickUseButton);
        _closeBtn.onClick.AddListener(CloseDetailUI);
    }

    public void Initialize(InventorySlot slot, PlayerInventory inventory, GameObject invObj)
    {
        _inv = invObj;
        _inventory = inventory;
        _current = slot;

        _amount.text = slot.amount.ToString();
        _itemName.text = slot.itemData.itemName;
        _description.text = slot.itemData.description;
        _icon.sprite = slot.itemData.icon;

    }

    private void ClickUseButton()
    {
        if (_current == null)
            return;

        _inventory.UseItem(_current);
        if(_current.amount <= 0)
        {
            CloseDetailUI();
        }
    }

    private void CloseDetailUI()
    {
        // _inv.SetActive(true);
        _inv.GetComponentInParent<InventoryUI>().ResetSelectedSlot();
        gameObject.SetActive(false);
    }
}