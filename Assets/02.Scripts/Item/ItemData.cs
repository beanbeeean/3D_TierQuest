using UnityEngine;

[System.Serializable]
public struct ItemData
{
    public ItemType type;
    public string itemId;
    public string itemName;
    public string description;
    public Sprite icon;
    public int value;
}