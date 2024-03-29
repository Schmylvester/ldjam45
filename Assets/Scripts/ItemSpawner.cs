﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    static public ItemSpawner instance;

    private void Start()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
        }

        instance = this;
    }
    public void Spawn(LootTable lt, Vector2 pos)
    {
        float sumWeight = 0;
        foreach (LootChance l in lt.table)
        {
            sumWeight += l.weight;
        }

        bool droppedItem = false;
        while (!droppedItem)
        {
            foreach (LootChance l in lt.table)
            {
                if (Random.Range(0.0f, 1.0f) < l.weight / sumWeight)
                {
                    droppedItem = true;
                    if (l.itemName == "None") continue;

                    itemPrefab.SetActive(false);
                    GameObject go = Instantiate(itemPrefab);
                    go.GetComponent<Loot>().itemName = l.itemName;
                    go.SetActive(true);
                    itemPrefab.SetActive(true);

                    Vector2 randOffset = new Vector2(Random.Range(-1, 1) * 0.1f, Random.Range(-1, 1) * 0.1f);
                    go.transform.position = pos + randOffset;
                }

                if (Random.Range(0.0f, 1.0f) < 0.05f)
                {
                    droppedItem = true;

                    itemPrefab.SetActive(false);
                    GameObject go = Instantiate(itemPrefab);
                    Item item = ItemDatabase.instance.getRandomItem(false);
                    go.GetComponent<Loot>().itemName = item.name;
                    go.SetActive(true);
                    itemPrefab.SetActive(true);

                    Vector2 randOffset = new Vector2(Random.Range(-1, 1) * 0.1f, Random.Range(-1, 1) * 0.1f);
                    go.transform.position = pos + randOffset;
                }
            }
        }
    }
}
