using System.Collections;
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

    public Item getRandomItem(bool includeCoolAxe, int maxValue = -1)
    {
        Item toReturn;
        do
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
                    toReturn = items[Random.Range(0, items.Count)];
                else
                    toReturn = new Item() { isNull = true };
            }
            else
            {
                toReturn = m_allItems.items[Random.Range(0, m_allItems.items.Length)];
            }
        } while (toReturn.name == "Cool Axe" && !includeCoolAxe);
        return toReturn;
    }

    public Sprite getSprite(int idx)
    {
        return m_imageDatabase[idx];
    }
}
