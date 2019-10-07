using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferFrom : TransferMenu
{
    protected override List<Item> getItems()
    {
        List<Item> returnList = new List<Item>();
        var items = BusinessManager.instance.getData(storeIdx).itemsForSale;
        for (int i = 0; i < items.Count; ++i)
        {
            bool onList = false;
            for (int j = 0; j < i; ++j)
            {
                if (items[i].item.name == items[j].item.name)
                {
                    onList = true;
                    break;
                }
            }
            if (!onList)
                returnList.Add(items[i].item);
        }
        return returnList;
    }

    protected override List<int> getCounts()
    {
        List<int> returnList = new List<int>();
        for (int i = 0; i < BusinessManager.instance.getData(storeIdx).itemsForSale.Count; ++i)
        {
            for (int j = 0; j < i; ++j)
            {
                if (BusinessManager.instance.getData(storeIdx).itemsForSale[i].item.name ==
                BusinessManager.instance.getData(storeIdx).itemsForSale[j].item.name)
                {
                    continue;
                }
            }
            int count = 1;
            for (int j = i + 1; j < BusinessManager.instance.getData(storeIdx).itemsForSale.Count; ++j)
            {
                if (BusinessManager.instance.getData(storeIdx).itemsForSale[i].item.name ==
                BusinessManager.instance.getData(storeIdx).itemsForSale[j].item.name)
                {
                    count++;
                }
            }
            returnList.Add(count);
        }
        return returnList;
    }

    protected override int initValue(Item item)
    {
        for (int i = 0; i < BusinessManager.instance.getData(storeIdx).itemsForSale.Count; ++i)
        {
            if (BusinessManager.instance.getData(storeIdx).itemsForSale[i].item.name == item.name)
            {
                return BusinessManager.instance.getData(storeIdx).itemsForSale[i].cost;
            }
        }
        return 0;
    }
}
