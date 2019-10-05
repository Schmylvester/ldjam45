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
    Null = -3,
    
    None,
    Consumable,
    
    Weapon,
    Shield,
    Headwear,
    Footwear,
    Gloves,
    Clothing,

    EQUIP_COUNT
}

[System.Serializable]
public struct Item
{
    public bool isNull;
    public string name;
    public string description;
    public ItemType type;
    public int baseValue;
    public Rarity rarity;
    public int spriteIdx;
    public string[] traits;
}