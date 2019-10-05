public enum Rarity
{
    Null = -1,

    Common,
    Uncommon,
    Rare,
    Legendary,

    COUNT
}

public enum ItemType
{
    Null = -1,

    None,
    Equippable,
    Consumable,

    COUNT
}

[System.Serializable]
public struct Item
{
    public string name;
    public string description;
    public ItemType type;
    public int baseValue;
    public Rarity rarity;
    public UnityEngine.Sprite sprite;
}