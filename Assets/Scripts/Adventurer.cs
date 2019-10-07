using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adventurer : MonoBehaviour
{
    public static void onAdvanceDay()
    {
        for (int i = 0; i < BusinessManager.instance.getData(HireeType.Adventurer).numberHired; ++i)
        {
            int itemsRetreived = 1 + Random.Range(0, 5);
            for (int j = 0; j < itemsRetreived; ++j)
            {
                if (Random.Range(0, 6) == 0)
                    PlayerInventory.instance.addItem(ItemDatabase.instance.getRandomItem());
                else
                    PlayerInventory.instance.addItem(ItemDatabase.instance.getRandomItem(40));
            }
            MessageQueue.addToQueue("An aventurer returned with " + itemsRetreived + " items for you.");
        }
    }
}
