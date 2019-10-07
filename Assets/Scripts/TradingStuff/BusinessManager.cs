using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BusinessType
{
    Stall,
    LittleShop,
    BigShop,

    COUNT
}

public enum HireeType
{
    Sales,
    Adventurer,
    Promoter,
    Guard,

    COUNT
}

public struct ItemForSale
{
    public Item item;
    public int cost;
}

public struct HireeData
{
    public string type;
    public string description;
    public int cost;
    public int numberHired;
    public int numberUnassigned;
}

public struct BusinessData
{
    public string name;
    public bool owned;
    public bool rented;
    public int buyCost;
    public int rentCost;
    public int maxWorkers;
    public int maxItems;
    public List<HireeType> workersAssigned;
    public List<ItemForSale> itemsForSale;
}

public class BusinessManager : MonoBehaviour
{
    public static BusinessManager instance = null;
    BusinessData[] businessData = new BusinessData[(int)BusinessType.COUNT];
    HireeData[] hireeDatas = new HireeData[(int)HireeType.COUNT];

    public BusinessData getData(BusinessType type) { return businessData[(int)type]; }
    public HireeData getData(HireeType type) { return hireeDatas[(int)type]; }
    public void setOwned(BusinessType type, int setTo)
    {
        businessData[(int)type].rented = setTo == 1;
        businessData[(int)type].owned = setTo == 2;
    }

    public void changeHired(HireeType type, int changeBy)
    {
        hireeDatas[(int)type].numberHired += changeBy;
    }

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        initData();
    }

    void initData()
    {
        initBusinessData();
        initHireeData();
    }

    void initBusinessData()
    {
        businessData[(int)BusinessType.Stall] = new BusinessData()
        {
            name = "Stall",
            buyCost = 1500,
            rentCost = 200,
            owned = false,
            rented = false,
            maxItems = 4,
            maxWorkers = 2,
            workersAssigned = new List<HireeType>(),
            itemsForSale = new List<ItemForSale>()
        };

        businessData[(int)BusinessType.LittleShop] = new BusinessData()
        {
            name = "Small Shop",
            buyCost = 4000,
            owned = false,
            rented = false,
            maxItems = 15,
            maxWorkers = 5,
            workersAssigned = new List<HireeType>(),
            itemsForSale = new List<ItemForSale>()
        };

        businessData[(int)BusinessType.BigShop] = new BusinessData()
        {
            name = "Big Shop",
            buyCost = 12000,
            owned = false,
            rented = false,
            maxItems = 40,
            maxWorkers = 10,
            workersAssigned = new List<HireeType>(),
            itemsForSale = new List<ItemForSale>()
        };
    }

    void initHireeData()
    {
        hireeDatas[(int)HireeType.Adventurer] = new HireeData()
        {
            type = "Adventurer",
            description = "Adventurers can be sent on adventures and will bring you any items they find.",
            cost = 120,
            numberHired = 0,
            numberUnassigned = 0
        };

        hireeDatas[(int)HireeType.Guard] = new HireeData()
        {
            type = "Guard",
            description = "Guards can be assigned to stores and will reduce the risk of items being stolen.",
            cost = 180,
            numberHired = 0,
            numberUnassigned = 0
        };

        hireeDatas[(int)HireeType.Promoter] = new HireeData()
        {
            type = "Promoter",
            description = "A good promoter will increase a store's reputation, and can improve the sales.",
            cost = 250,
            numberHired = 0,
            numberUnassigned = 0
        };

        hireeDatas[(int)HireeType.Sales] = new HireeData()
        {
            type = "Sales Assistant",
            description = "A sales assistant can sell items from your store while you are doing something else.",
            cost = 100,
            numberHired = 0,
            numberUnassigned = 0
        };
    }

    public void handleExpenses()
    {
        foreach (HireeData hireeData in hireeDatas)
        {
            for (int i = 0; i < hireeData.numberHired; ++i)
            {
                if (!PlayerInventory.instance.changeCash(-hireeData.cost))
                {
                    int numberLost = hireeData.numberHired - i;
                    MessageQueue.addToQueue(numberLost + " of your " + hireeData.type + "s had to quit as you could not pay them.");
                    break;
                }
            }
        }

        for (int i = 0; i < businessData.Length; ++i)
        {
            if (businessData[i].rented)
            {
                if (!PlayerInventory.instance.changeCash(-businessData[i].rentCost))
                {
                    MessageQueue.addToQueue("You could not afford to keep renting the " + businessData[i].name + ".");
                    businessData[i].rented = false;
                }
            }
        }
    }
}