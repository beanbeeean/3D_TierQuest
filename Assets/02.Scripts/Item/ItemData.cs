using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/Item Data")]
public class ItemData : ScriptableObject
{
    public ItemType type;
    public string itemId;
    public string itemName;
    public string description;
    public Sprite icon;
    public int value;
}