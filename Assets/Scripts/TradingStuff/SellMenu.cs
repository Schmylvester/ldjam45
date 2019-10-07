using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellMenu : ShopMenu
{
    protected override List<Item> getItems()
    {
        return PlayerInventory.instance.m_items;
    }

    protected override List<int> getCounts()
    {
        return PlayerInventory.instance.m_counts;
    }

    protected override int initValue(Item item)
    {
        return (int)(item.baseValue * 0.7f);
    }
}
