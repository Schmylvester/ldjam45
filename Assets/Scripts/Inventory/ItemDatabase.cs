using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    [HideInInspector] public static ItemDatabase instance;
    [SerializeField] Item[] m_allItems;

    private void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        for(int i = 0; i < m_allItems.Length; ++i)
        {
            for(int j = i + 1; j < m_allItems.Length; ++j)
            {
                if(m_allItems[i].name == m_allItems[j].name)
                {
                    Debug.LogWarning("Duplicate Item Found");
                }
            }
        }
    }

    public Item getItem(string itemName)
    {
        foreach(Item item in m_allItems)
        {
            if(item.name == itemName)
            {
                return item;
            }
        }

        Debug.LogError("No");
        return new Item();
    }
}
