using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreInventory : MonoBehaviour
{
    public static StoreInventory instance = null;
    public List<Item> items { get; } = new List<Item>();
    public List<int> counts { get; } = new List<int>();

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
        int num = Random.Range(0, 9);
        for (int i = 0; i < num; ++i)
        {
            items.Add(ItemDatabase.instance.getRandomItem());
            counts.Add((Random.Range(1, 10) / 9) + 1);
        }
    }

    public void addItem(Item item, int count = 1)
    {
        for (int i = 0; i < items.Count; ++i)
        {
            if (items[i].name == item.name)
            {
                counts[i] += count;
                return;
            }
        }
        items.Add(item);
        counts.Add(count);
    }

    public bool removeItem(string item, int count = 1)
    {
        for (int i = 0; i < items.Count; ++i)
        {
            if (items[i].name == item)
            {
                if (count <= counts[i])
                {
                    counts[i] -= count;
                    if (counts[i] == 0)
                    {
                        counts.RemoveAt(i);
                        items.RemoveAt(i);
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
}
