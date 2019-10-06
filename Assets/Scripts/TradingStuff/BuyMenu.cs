using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyMenu : ShopMenu
{
    protected override List<Item> getItems()
    {
        return StoreInventory.instance.items;
    }

    protected override List<int> getCounts()
    {
        return StoreInventory.instance.counts;
    }

    protected override int initValue(Item item)
    {
        return (int)(item.baseValue * 1.6f);
    }
}
