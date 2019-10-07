﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public struct AllItems
{
    public Item[] items;
}

public class ItemDatabase : MonoBehaviour
{
    [HideInInspector] public static ItemDatabase instance;
    AllItems m_allItems;
    [SerializeField] Sprite[] m_imageDatabase;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            string filePath = Path.Combine(Application.streamingAssetsPath, "Items.json");
            string dataAsJson = File.ReadAllText(filePath);
            m_allItems = JsonUtility.FromJson<AllItems>(dataAsJson);

            for (int i = 0; i < m_allItems.items.Length; ++i)
            {
                for (int j = i + 1; j < m_allItems.items.Length; ++j)
                {
                    if (m_allItems.items[i].name == m_allItems.items[j].name)
                    {
                        Debug.LogWarning("Duplicate Item Found");
                    }
                }
            }
        }
    }

    public Item getItem(string itemName)
    {
        foreach (Item item in m_allItems.items)
        {
            if (item.name == itemName)
            {
                return item;
            }
        }

        Debug.LogError("No");
        return new Item();
    }

    public Item getRandomItem(int maxValue = -1)
    {
        if (maxValue > 0)
        {
            List<Item> items = new List<Item>();
            for (int i = 0; i < m_allItems.items.Length; ++i)
            {
                if (m_allItems.items[i].baseValue < maxValue)
                    items.Add(m_allItems.items[i]);
            }
            if (items.Count > 0)
                return items[Random.Range(0, items.Count)];
            return new Item() { isNull = true };
        }
        return m_allItems.items[Random.Range(0, m_allItems.items.Length)];
    }

    public Sprite getSprite(int idx)
    {
        return m_imageDatabase[idx];
    }
}
