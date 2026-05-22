using UnityEngine;

public class ShopNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private ShopUIController _shopUI;
    [SerializeField] private GameObject _NpcPopup;

    public void Interact()
    {
        _shopUI.ToggleShopUI();
    }

    public void ActivatePopup()
    {
        _NpcPopup.SetActive(true);
    }

    public void InActivatePopup()
    {
        _NpcPopup.SetActive(false);
    }
}