using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyMenu : ShopMenu
{
    protected override List<Item> getItems()
    {
        return StoreInventory.instance.items[storeIdx];
    }

    protected override List<int> getCounts()
    {
        return StoreInventory.instance.counts[storeIdx];
    }

    protected override int initValue(Item item)
    {
        if(storeIdx == 0)
            return (int)(item.baseValue * 1.6f);
        else
            return (int)(item.baseValue * 1.4f);
    }
}
