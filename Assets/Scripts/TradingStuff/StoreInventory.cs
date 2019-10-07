using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreInventory : MonoBehaviour
{
    public static StoreInventory instance = null;
    public List<Item>[] items = new List<Item>[3]
    {
        new List<Item>(),
        new List<Item>(),
        new List<Item>(),
    };
    public List<int>[] counts = new List<int>[3]
    {
        new List<int>(),
        new List<int>(),
        new List<int>(),
    };

    public string playerAtStall = "";

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void initStoreInventory()
    {
        BusinessManager b = BusinessManager.instance;
        if (b.getData(BusinessType.Stall).owned || b.getData(BusinessType.Stall).rented)
            initPlayerInventory(2);
        if (b.getData(BusinessType.LittleShop).owned)
            initPlayerInventory(0);
        else
            initNPCInventory(0);
        if (b.getData(BusinessType.BigShop).owned)
            initPlayerInventory(1);
        else
            initNPCInventory(1);
    }

    void initPlayerInventory(int store)
    {
        BusinessData data;
        switch (store)
        {
            case 0:
                data = BusinessManager.instance.getData(BusinessType.LittleShop);
                break;
            case 1:
                data = BusinessManager.instance.getData(BusinessType.BigShop);
                break;
            default:
                data = BusinessManager.instance.getData(BusinessType.Stall);
                break;
        }
        string message = "";
        for (int i = 0; i < data.itemsForSale.Count; ++i)
        {
            if (Random.Range(0.0f, 1.0f) < getStolenChance(data))
            {
                MessageQueue.addToQueue(data.itemsForSale[i].item.name + " was stolen from " + data.name + ". Assign a guard to this store to reduce the risk of this.");
                data.itemsForSale.RemoveAt(i--);
                continue;
            }
            else if (Random.Range(0.0f, 1.0f) < getSoldChance(data.itemsForSale[i], data, ref message))
            {
                MessageQueue.addToQueue(data.itemsForSale[i].item.name + " was sold for £" + data.itemsForSale[i].cost + ".");
                PlayerInventory.instance.changeCash(data.itemsForSale[i].cost);
                data.itemsForSale.RemoveAt(i--);
            }
        }
        if (message != "")
            MessageQueue.addToQueue(message);
    }

    void initNPCInventory(int store)
    {
        int money = 0;
        for (int i = 0; i < 6 && i < items[store].Count; ++i)
        {
            if (Random.Range(0, 2) == 0)
            {
                int idx = Random.Range(0, items[store].Count);
                money += items[store][idx].baseValue;
                removeItem(items[store][idx].name, store);
            }
        }
        while (money >= 10)
        {
            Item item = ItemDatabase.instance.getRandomItem(money);
            if (!item.isNull)
            {
                money -= item.baseValue;
                addItem(item, store);
            }
            else
            {
                break;
            }
        }
    }

    public void addItem(Item item, int store, int count = 1)
    {
        for (int i = 0; i < items[store].Count; ++i)
        {
            if (items[store][i].name == item.name)
            {
                counts[store][i] += count;
                return;
            }
        }
        items[store].Add(item);
        counts[store].Add(count);
    }

    public bool removeItem(string item, int store, int count = 1)
    {
        for (int i = 0; i < items[store].Count; ++i)
        {
            if (items[store][i].name == item)
            {
                if (count <= counts[store][i])
                {
                    counts[store][i] -= count;
                    if (counts[store][i] == 0)
                    {
                        counts[store].RemoveAt(i);
                        items[store].RemoveAt(i);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
    }

    float getStolenChance(BusinessData data)
    {
        float stolenChance = 0.1f;
        foreach (HireeType hireeData in data.workersAssigned)
        {
            if (hireeData == HireeType.Guard)
                stolenChance *= 0.3f;
            if (hireeData == HireeType.Sales)
                stolenChance *= 0.8f;
        }
        return stolenChance;
    }

    float getSoldChance(ItemForSale item, BusinessData data, ref string message)
    {
        if (!data.workersAssigned.Contains(HireeType.Sales) && playerAtStall != data.name)
        {
            message = ("You should assign a sales assistant to your " + data.name + " so it can operate while you are busy.");
            return 0;
        }
        float soldChance = 0.6f;
        foreach (HireeType hireeData in data.workersAssigned)
        {
            if (hireeData == HireeType.Promoter)
                soldChance *= 1.7f;
            if (hireeData == HireeType.Sales)
                soldChance *= 1.2f;
        }

        soldChance *= item.item.baseValue / item.cost;

        return soldChance;
    }
}