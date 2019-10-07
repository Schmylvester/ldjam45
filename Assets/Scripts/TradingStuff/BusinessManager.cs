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
        hireeDatas[(int)type].numberUnassigned += changeBy;
        while (hireeDatas[(int)type].numberUnassigned < 0)
        {
            for (int i = 0; i < (int)BusinessType.COUNT; ++i)
            {
                if (businessData[i].workersAssigned.Remove(type))
                {
                    hireeDatas[(int)type].numberUnassigned++;
                    break;
                }
            }
        }
    }

    public void changeUnassigned(HireeType type, int changeBy)
    {
        hireeDatas[(int)type].numberUnassigned += changeBy;
    }

    public void setCost(BusinessType business, Item item, int _cost)
    {
        for(int i = 0; i < businessData[(int)business].itemsForSale.Count; ++i)
        {
            if(businessData[(int)business].itemsForSale[i].item.name == item.name)
            {
                businessData[(int)business].itemsForSale[i] = new ItemForSale()
                {
                    item = businessData[(int)business].itemsForSale[i].item,
                    cost = _cost
                };
            }
        }
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
        for (int i = 0; i < hireeDatas.Length; ++i)
        {
            for (int j = 0; j < hireeDatas[i].numberHired; ++j)
            {
                if (!PlayerInventory.instance.changeCash(-hireeDatas[i].cost))
                {
                    int numberLost = hireeDatas[i].numberHired - j;
                    MessageQueue.addToQueue(numberLost + " of your " + hireeDatas[i].type + "s had to quit as you could not pay them.");
                    changeHired((HireeType)i, -numberLost);
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